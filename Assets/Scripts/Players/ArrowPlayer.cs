using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPlayer : MonoBehaviour
{
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] Rigidbody _rigidbody;
    private float _speed = 40f;
    private int _damage;
    private int _enemyTouched;
    private int _maxEnemyTouched;

    public int MaxEnemyTouched { set => _maxEnemyTouched = value; }
    public int Damage { set => _damage = value; }

    void Update()
    {
        _rigidbody.velocity = transform.forward * _speed;

        if (transform.position.y < -10)
        {
            PoolingManager.Instance.ReturnToPool(gameObject);
        }
    }

    void OnCollisionEnter(Collision infoCollision)
    {
        if (infoCollision.gameObject.tag == "Enemy" && _enemyTouched < _maxEnemyTouched)
        {
            _enemyTouched += 1;
            IEnemy _enemy = infoCollision.gameObject.GetComponent<IEnemy>();
            GameManager.Instance.OnEnemyDamageTaken?.Invoke(_damage, _enemy, 0);
        }
        if ((infoCollision.gameObject.tag != "Player" && infoCollision.gameObject.name != "Enemy") || _enemyTouched == _maxEnemyTouched)
        {
            PoolingManager.Instance.ReturnToPool(gameObject);
        }
    }

    private void OnEnable()
    {
        _enemyTouched = 0;
        _trailRenderer.Clear();
        _rigidbody.velocity = Vector3.zero;
    }
}
