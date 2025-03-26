using System.Collections.Generic;
using UnityEngine;

public class CustomPool : MonoBehaviour
{
    [SerializeField] private GameObject ufoPrefab;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int initialSize = 10;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        poolDictionary[ufoPrefab] = new Queue<GameObject>();
        poolDictionary[asteroidPrefab] = new Queue<GameObject>();

        PreloadObjects(ufoPrefab);
        PreloadObjects(asteroidPrefab);
    }

    private void PreloadObjects(GameObject prefab)
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewObject(prefab);
            obj.SetActive(false);
            poolDictionary[prefab].Enqueue(obj);
        }
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool doesn't contain prefab");
            return null;
        }

        if (poolDictionary[prefab].Count > 0)
        {
            GameObject obj = poolDictionary[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return CreateNewObject(prefab);
        }
    }

    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool doesn't contain prefab");
            return;
        }

        obj.SetActive(false);
        poolDictionary[prefab].Enqueue(obj);
    }
}