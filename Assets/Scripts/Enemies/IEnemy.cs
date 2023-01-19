using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarriorAnimsFREE;



public class IEnemy : MonoBehaviour
{
    protected int _health = 100;
    protected int _maxHealth = 100;
    public int MaxHealth { set => _maxHealth = _health = value; }
    protected float _delayBeforeStartingAttack = 0.5f;
    protected float _cooldown = 0f;
    protected bool _couldAttack = false;
    protected GameManager _gameManager;
    protected Camera _camera;
    protected EnemyAgent _enemyAgent;
    protected Warrior _warrior = Warrior.Archer;

    public int Health { get => _health; set => _health = value; }
    public Warrior Warrior { get => _warrior; set => _warrior = value; }

    protected Coroutine _attackCoroutine;
    protected Coroutine _movmentCoroutine;

    public virtual void Start()
    {
        _enemyAgent = GetComponent<EnemyAgent>();
        _gameManager = GameManager.Instance;
        _camera = Camera.main;
    }

    public virtual void Update()
    {
        // Could not attack if outside of camera view
        Vector3 screenPoint = _camera.WorldToViewportPoint(transform.position);
        if (screenPoint.x < 0f || screenPoint.x > 1f || screenPoint.y < 0f || screenPoint.y > 1f)
        {
            _couldAttack = false;
        }
        else
        {
            _couldAttack = true;
        }
    }

    public float GetHealthInPercent()
    {
        return (float)_health / _maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (_health <= 0 || _health >= _maxHealth)
        {
            return;
        }
        _health -= damage;
        _gameManager.OnDamageTaken?.Invoke(damage, gameObject);

        if (_health <= 0)
        {
            _gameManager.OnEnemyKilled.Invoke(gameObject);
        }
    }

    public void DeactivateEnemy()
    {
        StopAllCoroutines();
        _enemyAgent.Agent.ResetPath();
        _enemyAgent.Agent.isStopped = true;
        SpecificReset();
    }

    public void ActivateEnemy()
    {
        _movmentCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public virtual IEnumerator AttackRoutine()
    {
        yield return null;
    }

    public virtual IEnumerator MovementRoutine()
    {
        yield return null;
    }

    public virtual void SpecificReset() { }

}
