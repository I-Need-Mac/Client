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
    private Dictionary<int, IEnumerator> monsters;
    private WaitForSeconds time;

    protected override void Awake()
    {
        monsterPoolManager = GetComponentInChildren<MonsterPoolManager>();
        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        monsters = new Dictionary<int, IEnumerator>();
        time = new WaitForSeconds(spawnTime);
    }

    private void Start()
    {
        StartSpawn();
    }

    private void StartSpawn()
    {
        monsters.Add(101, SpawnMonsters(101, GRID.A));

        foreach (IEnumerator mob in monsters.Values)
        {
            StartCoroutine(mob);
        }
    }

    private IEnumerator SpawnMonsters(int monsterId, GRID spawnPos)
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (spawnCount < spawnAmount)
            {
                Monster monster = monsterPoolManager.SpawnMonster(monsterId, CameraManager.Instance.RandomPosInGrid(spawnPos.ToString()));
                ++spawnCount;
            }
            yield return time;
        }
    }

    public void DeSpawnMonster(Monster monster)
    {
        MonsterPoolManager.Instance.DespawnMonster(monster);
        --spawnCount;
    }
}
