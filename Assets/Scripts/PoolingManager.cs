using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarriorAnimsFREE;


public class PoolingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs = new List<GameObject>();
    private List<GameObject> _enemyPrefabs = new List<GameObject>();
    private GameObject _experienceOrbPrefab;
    private static PoolingManager _instance;
    public static PoolingManager Instance { get { if (_instance == null) _instance = GameObject.FindObjectOfType<PoolingManager>(); return _instance; } }
    public Dictionary<string, List<GameObject>> _pools = new Dictionary<string, List<GameObject>>();

    private Transform _poolParent;

    private void Awake()
    {
        foreach (GameObject prefab in _prefabs)
        {
            if (prefab.GetComponent<IEnemy>() != null)
            {
                _enemyPrefabs.Add(prefab);
            }
        }

        _experienceOrbPrefab = _prefabs.Find(x => x.GetComponent<ExperienceOrb>() != null);
    }

    private void Start()
    {
        _poolParent = new GameObject("Pool").transform;

        foreach (GameObject prefab in _prefabs)
        {
            _pools.Add(prefab.name, new List<GameObject>());
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = UnityUtils.InstantiateDisabled(prefab, _poolParent);
                _pools[prefab.name].Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(GameObject _obj, Transform parent = null)
    {
        if (parent == null) parent = _poolParent;
        string key = _obj.name;

        if (_pools.ContainsKey(key))
        {
            for (int i = 0; i < _pools[key].Count; i++)
            {
                if (!_pools[key][i].gameObject.activeInHierarchy)
                {
                    return _pools[key][i];
                }
            }
        }
        // If there's no pool of objects of this type, we will create it.
        else
        {
            _pools.Add(key, new List<GameObject>());
        }

        // We instantiate an object of the desired type and add it to the pool.
        GameObject obj = Instantiate(_obj);
        obj.SetActive(false);
        _pools[key].Add(obj);
        // If we have specified a parent, we set it as the object's parent.
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public GameObject SpawnObjectFromPool(GameObject _obj, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obj = GetPooledObject(_obj, parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public GameObject SpawnRandomEnemyFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetPooledObject(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)], _poolParent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public GameObject SpawnExperienceOrb(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetPooledObject(_experienceOrbPrefab, _poolParent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
        return obj;
    }

}
