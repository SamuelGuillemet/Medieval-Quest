using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

/// <summary>
/// 5 points de vie. 
/// Rapide.
/// Action : 
/// Tir un nombre de flèches choisi aléatoirement entre 1 et 5 qui infligent 3 dégâts. 
/// Ces flèches traversent les ennemis et ne touchent que les personnages et les murs. 
/// 5 secondes de cooldown. 
/// Comportement : 
/// S’il peut attaquer, attaque. 
/// Sinon, il choisit un point de l’arène éloigné du personnage mais visible à la caméra et s’y rend. 
/// Sa portée est limitée à la taille de l’écran (ie: Il doit apparaître à l’écran pour pouvoir tirer.)
/// </summary>
public class Archer : IEnemy
{
    [SerializeField] private GameObject _arrow;
    private Transform _muzzle;
    public override void Start()
    {
        base.Start();

        MaxHealth = 5;
        _cooldown = 5f;
        _warrior = Warrior.Crossbow;
        _enemyAgent.Agent.speed = 8f;
        _enemyAgent.Agent.stoppingDistance = 0.1f;

        _muzzle = transform.Find("Muzzle");

        _movmentCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, _enemyAgent.Agent.destination, new Color(0.3882353f, 0f, 0.1294118f));
    }

    public override IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (!_couldAttack)
            {
                yield return null;
            }
            else
            {
                _enemyAgent.Agent.isStopped = true;

                float time = 0f;
                while (true)
                {
                    Vector3 direction = _gameManager.Player.transform.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, time).eulerAngles;
                    transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                    if (time > 1.5f)
                    {
                        transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
                        break;
                    }

                    // Break if the enemy faces the player
                    if (Vector3.Angle(transform.forward, direction) < 10f)
                    {
                        break;
                    }
                }

                InstantiateArrow();

                _enemyAgent.Agent.isStopped = false;

                yield return new WaitForSeconds(_cooldown);
            }
        }
    }


    public override IEnumerator MovementRoutine()
    {
        _enemyAgent.Agent.isStopped = false;
        while (true)
        {
            Vector3 destination = FindPointOnEdgeOfScreen();
            _enemyAgent.Agent.SetDestination(destination);

            yield return new WaitForSeconds(Mathf.Clamp(_enemyAgent.Agent.remainingDistance / _enemyAgent.Agent.speed, 0f, 5f));


            yield return new WaitWhile(() =>
            {
                Vector3 screenPoint = _camera.WorldToViewportPoint(_enemyAgent.Agent.destination);
                if (screenPoint.x < 0f || screenPoint.x > 1f || screenPoint.y < 0f || screenPoint.y > 1f)
                {
                    // We check if he is close to his destination
                    if (Vector3.Distance(transform.position, _enemyAgent.Agent.destination) > 2f)
                    {
                        return true;
                    }
                    return false;
                }

                if (screenPoint.x > 0.3f && screenPoint.x < 0.7f && screenPoint.y > 0.3f && screenPoint.y < 0.7f)
                {
                    // Archer in center of screen
                    return false;
                }
                return true;
            });
        }
    }

    private Vector3 FindPointOnEdgeOfScreen()
    {
        int whichSide = Random.Range(0, 4);
        float slide = Random.Range(0f, 1f);

        Vector3 randomPointOnEdgeOfScreen = Vector3.zero;

        switch (whichSide)
        {
            case 0:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(slide, 0.15f, _camera.transform.position.y));
                break;
            case 1:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(slide, 0.75f, _camera.transform.position.y));
                break;
            case 2:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(0.15f, slide, _camera.transform.position.y));
                break;
            case 3:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(0.75f, slide, _camera.transform.position.y));
                break;
        }

        Vector3 destination = new Vector3(randomPointOnEdgeOfScreen.x, 2f, randomPointOnEdgeOfScreen.z);
        return NavMesh.SamplePosition(destination, out NavMeshHit hit, 10f, NavMesh.AllAreas) ? hit.position : destination;
    }

    private void InstantiateArrow()
    {
        int nbArrow = Random.Range(1, 6);

        // Scatters the arrow to within 1 unit of the target using trigonometry
        float distancePlayer = Vector3.Distance(transform.position, _gameManager.Player.transform.position);
        float angle = Mathf.Atan(1f / distancePlayer);
        float angleRange = (2f * angle) / (nbArrow + 1);

        for (int i = 0; i < nbArrow; i++)
        {
            // Split the -angle to angle range into nb arrow parts
            float angleScatter = angle - (angleRange * (i + 1));

            GameObject arrow = Instantiate(_arrow, _muzzle.position, Quaternion.identity); // TODO: Pooling
            arrow.transform.LookAt(_gameManager.Player.transform);

            arrow.transform.RotateAround(transform.position, Vector3.up, angleScatter * Mathf.Rad2Deg);
            arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * 400f);
        }
    }
}
