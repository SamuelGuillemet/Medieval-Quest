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
///  - S’il peut attaquer, attaque. 
///  - Sinon se déplace vers un ennemis ayant peu de point de vie. 
/// </summary>
public class Liche : IEnemy
{
    [SerializeField] private GameObject _damageZone;
    private float _timingDamageZone = 5f;
    private Dictionary<int, float> _damageZoneTimers = new Dictionary<int, float>();
    public override void OnEnable()
    {
        base.OnEnable();

        MaxHealth = 15;
        _delayBeforeStartingAttack = WarriorTiming.TimingLock(Warrior.Liche);
        _cooldown = 10f;
        _warrior = Warrior.Liche;
        _enemyAgent.Agent.speed = 4f;

        _damageZone.SetActive(false);

        _movementCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, _enemyAgent.Agent.destination, Color.magenta);
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
                _enemyAgent.AttackAnimation();
                yield return new WaitForSeconds(.5f * _delayBeforeStartingAttack);
                _audioSource.PlayOneShot(_audioClipAttack);
                yield return new WaitForSeconds(_delayBeforeStartingAttack);

                _damageZone.SetActive(true);

                ResumeAgent();

                yield return new WaitForSeconds(_timingDamageZone);
                _damageZone.SetActive(false);

                yield return new WaitForSeconds(_cooldown - _delayBeforeStartingAttack);
            }
        }
    }

    public override IEnumerator MovementRoutine()
    {
        ResumeAgent();
        while (true)
        {
            float lowLifePercent = Mathf.Infinity;
            IEnemy lowLifeEnemy = null;

            foreach (IEnemy enemy in _gameManager.Enemies)
            {
                if (enemy.Warrior != Warrior.Liche)
                {
                    if (enemy.GetHealthInPercent() < lowLifePercent)
                    {
                        lowLifePercent = enemy.GetHealthInPercent();
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

    private void OnTriggerStay(Collider other)
    {
        int amount = 1;
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Mignon"))
        {
            if (!_damageZoneTimers.ContainsKey(other.gameObject.GetInstanceID()))
            {
                _damageZoneTimers.Add(other.gameObject.GetInstanceID(), 1f);
            }
            _damageZoneTimers[other.gameObject.GetInstanceID()] += Time.deltaTime;
            if (_damageZoneTimers[other.gameObject.GetInstanceID()] >= 1f)
            {
                _damageZoneTimers[other.gameObject.GetInstanceID()] = 0;
                if (other.gameObject.CompareTag("Player"))
                {
                    _gameManager.OnPlayerDamageTaken?.Invoke(amount);
                }
                if (other.gameObject.CompareTag("Enemy"))
                {
                    IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
                    if (enemy.Warrior != Warrior.Liche)
                    {
                        enemy.Heal(amount);
                    }
                }
                if (other.gameObject.CompareTag("Mignon"))
                {
                    Mignon mignon = other.gameObject.GetComponent<Mignon>();
                    mignon.OnDamageTaken(amount);
                }
            }
        }
    }

    public override void SpecificReset()
    {
        _damageZone.SetActive(false);
        _damageZoneTimers.Clear();
    }

}
