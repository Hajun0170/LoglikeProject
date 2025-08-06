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
    private Dictionary<string, GameObject> prefabLookup; // prefab 저장용

    void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabLookup = new Dictionary<string, GameObject>();

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
            prefabLookup.Add(pool.key, pool.prefab);
        }
    }

    public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning("❌ Pool does not contain key: " + key);
            return null;
        }

        Queue<GameObject> queue = poolDictionary[key];

        // ⚠️ 자동 확장 로직
        if (queue.Count == 0)
        {
            Debug.Log($"🔁 풀 자동 확장: {key}");

            GameObject prefab = prefabLookup[key];
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            queue.Enqueue(newObj);
        }

        GameObject objectToSpawn = queue.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectToSpawn.GetComponent<Poolable>()?.OnReuse();

        queue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
