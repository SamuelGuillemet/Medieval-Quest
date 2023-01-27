﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private int _damage;
    public int Damage { set => _damage = value; }
    [SerializeField] GameObject _explosion;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Renderer _renderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            return;
        }
        if (other.gameObject.tag == "Enemy")
        {
            IEnemy _enemy = other.gameObject.GetComponent<IEnemy>();
            GameManager.Instance.OnEnemyDamageTaken?.Invoke(_damage, _enemy, 1);
        }
        Explode();
    }

    void Explode()
    {
        _explosion.SetActive(true);
        _rigidbody.velocity = Vector3.zero;
        _renderer.enabled = false;
        PoolingManager.Instance.ReturnToPool(gameObject, 0.5f);
    }

    public void SetMovement()
    {
        _rigidbody.AddForce(transform.forward * 800f);
    }

    private void OnDisable()
    {
        _explosion.SetActive(false);
        _renderer.enabled = true;
    }
}
