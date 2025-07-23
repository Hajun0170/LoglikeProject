using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.key, objectPool);
        }
    }

    public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning("Pool does not contain key: " + key);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[key].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectToSpawn.GetComponent<Poolable>()?.OnReuse();

        poolDictionary[key].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
