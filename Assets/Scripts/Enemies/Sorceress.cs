using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

/// <summary>
/// 3 points de vie. 
/// Rapide.
/// Action : 
/// - Lance un projectile qui inflige 5 dégats en zone de 2 unités.
/// - 10 secondes de cooldown.
/// Comportement : 
/// - Si elle est sur une tour, elle attaque.
/// - Sinon, elle choisit une tour de l'arène et se déplace sur elle.
/// - Sa portée est limitée à la taille de l’écran (ie: Elle doit apparaître à l’écran pour pouvoir tirer.)
/// </summary>
public class Sorceress : IEnemy
{
    [SerializeField] private Bomb _bomb;
    public override void OnEnable()
    {
        base.OnEnable();

        MaxHealth = 3;
        _cooldown = 10f;
        _warrior = Warrior.Sorceress;
        _delayBeforeStartingAttack = WarriorTiming.TimingLock(Warrior.Sorceress);
        _enemyAgent.Agent.speed = 7f;
        _enemyAgent.Agent.stoppingDistance = 0.5f;


        _movementCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, _enemyAgent.Agent.destination, new Color(0.1764706f, 0.9137256f, 0.7764707f));

        if (_enemyAgent.Agent.enabled)
        {
            if (_enemyAgent.Agent.remainingDistance < 0.1f)
            {
                StartCoroutine(FacePlayer(2f));
            }

            _couldAttack = _couldAttack && _enemyAgent.Agent.remainingDistance < _enemyAgent.Agent.stoppingDistance;
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

                _enemyAgent.AttackAnimation();
                yield return new WaitForSeconds(_delayBeforeStartingAttack / 2f);

                InstantiateProjectile();
                _audioSource.PlayOneShot(_audioClipAttack);

                ResumeAgent();

                yield return new WaitForSeconds(_cooldown);
            }
        }
    }

    private void InstantiateProjectile()
    {
        Bomb bomb = PoolingManager.Instance.SpawnObjectFromPool(_bomb.gameObject, transform.position + Vector3.up * 2f, Quaternion.identity).GetComponent<Bomb>();
        bomb.Range = 2f;
        bomb.Damage = 5;
        bomb.SetTarget(_gameManager.Player.transform);
    }

    public override IEnumerator MovementRoutine()
    {
        ResumeAgent();
        while (true)
        {
            Vector3 destination = FindTowerOnScreen();
            _enemyAgent.Agent.SetDestination(destination);

            yield return new WaitWhile(() =>
            {
                return IsOnScreen(_enemyAgent.Agent.destination, 0.05f);
            });
        }
    }

    private Vector3 FindTowerOnScreen()
    {
        List<Vector3> towers = _gameManager.GetTowers();
        Vector3 closestTower = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (Vector3 tower in towers)
        {
            Vector3 tempDestination = tower + new Vector3(2f, 5f, 2f);
            Vector3 destination = NavMesh.SamplePosition(tempDestination, out NavMeshHit hit, 2f, NavMesh.AllAreas) ? hit.position : tempDestination;

            if (IsOnScreen(destination, 0.1f))
            {
                return destination;
            }
            else if (Vector3.Distance(_gameManager.Player.transform.position, destination) < closestDistance)
            {
                closestDistance = Vector3.Distance(_gameManager.Player.transform.position, destination);
                closestTower = destination;
            }
        }

        return closestTower;
    }
}
