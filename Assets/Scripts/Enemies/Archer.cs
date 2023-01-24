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
    public override void OnEnable()
    {
        base.OnEnable();

        MaxHealth = 5;
        _cooldown = 5f;
        _warrior = Warrior.Crossbow;
        _enemyAgent.Agent.speed = 5f;
        _enemyAgent.Agent.stoppingDistance = 0.1f;

        _muzzle = transform.Find("Muzzle");

        _movementCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }



    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, _enemyAgent.Agent.destination, new Color(0.3882353f, 0f, 0.1294118f));

        if (_enemyAgent.Agent.enabled)
        {
            if (_enemyAgent.Agent.remainingDistance < 0.1f)
            {
                StartCoroutine(FacePlayer(2f));
            }

            Vector3 start = _enemyAgent.Agent.destination + (_gameManager.Player.transform.position + Vector3.up - _enemyAgent.Agent.destination).normalized;
            start.y = _enemyAgent.Agent.destination.y + 1f;
            Vector3 direction = _gameManager.Player.transform.position + Vector3.up - start;
            Debug.DrawRay(start, direction, Color.white);
        }
    }

    public override IEnumerator AttackRoutine()
    {
        base.AttackRoutine();
        while (true)
        {
            if (!_couldAttack)
            {
                yield return null;
            }
            else
            {
                StopAgent();

                yield return FacePlayer(0.5f);

                InstantiateArrow();
                _audioSource.PlayOneShot(_audioClipAttack);

                ResumeAgent();

                yield return new WaitForSeconds(_cooldown);
            }
        }
    }


    public override IEnumerator MovementRoutine()
    {
        ResumeAgent();
        while (true)
        {
            Vector3 destination = FindPointOnEdgeOfScreen();
            int count = 0;
            while (!IsPlayerVisibleFrom(destination) && count < 200)
            {
                destination = FindPointOnEdgeOfScreen();
                count++;
            }
            _enemyAgent.Agent.SetDestination(destination);

            yield return new WaitWhile(() =>
            {
                Vector3 screenPoint = _camera.WorldToViewportPoint(_enemyAgent.Agent.destination);
                if (!IsOnScreen(_enemyAgent.Agent.destination, 0.1f))
                {
                    return false;
                }

                if (screenPoint.x > 0.3f && screenPoint.x < 0.7f && screenPoint.y > 0.3f && screenPoint.y < 0.7f)
                {
                    return false;
                }
                return true;
            });
        }
    }

    private Vector3 FindPointOnEdgeOfScreen()
    {
        float _offset = 0.1f;
        int whichSide = Random.Range(0, 4);
        float slide = Random.Range(_offset, 1 - _offset);

        Vector3 randomPointOnEdgeOfScreen = Vector3.zero;

        switch (whichSide)
        {
            case 0:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(slide, _offset, _camera.transform.position.y));
                break;
            case 1:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(slide, 1 - _offset, _camera.transform.position.y));
                break;
            case 2:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(_offset, slide, _camera.transform.position.y));
                break;
            case 3:
                randomPointOnEdgeOfScreen = _camera.ViewportToWorldPoint(new Vector3(1 - _offset, slide, _camera.transform.position.y));
                break;
        }

        Vector3 destination = new Vector3(randomPointOnEdgeOfScreen.x, 2f, randomPointOnEdgeOfScreen.z);
        return NavMesh.SamplePosition(destination, out NavMeshHit hit, 10f, NavMesh.AllAreas) ? hit.position : destination;
    }

    private bool IsPlayerVisibleFrom(Vector3 origin)
    {
        Vector3 start = origin + (_gameManager.Player.transform.position + Vector3.up - origin).normalized;
        start.y = origin.y + 1f;
        Vector3 direction = _gameManager.Player.transform.position + Vector3.up - start;

        if (Physics.Raycast(start, direction, out RaycastHit hit))
        {
            if (hit.collider.gameObject == _gameManager.Player)
            {
                return true;
            }
        }
        return false;
    }

    private void InstantiateArrow()
    {
        int nbArrow = Random.Range(1, 6);

        // Scatters the arrow to within 1 unit of the target using trigonometry
        float distancePlayer = Vector3.Distance(transform.position, _gameManager.Player.transform.position + Vector3.up);
        float angle = Mathf.Atan(1f / distancePlayer);
        float angleRange = (2f * angle) / (nbArrow + 1);

        for (int i = 0; i < nbArrow; i++)
        {
            // Split the -angle to angle range into nb arrow parts
            float angleScatter = angle - (angleRange * (i + 1));

            GameObject arrow = PoolingManager.Instance.SpawnObjectFromPool(_arrow, _muzzle.position, Quaternion.identity);
            arrow.transform.LookAt(_gameManager.Player.transform.position + Vector3.up);
            arrow.transform.RotateAround(transform.position, Vector3.up, angleScatter * Mathf.Rad2Deg);
            arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * 400f);
        }
    }
}
