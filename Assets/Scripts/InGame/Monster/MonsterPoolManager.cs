using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFM;

public class MonsterPoolManager : SingletonBehaviour<MonsterPoolManager>
{
    [SerializeField] private MonsterPool nien;
    [SerializeField] private MonsterPool nienM;
    [SerializeField] private MonsterPool nienL;

    private Dictionary<string, MonsterPool> pools;

    protected override void Awake()
    {
        pools = new Dictionary<string, MonsterPool>
        {
            {"Nien", nien},
            {"Nien_M", nienM},
            {"Nien_L", nienL},
        };
    }

    public Monster SpawnMonster(Vector2 pos, string name)
    {
        Monster monster = pools[name].GetObject();
        monster.gameObject.layer = (int) LayerConstant.MONSTER;
        monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
        monster.gameObject.SetActive(true);
        return monster;
    }

    public void DespawnMonster(Monster monster)
    {
        pools[monster.monsterData.monsterName].ReleaseObject(monster);
    }
}
