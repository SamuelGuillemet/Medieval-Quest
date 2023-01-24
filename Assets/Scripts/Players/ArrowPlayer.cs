using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPlayer : MonoBehaviour
{

    private float _speed = 40f;
    private int _damage;
    private int _enemyTouched;
    private int _maxEnemyTouched;

    public int MaxEnemyTouched { set => _maxEnemyTouched = value; }
    public int Damage { set => _damage = value; }

    private void Start()
    {
        _enemyTouched = 0;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.right * _speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision infoCollision)
    {
        if (infoCollision.gameObject.tag == "Enemy")
        {
            _enemyTouched += 1;
            IEnemy _enemy = infoCollision.gameObject.GetComponent<IEnemy>();
            GameManager.Instance.OnEnemyDamageTaken?.Invoke(_damage, _enemy, 0);
        }
        Debug.Log(infoCollision.gameObject);
        if ((infoCollision.gameObject.tag != "Player" && infoCollision.gameObject.name != "Enemy") || _enemyTouched == _maxEnemyTouched)
        {
            Destroy(gameObject);
        }

    }

}
