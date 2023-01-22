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
    private MapGenerator _mapGenerator;

    private PrefabsGenerator _prefabsGenerator;

    [SerializeField]
    private int _seed = 1;

    private int _numberOfEnemies = 0;
    private EnemySpawnZone[] _enemySpawnZones;

    private int[] _fibonnaciSuite = new int[] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };
    [HideInInspector] public int WaveNumber = 0;

    public List<IEnemy> Enemies;

    public UnityEventIEnemy OnEnemyKilled;
    public UnityEventIntIEnemy OnEnemyDamageTaken;
    public UnityEventInt OnPlayerDamageTaken;
    public UnityEventExperienceOrb OnOrbCollected;

    private int _playerExperience = 0;
    public int PlayerExperience { get => _playerExperience; set => _playerExperience = value; }
    private int _playerLevel = 1;
    public int PlayerLevel { get => _playerLevel; set => _playerLevel = value; }
    public int PlayerExperienceToNextLevel { get => Mathf.CeilToInt(8 * Mathf.Pow(_playerLevel, 1.5f)); }

    private AudioManager _audioManager;

    private GameUI _gameUI;

    void Awake()
    {
        Enemies = new List<IEnemy>();

        _mapGenerator = FindObjectOfType<MapGenerator>();
        _prefabsGenerator = FindObjectOfType<PrefabsGenerator>();

        _gameUI = FindObjectOfType<GameUI>();

        _mapGenerator.Seed = _seed;
        _prefabsGenerator.NumberOfEnemySpawnZone = _numberOfEnemySpawnZone;

        _mapGenerator.GenerateMap();

        _enemySpawnZones = FindObjectsOfType<EnemySpawnZone>();
        _player = GameObject.FindGameObjectWithTag("Player");

        OnEnemyKilled.AddListener(OnEnemyKilledCallback);
        OnEnemyDamageTaken.AddListener(OnEnemyDamagedCallback);
        OnPlayerDamageTaken.AddListener(OnPlayerDamagedCallback);
        OnOrbCollected.AddListener(OnOrbCollectedCallback);

        _audioManager = AudioManager.Instance;
    }

    void Start()
    {
        StartCoroutine(EnemiesWave());
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

    private void OnEnemyKilledCallback(IEnemy enemy)
    {
        _numberOfEnemies--;
        PoolingManager.Instance.ReturnToPool(enemy.gameObject);
        _gameUI.UpdateEnnemiesBar(_numberOfEnemies);
        Enemies.Remove(enemy);

        StartCoroutine(SpawnExperienceOrb(enemy.transform.position));
    }

    private void OnEnemyDamagedCallback(int damage, IEnemy enemy)
    {
        Debug.Log("Enemy: " + enemy.name + " - Health: " + enemy.Health);
    }

    private void OnPlayerDamagedCallback(int damage)
    {
        Debug.Log("Player: " + Player.name + " - Damage: " + damage);
    }

    private void OnOrbCollectedCallback(ExperienceOrb orb)
    {
        PoolingManager.Instance.ReturnToPool(orb.gameObject);

        PlayerExperience += orb.ExperienceValue;
        if (PlayerExperience >= PlayerExperienceToNextLevel)
        {
            PlayerExperience -= PlayerExperienceToNextLevel;
            PlayerLevel++;
            _audioManager.PlaySound("LevelUp");
        }
        Debug.Log("Experience: " + PlayerExperience + " / " + PlayerExperienceToNextLevel + " - Level: " + PlayerLevel);
    }

    IEnumerator SpawnExperienceOrb(Vector3 position)
    {
        int count = Random.Range(2, 5);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            ExperienceOrb orb = PoolingManager.Instance.SpawnExperienceOrb(pos, Quaternion.identity).GetComponent<ExperienceOrb>();
            orb.ExperienceValue = Random.Range(2, 4);
            orb.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public List<Vector3> GetTowers()
    {
        List<Vector3> towers = new List<Vector3>();
        foreach (Tile tile in _prefabsGenerator.Tiles)
        {
            if (tile.name == TileType.Obstacle) towers.Add(tile.position);
        }

        return towers;
    }


    /// <summary>
    /// All this stuff is just for debug purpose to see the camera bounds in the scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        float _offsetOnScreen = 0.05f;
        Camera _camera = Camera.main;

        Vector3 a = _camera.ViewportToWorldPoint(new Vector3(_offsetOnScreen, _offsetOnScreen, _camera.transform.position.y));
        Vector3 b = _camera.ViewportToWorldPoint(new Vector3(1f - _offsetOnScreen, _offsetOnScreen, _camera.transform.position.y));
        Vector3 c = _camera.ViewportToWorldPoint(new Vector3(1f - _offsetOnScreen, 1f - 2f * _offsetOnScreen, _camera.transform.position.y));
        Vector3 d = _camera.ViewportToWorldPoint(new Vector3(_offsetOnScreen, 1f - 2f * _offsetOnScreen, _camera.transform.position.y));

        Vector3 aDirection = (a - _camera.transform.position).normalized;
        Vector3 aOnFloor = _camera.transform.position + aDirection * ((2f - _camera.transform.position.y) / aDirection.y);
        Vector3 bDirection = (b - _camera.transform.position).normalized;
        Vector3 bOnFloor = _camera.transform.position + bDirection * ((2f - _camera.transform.position.y) / bDirection.y);
        Vector3 cDirection = (c - _camera.transform.position).normalized;
        Vector3 cOnFloor = _camera.transform.position + cDirection * ((2f - _camera.transform.position.y) / cDirection.y);
        Vector3 dDirection = (d - _camera.transform.position).normalized;
        Vector3 dOnFloor = _camera.transform.position + dDirection * ((2f - _camera.transform.position.y) / dDirection.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(aOnFloor, bOnFloor);
        Gizmos.DrawLine(bOnFloor, cOnFloor);
        Gizmos.DrawLine(cOnFloor, dOnFloor);
        Gizmos.DrawLine(dOnFloor, aOnFloor);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_camera.transform.position, aDirection * 50f);
        Gizmos.DrawRay(_camera.transform.position, bDirection * 50f);
        Gizmos.DrawRay(_camera.transform.position, cDirection * 50f);
        Gizmos.DrawRay(_camera.transform.position, dDirection * 50f);
    }
}

public enum PlayerType
{
    None,
    Archer,
    Guerrier,
    Mage
}

[System.Serializable] public class UnityEventIEnemy : UnityEvent<IEnemy> { }
[System.Serializable] public class UnityEventIntIEnemy : UnityEvent<int, IEnemy> { }
[System.Serializable] public class UnityEventInt : UnityEvent<int> { }
[System.Serializable] public class UnityEventExperienceOrb : UnityEvent<ExperienceOrb> { }
