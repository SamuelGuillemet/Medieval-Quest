using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

[RequireComponent(typeof(NavMeshAgent), typeof(WarriorController))]
public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private WarriorController _warriorController;
    public NavMeshAgent Agent { get => _agent; }
    public Rigidbody Rigidbody { get => _rigidbody; }

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _warriorController = GetComponent<WarriorController>();
        _rigidbody = GetComponent<Rigidbody>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.angularSpeed = 800f;
        _agent.acceleration = 60f;
        _agent.speed = 4f;
        _agent.stoppingDistance = 1.5f;

        _warriorController.AllowInput(false);
        _warriorController.LockJump(true);
    }

    void Update()
    {
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

    public void AttackAnimation()
    {
        _warriorController.Attack1();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            GameManager.Instance.OnEnemyKilled.Invoke(GetComponent<IEnemy>());
        }
    }

}