using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();

    [SerializeField] GameObject[] objectsToPool;
    [SerializeField] int[] quantityToPool;

    public static ObjectPool objectPoolInstance;

    private void Awake()
    {
        if (objectPoolInstance == null)
        {
            objectPoolInstance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Mathf.Min(objectsToPool.Length, quantityToPool.Length); i++)
        {
            for (int j = 0; j < quantityToPool[i]; j++)
            {
                GameObject obj = GameObject.Instantiate(objectsToPool[i]);
                obj.name = objectsToPool[i].name;
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    public void RecreateObjects()
    {
        pool.Clear();
        Start();
    }

    public GameObject GetPooledObject(GameObject typeObject)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].name == typeObject.name && !pool[i].activeInHierarchy)
                return pool[i];
        }
        return null;
    }
}
