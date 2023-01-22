using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int _damage = 3;
    [SerializeField] private Rigidbody _rigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.OnPlayerDamageTaken?.Invoke(_damage);
            DisableObject();
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            DisableObject();
        }
    }

    private void Update()
    {
        if (transform.position.y < -1) DisableObject();
        if (transform.position.y > 10) DisableObject();
    }

    private void DisableObject()
    {
        PoolingManager.Instance.ReturnToPool(this.gameObject);
        _rigidbody.velocity = Vector3.zero;
    }
}
