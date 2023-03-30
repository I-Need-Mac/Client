using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeReference] protected T prefab;
    [SerializeField] [Range(1, 1000)] protected int count = 1;

    protected Stack<T> pool;

    protected virtual void Awake()
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
