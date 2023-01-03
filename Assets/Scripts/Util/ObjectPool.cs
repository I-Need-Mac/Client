using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeReference] private T prefab;
    [SerializeField] [Range(1, 1000)] private int count = 1;

    private Stack<T> pool;

    private void Awake()
    {
        pool = new Stack<T>();
        AddObject();
    }

    public T GetObject()
    {
        if (pool.Count == 0)
        {
            AddObject();
        }

        T obj = pool.Pop();
        obj.gameObject.transform.SetParent(transform);
        return obj;
    }

    public void ReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.gameObject.transform.SetParent(transform);
        pool.Push(obj);
    }

    public void AddObject()
    {
        for (int i = 0; i < count; i++)
        {
            T obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
    }
}
