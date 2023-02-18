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

    public ExpItem SpawnExpItem(int type, float exp, Vector3 pos)
    {
        ExpItem expItem = expPool.GetObject();
        expItem.ReceiveInfo(type, exp);
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
