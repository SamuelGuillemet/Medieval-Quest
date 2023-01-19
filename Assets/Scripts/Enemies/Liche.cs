using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarriorAnimsFREE;

/// <summary>
/// 15 points de vie. 
/// Lent.
/// Action :  
///  - Créer une zone de magie au sol autour d’elle pendant 5 secondes. 
///  - La zone inflige 1 dégâts par secondes au personnage s’il se trouve dedans, et soignent de 1 pv par seconde les ennemis n’étant pas des Liches. 
///  - Cooldown 10 secondes.
/// Comportement : 
///  - S’il peut attaquer, attaque. Sinon se déplace vers un ennemis ayant peu de point de vie. 
/// </summary>
public class Liche : IEnemy
{
    [SerializeField] private GameObject _damageZone;
    private float _timingDamageZone = 5f;
    public override void Start()
    {
        base.Start();

        MaxHealth = 15;
        _delayBeforeStartingAttack = WarriorTiming.TimingLock(Warrior.Liche);
        _cooldown = 10f;
        _warrior = Warrior.Liche;
        _enemyAgent.Agent.speed = 4f;

        _damageZone.SetActive(false);

        _movmentCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, _enemyAgent.Agent.destination, Color.magenta);
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
                _enemyAgent.AttackAnimation();
                yield return new WaitForSeconds(1.5f * _delayBeforeStartingAttack);

                _damageZone.SetActive(true);

                _enemyAgent.Agent.isStopped = false;

                yield return new WaitForSeconds(_timingDamageZone);
                _damageZone.SetActive(false);

                yield return new WaitForSeconds(_cooldown - _delayBeforeStartingAttack);
            }
        }
    }

    public override IEnumerator MovementRoutine()
    {
        _enemyAgent.Agent.isStopped = false;
        while (true)
        {
            float lowLifePercent = Mathf.Infinity;
            GameObject lowLifeEnemy = null;

            foreach (var enemy in _gameManager.Enemies)
            {
                IEnemy enemyScript = enemy.GetComponent<IEnemy>();

                if (enemyScript.Warrior != Warrior.Liche)
                {
                    if (enemyScript.GetHealthInPercent() < lowLifePercent)
                    {
                        lowLifePercent = enemyScript.GetHealthInPercent();
                        lowLifeEnemy = enemy;
                    }
                }
            }

            if (lowLifeEnemy == null)
            {
                _enemyAgent.Agent.SetDestination(_gameManager.Player.transform.position);
            }
            else
            {
                _enemyAgent.Agent.SetDestination(lowLifeEnemy.transform.position);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EffectDamageZone(1));
        }
        if (other.gameObject.CompareTag("Enemy")
            && other.gameObject.GetComponent<IEnemy>().Warrior != Warrior.Liche)
        {
            StartCoroutine(EffectDamageZoneOnEnemy(other.gameObject.GetComponent<IEnemy>(), 1));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(EffectDamageZone(1));
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            StopCoroutine(EffectDamageZoneOnEnemy(other.gameObject.GetComponent<IEnemy>(), 1));
        }
    }

    IEnumerator EffectDamageZoneOnEnemy(IEnemy enemy, int care)
    {
        while (true)
        {
            if (!_damageZone.activeInHierarchy) yield break;
            enemy.TakeDamage(-1 * care);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator EffectDamageZone(int damage)
    {
        while (true)
        {
            if (!_damageZone.activeInHierarchy) yield break;
            Debug.Log("Damage on player");
            yield return new WaitForSeconds(1f);
        }
    }

    public override void SpecificReset()
    {
        _damageZone.SetActive(false);
    }

}
