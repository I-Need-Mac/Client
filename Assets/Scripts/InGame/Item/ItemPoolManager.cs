using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoolManager : SingletonBehaviour<ItemPoolManager>
{
    [SerializeField] private ExpItemPool expPool;

    protected override void Awake()
    {
        
    }

    public ExpItem SpawnExpItem(Vector3 pos)
    {
        ExpItem expItem = expPool.GetObject();
        expItem.transform.position = pos;
        expItem.gameObject.transform.SetParent(transform.Find("ExpItemPool").transform);
        expItem.gameObject.SetActive(true);
        return expItem;
    }

    public void DeSpawnExpItem(ExpItem expItem)
    {
        expPool.ReleaseObject(expItem);
    }
}
