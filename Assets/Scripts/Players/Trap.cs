using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private int _damage;
    private int _maxEnemyTouched;
    private int _enemyTouched;
    private float _time = 3f;
    public Animator TrapDoorAnim;

    public int Damage { set => _damage = value; }
    public int MaxEnemyTouched { set => _maxEnemyTouched = value; }

    // Use this for initialization
    void Awake()
    {
        _enemyTouched = 0;
        TrapDoorAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_enemyTouched == _maxEnemyTouched)
        {
            TrapDoorAnim.SetBool("open", true);
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _enemyTouched += 1;
            IEnemy _enemy = other.gameObject.GetComponent<IEnemy>();
            _enemy.TakeDamage(_damage, 0);
            _enemy.FreezeEnemy(_time);
        }
    }

}