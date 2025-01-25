using Game.Pools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public int _startSize = 5;
    [SerializeField] public List<PoolableObject> _prefabs = new List<PoolableObject>();

    [SerializeField] private Dictionary<string, Queue<GameObject>> _pools = new();
    [SerializeField] int _currentCount;

    public static PoolingSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _pools = new Dictionary<string, Queue<GameObject>>();

        foreach (var prefab in _prefabs)
        {
            _pools.Add(prefab.name, new Queue<GameObject>());
            for (int i = 0; i < _startSize; i++)
            {
                GameObject next = CreateNew(prefab.name);

                _pools[prefab.name].Enqueue(next);
            }
        }
    }

    private GameObject CreateNew(string prefabName)
    {
        var prefab = _prefabs.First(p => p.name == prefabName);

        // use this object as parent so that objects dont crowd hierarchy
        GameObject next = Instantiate(prefab, transform).gameObject;
        next.name = $"{prefab.name}_pooled_{_currentCount}";
        next.SetActive(false);
        next.GetComponent<PoolableObject>().SetCacheKey(prefabName);
        _currentCount++;
        return next;
    }

    /// <summary>
    /// Used to take Object from Pool.
    /// <para>Should be used on server to get the next Object</para>
    /// <para>Used on client by ClientScene to spawn objects</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject Get(PoolableObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(prefab.name))
        {
            _prefabs.Add(prefab);
            _pools.Add(prefab.name, new Queue<GameObject>());
        }

        return Get(prefab.name, position, rotation);
    }
    public GameObject Get(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject next = _pools[prefabName].Count > 0
            ? _pools[prefabName].Dequeue() // take from pool
            : CreateNew(prefabName); // create new because pool is empty

        // CreateNew might return null if max size is reached
        if (next == null) { return null; }

        // set position/rotation and set active
        next.transform.position = position;
        next.transform.rotation = rotation;
        next.SetActive(true);
        return next;
    }

    public T Get<T>(PoolableObject prefab) where T : MonoBehaviour
    {
        var instance = Get(prefab, Vector3.zero, Quaternion.identity);
        return instance.GetComponent<T>();
    }

    public T Get<T>(string prefabName) where T : MonoBehaviour
    {
        var instance = Get(prefabName, Vector3.zero, Quaternion.identity);
        return instance.GetComponent<T>();
    }

    /// <summary>
    /// Used to put object back into pool so they can b
    /// <para>Should be used on server after unspawning an object</para>
    /// <para>Used on client by ClientScene to unspawn objects</para>
    /// </summary>
    /// <param name="spawned"></param>
    public void PutBackInPool(string prefabName, GameObject spawned)
    {
        // disable object
        spawned.SetActive(false);
        spawned.transform.SetParent(transform);

        // add back to pool
        _pools[prefabName].Enqueue(spawned);
    }
}
