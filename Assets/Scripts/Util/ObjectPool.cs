using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private int addCount = 50;

    private Queue<GameObject> objQueue = new Queue<GameObject>();

    public GameObject GetObject()
    {
        if (objQueue.Count <= 0)
        {
            AddObject();
        }

        GameObject obj = objQueue.Dequeue();
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        objQueue.Enqueue(obj);
    }

    public void AddObject()
    {
        GameObject obj;

        for (int i = 0; i < addCount; i++)
        {
            obj = Instantiate(objPrefab, this.transform);
            obj.SetActive(false);

            objQueue.Enqueue(obj);
        }
    }
}
