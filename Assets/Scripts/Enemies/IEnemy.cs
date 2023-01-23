using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarriorAnimsFREE;


[RequireComponent(typeof(EnemyAgent))]
public class IEnemy : MonoBehaviour
{
    protected int _health = 100;
    protected int _maxHealth = 100;
    protected float _delayBeforeStartingAttack = 0.5f;
    protected float _cooldown = 0f;
    protected bool _couldAttack = false;
    protected GameManager _gameManager;
    protected AudioManager _audioManager;
    protected Camera _camera;
    protected EnemyAgent _enemyAgent;
    protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _audioClipAttack;
    protected Warrior _warrior = Warrior.Archer;

    public int MaxHealth { set => _maxHealth = _health = value; }
    public int Health { get => _health; set => _health = value; }
    public Warrior Warrior { get => _warrior; set => _warrior = value; }
    public EnemyAgent EnemyAgent { get => _enemyAgent; set => _enemyAgent = value; }

    protected Coroutine _attackCoroutine;
    protected Coroutine _movementCoroutine;

    public virtual void OnEnable()
    {
        _enemyAgent = GetComponent<EnemyAgent>();
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        _camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();

        _couldAttack = false;
    }

    public virtual void Update()
    {
        // Could not attack if outside of camera view
        _couldAttack = IsOnScreen(transform.position, 0.05f);
    }

    public float GetHealthInPercent()
    {
        return (float)_health / _maxHealth;
    }

    public void TakeDamage(int damage, float knockback)
    {
        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
        _audioManager.PlaySound("EnemyHit");

        if (_health <= 0)
        {
            _gameManager.OnEnemyKilled.Invoke(this);
        }
        else
        {
            if (knockback > 0)
            {
                StartCoroutine(DamageTakenKnockback((transform.position - _gameManager.Player.transform.position).normalized, knockback));
            }
        }
    }

    public void Heal(int amount)
    {
        if (_health + amount > _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health += amount;
        }
    }

    public void RepusleEnemy(Vector3 direction, float strenght = 1f)
    {
        StartCoroutine(DamageTakenKnockback(direction, strenght));
    }

    IEnumerator DamageTakenKnockback(Vector3 direction, float strenght = 1f)
    {
        direction.y = 1f;
        _enemyAgent.Agent.enabled = false;
        _enemyAgent.Rigidbody.isKinematic = false;
        _couldAttack = false;
        StopCoroutine(_movementCoroutine);

        _enemyAgent.Rigidbody.AddForce(direction * 35f * strenght, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Physics.Raycast(transform.position, Vector3.down, 1.1f, LayerMask.GetMask("Floor")));
        ResetKnockback();
        _movementCoroutine = StartCoroutine(MovementRoutine());
        _couldAttack = true;
    }

    private void ResetKnockback()
    {
        _enemyAgent.Rigidbody.velocity = Vector3.zero;
        _enemyAgent.Rigidbody.angularVelocity = Vector3.zero;
        _enemyAgent.Rigidbody.isKinematic = true;
        _enemyAgent.Agent.enabled = true;
    }

    public void StopAgent()
    {
        if (_enemyAgent.Agent.enabled)
        {
            _enemyAgent.Agent.isStopped = true;
        }
    }

    public void ResumeAgent()
    {
        if (_enemyAgent.Agent.enabled)
        {
            _enemyAgent.Agent.isStopped = false;
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
        _movementCoroutine = StartCoroutine(MovementRoutine());
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public virtual IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
    }

    public virtual IEnumerator MovementRoutine()
    {
        yield return null;
    }

    public virtual void SpecificReset() { }

    protected IEnumerator FacePlayer(float maxTime)
    {
        float time = 0f;
        while (true)
        {
            Vector3 direction = _gameManager.Player.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, time).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            if (time > maxTime)
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
    }

    protected bool IsOnScreen(Vector3 position, float offset = 0f)
    {
        Vector3 screenPoint = _camera.WorldToViewportPoint(position);
        return screenPoint.x > 0f + offset && screenPoint.x < 1f - offset && screenPoint.y > 0f + offset && screenPoint.y < 1f - 2f * offset;
    }

    private void OnDisable()
    {
        ResetKnockback();
        StopAllCoroutines();
    }

    public void FreezeEnemy(float time)
    {
        StartCoroutine(FreezeEnemyRoutine(time));
    }

    IEnumerator FreezeEnemyRoutine(float time = 1f)
    {
        Debug.Log("Freeze enemy");
        DeactivateEnemy();
        yield return new WaitForSeconds(time);
        ActivateEnemy();
    }
}
