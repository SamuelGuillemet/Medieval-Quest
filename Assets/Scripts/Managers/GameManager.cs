using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    public PlayerType SelectedPlayer = PlayerType.None;
    private IPlayer _player;
    public IPlayer Player { get => _player; set => _player = value; }

    private static GameManager _instance;
    public static GameManager Instance { get { if (_instance == null) _instance = GameObject.FindObjectOfType<GameManager>(); return _instance; } }
    private MapGenerator _mapGenerator;

    private PrefabsGenerator _prefabsGenerator;

    private int _seed = 1;
    private int _width = 20;
    private int _height = 20;

    private int _numberOfEnemies = 0;
    private EnemySpawnZone[] _enemySpawnZones;
    [SerializeField] private int _numberOfEnemySpawnZone = 8;

    private int[] _fibonnaciSuite = new int[] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };
    [HideInInspector] public int WaveNumber = 0;

    public List<IEnemy> Enemies;

    [HideInInspector] public UnityEventIEnemy OnEnemyKilled;
    [HideInInspector] public UnityEventIntIEnemyFloat OnEnemyDamageTaken;
    [HideInInspector] public UnityEventInt OnPlayerDamageTaken;
    [HideInInspector] public UnityEventInt OnPlayerHealed;
    [HideInInspector] public UnityEventExperienceOrb OnOrbCollected;
    [HideInInspector] public UnityEventInt OnPlayerUpgrade;
    [HideInInspector] public UnityEvent OnPlayerDeath;

    private int _playerExperience = 0;
    public int PlayerExperience { get => _playerExperience; set => _playerExperience = value; }
    private int _playerLevel = 1;
    public int PlayerLevel { get => _playerLevel; set => _playerLevel = value; }
    public int PlayerExperienceToNextLevel { get => 10 * _playerLevel; }

    private AudioManager _audioManager;

    private GameUI _gameUI;
    [SerializeField] private GameObject _damageOutputPrefab;

    private SaveDataBetweenScenes _saveBetwenScene;

    [Space(10)]
    [Header("Prefabs for player")]
    [SerializeField] private GameObject _archerPrefab;
    [SerializeField] private GameObject _demonPrefab;
    [SerializeField] private GameObject _magePrefab;

    void Awake()
    {
        _seed = Random.Range(0, 10000);
        _width = Random.Range(12, 24);
        _height = Random.Range(12, 24);

        _width += _width % 2 == 0 ? 0 : 1;
        _height += _height % 2 == 0 ? 0 : 1;


        _saveBetwenScene = SaveDataBetweenScenes.Instance;

        if (_saveBetwenScene.SelectedPlayer == PlayerType.None)
        {
            if (SelectedPlayer == PlayerType.None) SelectedPlayer = PlayerType.Archer;
        }
        else
        {
            SelectedPlayer = _saveBetwenScene.SelectedPlayer;
        }

        GameObject player = null;

        Vector3 playerPosition = new Vector3(_width * 2, 4, _height * 2);
        switch (SelectedPlayer)
        {
            case PlayerType.Archer:
                player = Instantiate(_archerPrefab, playerPosition, Quaternion.identity);
                break;
            case PlayerType.Demon:
                player = Instantiate(_demonPrefab, playerPosition, Quaternion.identity);
                break;
            case PlayerType.Mage:
                player = Instantiate(_magePrefab, playerPosition, Quaternion.identity);
                break;
        }

        _player = player.GetComponentInChildren<IPlayer>();

        Debug.Log("Player type: " + player + " " + SelectedPlayer);

    }

    void OnEnable()
    {
        Enemies = new List<IEnemy>();

        _mapGenerator = FindObjectOfType<MapGenerator>();
        _prefabsGenerator = FindObjectOfType<PrefabsGenerator>();

        _gameUI = FindObjectOfType<GameUI>();

        _mapGenerator.Seed = _seed;
        _mapGenerator.MapWidth = _width;
        _mapGenerator.MapHeight = _height;
        _prefabsGenerator.NumberOfEnemySpawnZone = _numberOfEnemySpawnZone;

        _mapGenerator.GenerateMap();

        _enemySpawnZones = FindObjectsOfType<EnemySpawnZone>();

        OnEnemyKilled.AddListener(OnEnemyKilledCallback);
        OnEnemyDamageTaken.AddListener(OnEnemyDamagedCallback);
        OnPlayerDamageTaken.AddListener(OnPlayerDamagedCallback);
        OnPlayerHealed.AddListener(OnPlayerHealedCallback);
        OnOrbCollected.AddListener(OnOrbCollectedCallback);
        OnPlayerUpgrade.AddListener(OnPlayerUpgradeCallback);
        OnPlayerDeath.AddListener(OnPlayerDeathCallback);

        _audioManager = AudioManager.Instance;
    }

    void Start()
    {
        StartCoroutine(EnemiesWave());
        _gameUI.UpdateExperienceBar(_playerExperience, PlayerExperienceToNextLevel);
        _gameUI.UpdateWaveText(WaveNumber, _fibonnaciSuite[WaveNumber]);
    }

    IEnumerator EnemiesWave()
    {
        yield return new WaitForSeconds(2f);
        while (WaveNumber < _fibonnaciSuite.Length)
        {
            _gameUI.UpdateWaveText(WaveNumber + 1, _fibonnaciSuite[WaveNumber]);
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
        _gameUI.Victory();
    }

    private void OnEnemyKilledCallback(IEnemy enemy)
    {
        _numberOfEnemies--;
        PoolingManager.Instance.ReturnToPool(enemy.gameObject);
        _gameUI.UpdateEnnemiesBar(_numberOfEnemies);
        Enemies.Remove(enemy);

        StartCoroutine(SpawnExperienceOrb(enemy.transform.position));
    }

    private void OnEnemyDamagedCallback(int damage, IEnemy enemy, float repuslionForce)
    {
        enemy.TakeDamage(damage, repuslionForce);
        Debug.Log("Enemy: " + enemy.name + " - Health: " + enemy.Health);

        DamageOutput.Create(_damageOutputPrefab, Player.transform.position, damage);
    }

    private void OnPlayerDamagedCallback(int damage)
    {
        _player.TakeDamage(damage);
        Debug.Log("Player: " + Player.name + " - Health: " + _player.Health);
        _gameUI.UpdateHealthBar(_player.Health);

    }

    private void OnPlayerHealedCallback(int heal)
    {
        _player.Heal(heal);
        Debug.Log("Player: " + Player.name + " - Health: " + _player.Health);
        _gameUI.UpdateHealthBar(_player.Health);
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
            _gameUI.CreateUpgradeImage();
        }
        _gameUI.UpdateExperienceBar(PlayerExperience, PlayerExperienceToNextLevel);
        Debug.Log("Experience: " + PlayerExperience + " / " + PlayerExperienceToNextLevel + " - Level: " + PlayerLevel);
    }

    private void OnPlayerUpgradeCallback(int key)
    {
        _player.Upgrade(key);
    }

    private void OnPlayerDeathCallback()
    {
        _gameUI.Defeat();
    }

    IEnumerator SpawnExperienceOrb(Vector3 position)
    {
        int count = Random.Range(2, 5);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            ExperienceOrb orb = PoolingManager.Instance.SpawnExperienceOrb(pos, Quaternion.identity).GetComponent<ExperienceOrb>();
            orb.ExperienceValue = Random.Range(3, 6);
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
}

public enum PlayerType
{
    None,
    Archer,
    Demon,
    Mage
}

[System.Serializable] public class UnityEventIEnemy : UnityEvent<IEnemy> { }
[System.Serializable] public class UnityEventIntIEnemyFloat : UnityEvent<int, IEnemy, float> { }
[System.Serializable] public class UnityEventInt : UnityEvent<int> { }
[System.Serializable] public class UnityEventExperienceOrb : UnityEvent<ExperienceOrb> { }
