using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private GameObject _player;
    private NavMeshAgent _playerAgent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.angularSpeed = 800f;
        _agent.acceleration = 60f;
        _agent.speed = 6f;
        _agent.stoppingDistance = 1f;

        _playerAgent = _player.GetComponent<NavMeshAgent>();

        StartCoroutine(FollowPlayer());
    }

    IEnumerator FollowPlayer()
    {
        while (true)
        {
            // Set the destionation 1 block away from the player
            Vector3 playerPos = _player.transform.position;

            Vector3 direction = (playerPos - transform.position).normalized;
            Vector3 destination = playerPos - (direction * _agent.stoppingDistance);

            _agent.SetDestination(destination);
            Debug.DrawLine(transform.position, destination, Color.red);

            yield return new WaitUntil(() => _playerAgent.velocity.magnitude > 0.1f);
        }
    }
}