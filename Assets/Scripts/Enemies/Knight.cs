using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

/// <summary>
/// 10 points de vie. 
/// Vitesse moyenne.
/// Action : 
/// - Coup d'épée qui fait 5 dégâts dans une sphère devant le Soldat. 
/// - 1 seconde de cooldown.
/// Comportement : 
/// - Avance vers le personnage. 
/// - Attaque dès que le personnage est touchable et que l’attaque est possible. 
/// - Ne se déplace pas pendant l’attaque.
/// </summary>
public class Knight : IEnemy
{
    private NavMeshAgent _playerAgent;
    public override void OnEnable()
    {
        base.OnEnable();

        MaxHealth = 10;
        _delayBeforeStartingAttack = WarriorTiming.TimingLock(Warrior.Knight);
        _warrior = Warrior.Knight;
        _cooldown = 1f;
        _enemyAgent.Agent.speed = 6f;
        _enemyAgent.Agent.stoppingDistance = 0.5f;

        _playerAgent = _gameManager.Player.GetComponent<NavMeshAgent>();

        _movementCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, _enemyAgent.Agent.destination, new Color(0.3019608f, 0.3254902f, 0.3529412f));

        _couldAttack = _couldAttack && CouldAttackPlayer() != null;
    }

    public override IEnumerator AttackRoutine()
    {
        base.AttackRoutine();
        while (true)
        {
            if (!_couldAttack)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                StopAgent();
                _enemyAgent.AttackAnimation();
                yield return new WaitForSeconds(0.5f * _delayBeforeStartingAttack);
                _audioSource.PlayOneShot(_audioClipAttack);

                if (CouldAttackPlayer() != null) _gameManager.OnPlayerDamageTaken?.Invoke(5);

                yield return new WaitForSeconds(_delayBeforeStartingAttack);


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
            // Set the destionation 1 block away from the player
            Vector3 playerPos = _playerAgent.transform.position;

            Vector3 direction = (playerPos - transform.position).normalized;
            Vector3 destination = playerPos - (direction * _enemyAgent.Agent.stoppingDistance);

            _enemyAgent.Agent.SetDestination(destination);

            yield return new WaitUntil(() => _playerAgent.velocity.magnitude > 0.1f);
        }
    }

    private GameObject CouldAttackPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                return collider.gameObject;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3019608f, 0.3254902f, 0.3529412f);
        Gizmos.DrawWireSphere(transform.position + transform.forward, 1f);
    }
}
