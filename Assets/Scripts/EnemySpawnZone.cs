using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    public void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy");
        GameObject enemyToInstantiate = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        GameObject enemy = Instantiate(enemyToInstantiate, transform.position, Quaternion.identity);
        GameManager.Instance.Enemies.Add(enemy);
    }
}
