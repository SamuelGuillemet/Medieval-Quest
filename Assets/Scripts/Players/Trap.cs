using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private int _damage;
    private int _maxEnemyTouched;
    private int _enemyTouched;
    private float _time = 2f;
    [SerializeField] private Animator TrapDoorAnim;

    public int Damage { set => _damage = value; }
    public int MaxEnemyTouched { set => _maxEnemyTouched = value; }

    private void Update()
    {
        if (_enemyTouched == _maxEnemyTouched)
        {
            TrapDoorAnim.SetTrigger("open");
            PoolingManager.Instance.ReturnToPool(gameObject, _time + 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && _enemyTouched < _maxEnemyTouched)
        {
            _enemyTouched += 1;
            IEnemy _enemy = other.gameObject.GetComponent<IEnemy>();
            _enemy.TakeDamage(_damage, 0);
            if (other.gameObject.activeInHierarchy) _enemy.FreezeEnemy(_time);
        }
    }

    private void OnEnable()
    {
        _enemyTouched = 0;
        TrapDoorAnim.SetTrigger("close");
    }

}