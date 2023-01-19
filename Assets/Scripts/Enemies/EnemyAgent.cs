using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private WarriorController _warriorController;
    public NavMeshAgent Agent { get => _agent; }

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _warriorController = GetComponent<WarriorController>();

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

    // public void StopFollowingPlayer()
    // {
    //     Debug.Log("Killed : " + _followPlayerCoroutine.GetHashCode() + " " + gameObject.name);
    //     _followPlayer = false;
    //     if (_followPlayerCoroutine != null) StopCoroutine(_followPlayerCoroutine);
    //     _followPlayerCoroutine = null;
    // }

    // private IEnumerator FollowPlayerEnumerator()
    // {
    //     while (true)
    //     {
    //         if (!_followPlayer) yield break;

    //         NavMeshHit hit;
    //         _playerAgent.SamplePathPosition(NavMesh.AllAreas, 1f, out hit);

    //         // If the player is on the PlayerZone Area, stop the agent
    //         if (hit.mask == 8)
    //         {
    //             _agent.isStopped = true;
    //             yield return null;
    //         }
    //         else
    //         {
    //             _agent.isStopped = false;
    //             // Set the destionation 1 block away from the player
    //             Vector3 playerPos = _player.transform.position;

    //             Vector3 direction = (playerPos - transform.position).normalized;
    //             Vector3 destination = playerPos - (direction * _agent.stoppingDistance);

    //             _agent.SetDestination(destination);
    //             Debug.DrawLine(transform.position, destination, Color.red);

    //             yield return new WaitUntil(() => _playerAgent.velocity.magnitude > 0.1f);
    //         }
    //     }
    // }

    public void AttackAnimation()
    {
        _warriorController.Attack1();
    }

}