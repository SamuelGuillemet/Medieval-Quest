using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField]
    Camera _cam;
    private float _horizontalInput;
    private float _verticalInput;

    NavMeshAgent _agent;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _agent.angularSpeed = 0f;
        _agent.acceleration = 60f;
        _agent.speed = 15f;
        _agent.stoppingDistance = 0.1f;

        _agent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        // Calculate the direction of the movement based on input and rotation
        Vector3 direction = new Vector3(_horizontalInput, 0, _verticalInput);

        _agent.SetDestination(direction + transform.position);
        UpdateRotation();

        Quaternion rotation = transform.rotation;

        Vector3 input = new Vector3(_horizontalInput, 0, _verticalInput);
        Vector3 animspeed = new Vector3(Vector3.Dot(input, transform.forward), 0, Vector3.Dot(input, transform.right));

        float horizontalSpeed = -animspeed.x;
        float verticalSpeed = -animspeed.z;

        _animator.SetFloat("HorizontalSpeed", horizontalSpeed * (_agent.velocity.magnitude / _agent.speed));
        _animator.SetFloat("VerticalSpeed", verticalSpeed * (_agent.velocity.magnitude / _agent.speed));

    }


    private void UpdateRotation()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    private void UpdateCameraPosition()
    {
        _cam.transform.parent.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}