using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 v3 = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.Q))
        {
            v3 += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            v3 += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            v3 += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            v3 -= Vector3.forward;
        }

        transform.Translate(v3 * Time.deltaTime * speed);
    }
}
