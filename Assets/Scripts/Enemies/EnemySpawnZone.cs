using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [SerializeField] private IEnemy[] _enemyPrefabs;
    public void SpawnEnemy()
    {
        IEnemy enemy = PoolingManager.Instance.SpawnRandomEnemyFromPool(transform.position, Quaternion.identity).GetComponent<IEnemy>();
        GameManager.Instance.Enemies.Add(enemy);
    }
}
