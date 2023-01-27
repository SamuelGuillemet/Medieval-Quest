using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

public class Mignon : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private WarriorController _warriorController;
    private Animator _animator;
    private int _health;
    private int _maxHealth;
    private int _care;
    private float _lifeTime;
    private float _timeToCare;
    private float _delayBetweenCare;

    public int MaxHealth { set => _maxHealth = _health = value; }
    public float DelayBetweenCare { set => _delayBetweenCare = value; }
    public float Speed { set => _agent.speed = value; }


    void Start()
    {
        _care = 2;
        _lifeTime = 6f;
        _timeToCare = 0f;

        _animator = GetComponentInChildren<Animator>();
        _warriorController = GetComponent<WarriorController>();

        _agent.angularSpeed = 800f;
        _agent.acceleration = 60f;
        _agent.speed = 4f;
        _agent.stoppingDistance = 1.5f;

        _warriorController.AllowInput(false);
        _warriorController.LockJump(true);

        _warriorController.Attack1();

        StartCoroutine(FollowPlayer());
    }

    void Update()
    {
        _lifeTime -= Time.deltaTime;
        _timeToCare -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            PoolingManager.Instance.ReturnToPool(gameObject);
        }

        _animator.SetFloat("Velocity", _agent.velocity.magnitude / _agent.speed);

        if (_agent.velocity.magnitude > 0.05f)
        {
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_timeToCare <= 0)
            {
                _timeToCare = _delayBetweenCare;
                GameManager.Instance.OnPlayerHealed?.Invoke(_care);
            }
        }
    }

    IEnumerator FollowPlayer()
    {
        while (true)
        {
            _agent.SetDestination(GameManager.Instance.Player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnDamageTaken(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            PoolingManager.Instance.ReturnToPool(gameObject);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
