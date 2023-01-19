using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

public class HeroController : MonoBehaviour
{
    private enum AgentRangeMode
    {
        Walk,
        Dash
    }

    [SerializeField] Camera _cam;
    private float _horizontalInput;
    private float _verticalInput;

    private bool _tryToClimb = false;
    private AgentRangeMode _agentRange = AgentRangeMode.Walk;
    // Return -1f for AgentRangeMode.Walk and -0.1f pour AgentRangeMode.Dash
    private float AgentRange => _agentRange == AgentRangeMode.Walk ? -1f : -0.1f;

    private NavMeshAgent _agent;
    private WarriorController _warriorController;


    Animator _animator;

    public bool isMoving;


    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _warriorController = GetComponent<WarriorController>();

        _agent.angularSpeed = 0f;
        _agent.acceleration = 60f;
        _agent.speed = 10f;
        _agent.stoppingDistance = 0.1f;

        _agent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {
        CalculateDestinationWithInputFromTopWorld();
        UpdateRotation();

        _cam.transform.position = new Vector3(x: transform.position.x, y: 33, z: transform.position.z - 10);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _agentRange = AgentRangeMode.Dash;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _agentRange = AgentRangeMode.Walk;
        }

        if (_animator)
        {
            AdjustAnimations();

            if (Input.GetMouseButtonDown(0))
            {
                _warriorController.Attack1();
            }
        }

    }

    private void AdjustAnimations()
    {
        _animator.SetFloat("Velocity", _agent.velocity.magnitude / _agent.speed);

        if (_agent.velocity.magnitude > 0.1f)
        {
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
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

    private void CalculateDestinationWithClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }

    [Obsolete("Use CalculateDestinationWithInputFromTopWorld instead. This function was deprecated because it is not working properly.")]
    private void CalculateDestinationWithRayFromThePlayer()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (_horizontalInput == 0 && _verticalInput == 0)
        {
            isMoving = false; 
        } else
        {
            isMoving = true;
        }


        // Calculate the direction of the movement based on input and rotation
        Vector3 direction = new Vector3(_horizontalInput, AgentRange, _verticalInput);

        Debug.DrawRay(transform.position, direction, Color.green);


        // Compute a raycast from the player with the direction to get the y position of the ground to set the right y postion for the agent
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Vector3 destination = new Vector3(transform.position.x + _horizontalInput, hit.point.y, transform.position.z + _verticalInput);

            if (_agentRange == AgentRangeMode.Dash) destination = hit.point;

            _agent.SetDestination(destination);

            Debug.DrawLine(transform.position, destination, Color.blue);
        }


         Quaternion rotation = transform.rotation;

         Vector3 input = new Vector3(_horizontalInput, 0, _verticalInput);
         Vector3 animspeed = new Vector3(Vector3.Dot(input, transform.forward), 0, Vector3.Dot(input, transform.right));


         float horizontalSpeed = -animspeed.x;
         float verticalSpeed = -animspeed.z;

        _animator.SetFloat("HorizontalSpeed", horizontalSpeed * (_agent.velocity.magnitude / _agent.speed));
        _animator.SetFloat("VerticalSpeed", verticalSpeed * (_agent.velocity.magnitude / _agent.speed));
    }

    private void CalculateDestinationWithInputFromTopWorld()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        // Calculate the direction of the movement based on input and rotation
        Vector3 origin = new Vector3(_horizontalInput, 0f, _verticalInput);

        if (Mathf.Abs(_horizontalInput) < 0.1f && Mathf.Abs(_verticalInput) < 0.1f) return;

        if (_agentRange == AgentRangeMode.Dash)
        {
            origin *= 5f;
        }
        else
        {
            origin *= 2f;
        }

        if (_tryToClimb) origin.y = 10f;

        origin += transform.position;
        origin.y += 2.5f;

        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit))
        {
            Vector3 destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            Debug.DrawLine(transform.position, destination, Color.blue);
            _agent.SetDestination(destination);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            _tryToClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            _tryToClimb = false;
        }
    }
}
