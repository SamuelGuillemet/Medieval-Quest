using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private int _damage;
    private int _maxEnemyTouched;
    private int _enemyTouched;
    private float _time = 2f;
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
            Destroy(gameObject, _time + 0.5f);
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