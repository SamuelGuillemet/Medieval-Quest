using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private Mage _player;
    private int enemyTouched;
    private float _speed = 600f;
    private int _damage = 2;
    private float _orbRepulsion;


    // Update is called once per frame
    void Update()
    {
        transform.parent.position = _player.transform.position;
        transform.RotateAround(transform.parent.position, Vector3.up, _speed * Time.deltaTime);

        if (enemyTouched == _player.MaxEnemyTouched)
        {
            PoolingManager.Instance.ReturnToPool(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            GameManager.Instance.OnEnemyDamageTaken?.Invoke(_damage, enemy, _orbRepulsion);
            enemyTouched += 1;
        }

    }

    private void OnEnable()
    {
        enemyTouched = 0;
        _player = (Mage)GameManager.Instance.Player;
        _orbRepulsion = _player.OrbRepulsion;
        transform.position = _player.transform.position + new Vector3(0, 2, 2);
    }

}
