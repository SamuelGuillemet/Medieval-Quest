using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    public Transform camera;
    public float speed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 v3 = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.Q))
        {
            v3 -= new Vector3(camera.right.x, 0, camera.right.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            v3 += new Vector3(camera.right.x, 0, camera.right.z);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            v3 += new Vector3(camera.forward.x, 0, camera.forward.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            v3 -= new Vector3(camera.forward.x, 0, camera.forward.z);
        }

        transform.Translate(v3 * Time.deltaTime * speed);
    
        
        Quaternion toRotation = Quaternion.LookRotation(v3);

        Debug.Log(toRotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        if (v3 == Vector3.zero)
        {
            animator.SetBool("IsWalking", false);

        } else
        {
            animator.SetBool("IsWalking", true);
        }
    }
}
