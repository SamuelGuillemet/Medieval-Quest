using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private Mage _player;
    private int enemyTouched;
    private float _speed = 300f;
    private int _damage = 2;
    private bool _orbRepulsion;
    public bool OrbRepulsion { set => _orbRepulsion = value; }

    void Start()
    {
        _player = (Mage)GameManager.Instance.Player;
        enemyTouched = 0;
    }


    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_player.transform.position, Vector3.up, _speed * Time.deltaTime);

        if (enemyTouched == _player.MaxEnemyTouched)
        {
            Destroy(gameObject); // TODO Pooling
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            GameManager.Instance.OnEnemyDamageTaken?.Invoke(_damage, enemy, _orbRepulsion ? 2 : 0);
            enemyTouched += 1;
        }

    }

}
