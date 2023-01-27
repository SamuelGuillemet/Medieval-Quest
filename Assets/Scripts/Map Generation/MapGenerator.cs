using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    private enum DrawMode { NoiseMap, ColourMap, PrefabMap };

    [SerializeField] private int _mapWidth;
    [SerializeField] private int _mapHeight;
    [Space, Header("Noise Settings"), SerializeField] private DrawMode _drawMode = DrawMode.PrefabMap;
    [SerializeField] private float _noiseScale;
    [SerializeField] private int _octaves;
    [SerializeField, Range(0, 1)] private float _persistance;
    [SerializeField] private float _lacunarity;
    [SerializeField] private int _seed;
    [SerializeField] private Vector2 _offset;

    public bool autoUpdate;

    [Space, Header("Terrain Settings"), SerializeField] private TerrainType[] _regions;

    private PrefabsGenerator _prefabsGenerator;

    public int Seed { get => _seed; set => _seed = value; }
    public int MapWidth { set => _mapWidth = value; }
    public int MapHeight { set => _mapHeight = value; }

    public void GenerateMap()
    {
        _prefabsGenerator = GetComponent<PrefabsGenerator>();
        _prefabsGenerator.ClearPrefabs();

        float[,] noiseMap = Noise.GenerateNoiseMap(_mapWidth, _mapHeight, _seed, _noiseScale, _octaves, _persistance, _lacunarity, _offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.TextureRender.gameObject.SetActive(true);

        if (_drawMode == DrawMode.NoiseMap)
        {
            DrawNoiseMap(noiseMap, display);
        }
        else if (_drawMode == DrawMode.ColourMap)
        {
            DrawColourMap(noiseMap, display);
        }
        else
        {
            display.TextureRender.gameObject.SetActive(false);
            DrawPrefabMap(noiseMap);

            _prefabsGenerator.GeneratePrefabs(_mapWidth, _mapHeight);
            _prefabsGenerator.GenerateNavMesh();
        }
    }

    private void DrawNoiseMap(float[,] noiseMap, MapDisplay display)
    {
        display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
    }

    private void DrawColourMap(float[,] noiseMap, MapDisplay display)
    {
        int holes = 0;
        int obstacles = 0;
        int land = 0;

        Color[] colourMap = new Color[_mapWidth * _mapHeight];
        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight <= _regions[i].height)
                    {
                        colourMap[y * _mapWidth + x] = _regions[i].colour;
                        switch (_regions[i].name)
                        {
                            case TileType.Hole:
                                holes += 1;
                                break;
                            case TileType.Obstacle:
                                obstacles += 1;
                                break;
                            default:
                                land += 1;
                                break;
                        }
                        break;
                    }
                }
            }
        }

        // Debug.Log("Holes: " + holes);
        // Debug.Log("Obstacles: " + obstacles);
        // Debug.Log("Land: " + land);

        Debug.Log("Ratio: " + (1 - ((holes + obstacles) / (float)land)));

        display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, _mapWidth, _mapHeight));
    }

    private void DrawPrefabMap(float[,] noiseMap)
    {
        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight <= _regions[i].height)
                    {
                        _prefabsGenerator.AddTile(new Vector3(x, 0f, y), _regions[i].name);
                        break;
                    }
                }
            }
        }
    }

    private void OnValidate()
    {
        if (_mapWidth < 5)
        {
            _mapWidth = 5;
        }
        if (_mapHeight < 5)
        {
            _mapHeight = 5;
        }
        if (_lacunarity < 1)
        {
            _lacunarity = 1;
        }
        if (_octaves < 0)
        {
            _octaves = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (_prefabsGenerator)
        {
            if (_prefabsGenerator.Tiles != null)
            {
                foreach (Tile tile in _prefabsGenerator.Tiles)
                {
                    switch (tile.name)
                    {
                        case TileType.Hole:
                            Gizmos.color = Color.black;
                            break;
                        case TileType.Obstacle:
                            Gizmos.color = Color.red;
                            break;
                        case TileType.Light:
                            Gizmos.color = Color.yellow;
                            break;
                        default:
                            Gizmos.color = Color.green;
                            break;
                    }
                    Gizmos.DrawCube(new Vector3(tile.position.x + 2f, 0.25f, tile.position.z + 2f), Vector3.one / 2);
                }
            }
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public TileType name;
    public float height;
    public Color colour;
}