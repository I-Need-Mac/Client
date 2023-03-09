using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GRID
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
}

public class MonsterSpawner : SingletonBehaviour<MonsterSpawner>
{
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int stageId = 10101;

    private MonsterPoolManager monsterPoolManager;
    private int spawnAmount;
    private int spawnCount;
    private Dictionary<int, MonsterPool> monsters;
    private WaitForSeconds time;

    protected override void Awake()
    {
        monsterPoolManager = GetComponentInChildren<MonsterPoolManager>();
        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        monsters = new Dictionary<int, MonsterPool>();
        time = new WaitForSeconds(spawnTime);
    }

    private void Start()
    {
        //StartSpawn();
    }

    //private void StartSpawn()
    //{
    //    monsters.Add(101, SpawnMonsters(101, GRID.A));

    //    foreach (IEnumerator mob in monsters.Values)
    //    {
    //        StartCoroutine(mob);
    //    }
    //}

    //private IEnumerator SpawnMonsters(int monsterId, GRID spawnPos)
    //{
    //    yield return new WaitForSeconds(1f);

    //    while (true)
    //    {
    //        if (spawnCount < spawnAmount)
    //        {
    //            Monster monster = SpawnMonster(monsterId, CameraManager.Instance.RandomPosInGrid(spawnPos.ToString()));
    //            ++spawnCount;
    //        }
    //        yield return time;
    //    }
    //}

    //public Monster SpawnMonster(int monsterId, Vector2 pos)
    //{
    //    Monster monster = pool.GetObject();
    //    monster.monsterId = monsterId;
    //    monster.MonsterSetting(monsterId.ToString());
    //    monster.gameObject.layer = (int)LayerConstant.MONSTER;
    //    monster.transform.localScale = Vector2.one * monster.monsterData.sizeMultiple;
    //    monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
    //    monster.gameObject.SetActive(true);
    //    return monster;
    //}

    //public void DespawnMonster(Monster monster)
    //{
    //    pool.ReleaseObject(monster);
    //    --spawnCount;
    //}

    ////스폰테이블로더
    //public void MonsterSponeDataLoad(int monsterSponeId)
    //{
    //    Dictionary<string, Dictionary<string, object>> table = CSVReader.Read("MonsterSponeTable");
    //    if (table.ContainsKey(monsterSponeId.ToString()))
    //    {
    //        Dictionary<string, object> data = table[monsterSponeId.ToString()];

    //    }
    //}
}
