using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrefabsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject LightPrefab;
    [SerializeField] private GameObject HolePrefab;
    [SerializeField] private GameObject ObstaclePrefab;
    [SerializeField] private GameObject LandPrefab;
    [SerializeField] private GameObject LightPole;
    [SerializeField] private GameObject Start;
    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject Angle;
    [SerializeField] private GameObject EnemySpawn;

    private GameObject _LightsContainer;
    private GameObject _HolesContainer;
    private GameObject _ObstaclesContainer;
    private GameObject _LandContainer;
    private GameObject _Arena;
    private GameObject _EnemySpawnContainer;

    private int _tileSize = 4;

    // List of Tile to store the position of the prefabs
    public List<Tile> Tiles = new List<Tile>();

    [SerializeField] private int _holes;
    [SerializeField] private int _obstacles;
    [SerializeField] private int _lands;

    private int _numberOfEnemySpawnZone = 8;
    public int NumberOfEnemySpawnZone { get => _numberOfEnemySpawnZone; set => _numberOfEnemySpawnZone = value; }

    public void AddTile(Vector3 postion, TileType name)
    {
        Tiles.Add(new Tile(postion * _tileSize, name));
    }

    public void GeneratePrefabs(int mapWidth, int mapHeight)
    {
        InstantiateContainers();

        ModifyCenter(mapWidth, mapHeight);

        GenerateLand(mapWidth, mapHeight);
        GenerateLight();
        GenerateHoles();
        GenerateObstacles();
        GenerateSurround(mapWidth, mapHeight);

        GenerateEnemySpawn(mapWidth, mapHeight);
    }

    public void ClearPrefabs()
    {
        Tiles.Clear();
        // Clear the prefabs
        DestroyImmediate(_LightsContainer);
        DestroyImmediate(GameObject.Find("LightsContainer"));

        DestroyImmediate(_HolesContainer);
        DestroyImmediate(GameObject.Find("HolesContainer"));

        DestroyImmediate(_ObstaclesContainer);
        DestroyImmediate(GameObject.Find("ObstaclesContainer"));

        DestroyImmediate(_LandContainer);
        DestroyImmediate(GameObject.Find("LandContainer"));

        DestroyImmediate(_Arena);
        DestroyImmediate(GameObject.Find("Arena"));

        DestroyImmediate(_EnemySpawnContainer);
        DestroyImmediate(GameObject.Find("EnemySpawnContainer"));
    }

    private void InstantiateContainers()
    {
        _EnemySpawnContainer = new GameObject("EnemySpawnContainer");

        _Arena = new GameObject("Arena", typeof(NavMeshSurface));

        _LightsContainer = new GameObject("LightsContainer");
        _HolesContainer = new GameObject("HolesContainer");
        _ObstaclesContainer = new GameObject("ObstaclesContainer");
        _LandContainer = new GameObject("LandContainer");

        // Set _Arena as parent of the containers
        _LightsContainer.transform.parent = _Arena.transform;
        _HolesContainer.transform.parent = _Arena.transform;
        _ObstaclesContainer.transform.parent = _Arena.transform;
        _LandContainer.transform.parent = _Arena.transform;
    }

    private void ModifyCenter(int mapWidth, int mapHeight)
    {
        _holes = 0;
        _obstacles = 0;
        _lands = 0;

        // Clear the center of the map
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (
                Tiles[i].position.x >= ((mapWidth / 2f) - 2f) * _tileSize &&
                Tiles[i].position.x <= ((mapWidth / 2f) + 1f) * _tileSize &&
                Tiles[i].position.z >= ((mapHeight / 2f) - 2f) * _tileSize &&
                Tiles[i].position.z <= ((mapHeight / 2f) + 1f) * _tileSize
            )
            {
                //Remove the Tiles[i] from the list 
                Tiles.RemoveAt(i);
                i--;
            }

            // Count the number of each tile
            switch (Tiles[i].name)
            {
                case TileType.Hole:
                    _holes++;
                    break;
                case TileType.Obstacle:
                    _obstacles++;
                    break;
                default:
                    _lands++;
                    break;
            }
        }
    }

    private void GenerateLight()
    {
        // Instantiate light prefabs
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].name == TileType.Light)
            {
                Vector3 position = Tiles[i].position + new Vector3(2f, 2.5f, 2f);
                position += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                GameObject light = Instantiate(LightPrefab, position, Quaternion.identity, _LightsContainer.transform);
                light.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }

    private void GenerateHoles()
    {
        // Instantiate hole prefabs
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].name == TileType.Hole)
            {
                Vector3 position = Tiles[i].position + new Vector3(2f, 0f, 2f);
                Instantiate(HolePrefab, position, Quaternion.identity, _HolesContainer.transform);
            }
        }
    }

    private void GenerateObstacles()
    {
        // Instantiate obstacles prefab
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].name == TileType.Obstacle)
            {
                Vector3 position = Tiles[i].position + new Vector3(2f, 2f, 2f);
                float rotation = Random.Range(0, 4) * 90f;
                GameObject obstacle = Instantiate(ObstaclePrefab, position, Quaternion.Euler(0f, rotation, 0f), _ObstaclesContainer.transform);
            }
        }
    }

    private void GenerateLand(int mapWidth, int mapHeight)
    {
        // Instantiate land prefabs
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].name != TileType.Hole)
            {
                Vector3 position = Tiles[i].position + new Vector3(2f, -1f, 2f);
                Instantiate(LandPrefab, position, Quaternion.identity, _LandContainer.transform);
            }
        }
        Instantiate(Start, new Vector3((mapWidth / 2f) * _tileSize, 0f, (mapHeight / 2f) * _tileSize), Quaternion.Euler(0f, 90f, 0f), _LandContainer.transform);
    }

    private void GenerateSurround(int mapWidth, int mapHeight)
    {
        for (int i = -1; i < mapWidth + 1; i++)
        {
            for (int j = -1; j < mapHeight + 1; j++)
            {
                if (i == -1 || i == mapWidth || j == -1 || j == mapHeight)
                {
                    Vector3 postionLand = new Vector3(i * _tileSize + 2f, -1f, j * _tileSize + 2f);
                    Instantiate(LandPrefab, postionLand, Quaternion.identity, _LandContainer.transform);

                    if (j == i || (j == mapHeight && i == -1) || (j == -1 && i == mapWidth))
                    {
                        Instantiate(Angle, new Vector3(i * _tileSize + 2f, 2f, j * _tileSize + 2f), Quaternion.identity, _ObstaclesContainer.transform);
                    }

                    Vector3 positionSurround = new Vector3(i * _tileSize, 2f, j * _tileSize);
                    if (j % 3 == 1)
                    {
                        if (i == -1)
                        {
                            Instantiate(LightPole, positionSurround + Vector3.right * 1.25f, Quaternion.Euler(0, 0, 0), _ObstaclesContainer.transform);
                        }
                        else if (i == mapWidth)
                        {
                            Instantiate(LightPole, positionSurround - Vector3.left * 2.75f, Quaternion.Euler(0, 180, 0), _ObstaclesContainer.transform);
                        }
                    }
                    else if (i % 3 == 1)
                    {
                        if (j == -1)
                        {
                            Instantiate(LightPole, positionSurround + Vector3.forward * 1.25f, Quaternion.Euler(0, 270, 0), _ObstaclesContainer.transform);
                        }
                        else if (j == mapHeight)
                        {
                            Instantiate(LightPole, positionSurround - Vector3.back * 2.75f, Quaternion.Euler(0, 90, 0), _ObstaclesContainer.transform);
                        }
                    }
                }
            }
        }

        Vector3 positionSurround1 = new Vector3((mapWidth / 2f) * _tileSize, 4f, -3.5f);
        Vector3 positionSurround2 = new Vector3((mapWidth / 2f) * _tileSize, 4f, (mapHeight) * _tileSize + 3.5f);
        Vector3 positionSurround3 = new Vector3(-3.5f, 4f, (mapHeight / 2f) * _tileSize);
        Vector3 positionSurround4 = new Vector3((mapWidth) * _tileSize + 3.5f, 4f, (mapHeight / 2f) * _tileSize);

        GameObject wall1 = Instantiate(Wall, positionSurround1, Quaternion.identity, _ObstaclesContainer.transform);
        GameObject wall2 = Instantiate(Wall, positionSurround2, Quaternion.identity, _ObstaclesContainer.transform);
        GameObject wall3 = Instantiate(Wall, positionSurround3, Quaternion.identity, _ObstaclesContainer.transform);
        GameObject wall4 = Instantiate(Wall, positionSurround4, Quaternion.identity, _ObstaclesContainer.transform);

        wall1.transform.localScale = new Vector3((mapWidth * _tileSize), 4f, 1f);
        wall2.transform.localScale = new Vector3((mapWidth * _tileSize), 4f, 1f);
        wall3.transform.localScale = new Vector3(1f, 4f, (mapHeight * _tileSize));
        wall4.transform.localScale = new Vector3(1f, 4f, (mapHeight * _tileSize));
    }

    public void GenerateNavMesh()
    {
        NavMeshSurface navMesh = _Arena.GetComponent<NavMeshSurface>();

        navMesh.overrideTileSize = true;
        navMesh.tileSize = 128;

        navMesh.overrideVoxelSize = true;
        navMesh.voxelSize = 0.05f;

        navMesh.BuildNavMesh();
    }

    private void GenerateEnemySpawn(int mapWidth, int mapHeight)
    {
        List<Vector3> positionsList = new List<Vector3>();

        for (int i = 0; i < _numberOfEnemySpawnZone; i++)
        {
            while (true)
            {
                int randomTile = Random.Range(0, Tiles.Count);

                bool notToCloseFromCenter = Mathf.Abs(Tiles[randomTile].position.x - mapWidth * _tileSize / 2f) > 2f * _tileSize && Mathf.Abs(Tiles[randomTile].position.z - mapHeight * _tileSize / 2f) > 2f * _tileSize;
                if (Tiles[randomTile].name == TileType.Land && notToCloseFromCenter && !positionsList.Contains(Tiles[randomTile].position))
                {
                    Vector3 position = Tiles[randomTile].position + new Vector3(2f, 2f, 2f);
                    float rotation = Random.Range(0, 4) * 90f;
                    Instantiate(EnemySpawn, position, Quaternion.Euler(0f, rotation, 0f), _EnemySpawnContainer.transform);
                    positionsList.Add(position);
                    break;
                }
            }
        }
    }
}

public struct Tile
{
    public Vector3 position;
    public TileType name;

    public Tile(Vector3 position, TileType name)
    {
        this.position = position;
        this.name = name;
    }
}

[System.Serializable]
public enum TileType
{
    Light,
    Hole,
    Obstacle,
    Land
}
