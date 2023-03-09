using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFM;

public class MonsterPoolManager : SingletonBehaviour<MonsterPoolManager>
{
    //private Dictionary<int, MonsterPool> pools;
    private MonsterPool pool;

    protected override void Awake()
    {
        //pools = new Dictionary<string, MonsterPool>
        //{
        //    {"Nien", nien},
        //    {"Nien_M", nienM},
        //    {"Nien_L", nienL},
        //};
        //pools = new Dictionary<int, MonsterPool>();
        pool = GetComponentInChildren<MonsterPool>();
    }

    public Monster SpawnMonster(int monsterId, Vector2 pos)
    {
        //if (!pools.ContainsKey(monsterId))
        //{
        //    pools.Add(monsterId, new MonsterPool());
        //}
        //Monster monster = pools[monsterId].GetObject();
        //monster.gameObject.layer = (int) LayerConstant.MONSTER;
        //monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
        //monster.gameObject.SetActive(true);
        //return monster;
        Monster monster = pool.GetObject();
        monster.monsterId = monsterId;
        monster.MonsterSetting(monsterId.ToString());
        monster.gameObject.layer = (int)LayerConstant.MONSTER;
        monster.transform.localScale = Vector2.one * monster.monsterData.sizeMultiple;
        monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
        monster.gameObject.SetActive(true);
        return monster;
    }

    public void DespawnMonster(Monster monster)
    {
        //pools[monster.monsterId].ReleaseObject(monster);
        pool.ReleaseObject(monster);
    }
}
