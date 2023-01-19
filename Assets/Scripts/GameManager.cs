using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public PlayerType SelectedPlayer = PlayerType.None;
    private GameObject _player;
    public GameObject Player { get => _player; set => _player = value; }

    private static GameManager _instance;
    public static GameManager Instance { get { if (_instance == null) _instance = GameObject.FindObjectOfType<GameManager>(); return _instance; } }


    [SerializeField]
    private int _seed = 1;

    private int _numberOfEnemies = 0;
    private EnemySpawnZone[] _enemySpawnZones;

    private int[] _fibonnaciSuite = new int[] { 2, 2, 3, 5, 8, 13, 21, 34, 55, 89 };
    [HideInInspector] public int WaveNumber = 0;

    public List<GameObject> Enemies;

    public UnityEventGameObject OnEnemyKilled;
    public UnityEventFloat OnDamageTaken;

    private GameUI _gameUI;

    void Awake()
    {
        Enemies = new List<GameObject>();

        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        PrefabsGenerator prefabsGenerator = FindObjectOfType<PrefabsGenerator>();

        _gameUI = FindObjectOfType<GameUI>();

        mapGenerator.Seed = _seed;
        prefabsGenerator.NumberOfEnemySpawnZone = _numberOfEnemySpawnZone;

        mapGenerator.GenerateMap();

        _enemySpawnZones = FindObjectsOfType<EnemySpawnZone>();

        OnEnemyKilled.AddListener(OnEnemyKilledCallback);
        OnDamageTaken.AddListener(OnEnemyDamagedCallback);
    }

    void Start()
    {
        StartCoroutine(EnemiesWave());
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator EnemiesWave()
    {
        yield return new WaitForSeconds(3f);
        while (WaveNumber < _fibonnaciSuite.Length)
        {
            _gameUI.UpdateWaveText(WaveNumber, _fibonnaciSuite[WaveNumber]);
            int count = 0;
            while (count < _fibonnaciSuite[WaveNumber])
            {
                int spawnZone1 = Random.Range(0, _numberOfEnemySpawnZone);
                _enemySpawnZones[spawnZone1].SpawnEnemy();
                count++;
                _numberOfEnemies++;
                _gameUI.UpdateEnnemiesBar(_numberOfEnemies);

                if (count == _fibonnaciSuite[WaveNumber])
                    break;

                int spawnZone2 = Random.Range(0, _numberOfEnemySpawnZone);
                while (spawnZone1 == spawnZone2)
                    spawnZone2 = Random.Range(0, _numberOfEnemySpawnZone);
                _enemySpawnZones[spawnZone2].SpawnEnemy();
                count++;
                _numberOfEnemies++;
                _gameUI.UpdateEnnemiesBar(_numberOfEnemies);

                yield return new WaitForSeconds(2f);
            }

            WaveNumber++;
            yield return new WaitUntil(() => _numberOfEnemies == 0);
            yield return new WaitForSeconds(5f);
        }
    }

    private void OnEnemyKilledCallback(GameObject enemy)
    {
        _numberOfEnemies--;
        _gameUI.UpdateEnnemiesBar(_numberOfEnemies);
        Enemies.Remove(enemy);
        Destroy(enemy);
    }

    private void OnEnemyDamagedCallback(int damage, GameObject enemy)
    {
        Debug.Log("Enemy: " + enemy.name + " - Health: " + enemy.GetComponent<IEnemy>().Health);
    }


}

public enum PlayerType
{
    None,
    Archer,
    Guerrier,
    Mage
}

[System.Serializable] public class UnityEventGameObject : UnityEvent<GameObject> { }
[System.Serializable] public class UnityEventFloat : UnityEvent<int, GameObject> { }
