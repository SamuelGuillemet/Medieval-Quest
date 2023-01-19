using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int _damage = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            DisableObject();
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            DisableObject();
        }
    }

    private void Update()
    {
        if (transform.position.y < -10) DisableObject();
        if (transform.position.y > 10) DisableObject();
    }

    private void DisableObject()
    {
        Destroy(gameObject); //TODO: Pooling
    }
}
