using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // This script moves the player using the navMeshAgent when clicking on the ground

    NavMeshAgent agent;
    [SerializeField]
    private Camera _cam;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = 800f;
        agent.acceleration = 60f;
        agent.speed = 15f;
        agent.stoppingDistance = 1f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        _cam.transform.position = new Vector3(x: transform.position.x, y: 33, z: transform.position.z - 10);
    }
}
