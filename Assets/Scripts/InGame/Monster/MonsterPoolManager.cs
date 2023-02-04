using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFM;

public class MonsterPoolManager : SingletonBehaviour<MonsterPoolManager>
{
    [SerializeField] private MonsterPool monster_1;

    private Dictionary<string, MonsterPool> pools;

    protected override void Awake()
    {
        pools = new Dictionary<string, MonsterPool>
        {
            {"temp1", monster_1},
        };
    }

    public Monster SpawnMonster(Transform transform, string name)
    {
        Monster monster = pools[name].GetObject();
        monster.gameObject.layer = (int) LayerConstant.MONSTER;
        monster.transform.localPosition = new Vector3(monster.transform.localPosition.x, monster.transform.localPosition.y, (int)LayerConstant.MONSTER);
        monster.gameObject.SetActive(true);
        return monster;
    }

    public void DespawnMonster(Monster monster)
    {
        pools[monster.monsterData.monsterName].ReleaseObject(monster);
    }
}
