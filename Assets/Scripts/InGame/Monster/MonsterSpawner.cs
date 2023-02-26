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
    [SerializeField] private MonsterPoolManager monsterPoolManager;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int stageId = 10101;

    private int spawnAmount;
    private int spawnCount;
    private Dictionary<string, IEnumerator> monsters;
    private WaitForSeconds time;

    protected override void Awake()
    {
        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        monsters = new Dictionary<string, IEnumerator>();
        time = new WaitForSeconds(spawnTime);
    }

    private void Start()
    {
        StartSpawn();
    }

    private void StartSpawn()
    {
        monsters.Add("Nien", SpawnMonster("Nien", GRID.A));
        monsters.Add("Nien_M", SpawnMonster("Nien_M", GRID.C));
        monsters.Add("Nien_L", SpawnMonster("Nien_L", GRID.H));

        foreach (IEnumerator mob in monsters.Values)
        {
            StartCoroutine(mob);
        }
    }

    private IEnumerator SpawnMonster(string mobName, GRID spawnPos)
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (spawnCount < spawnAmount)
            {
                Monster monster = monsterPoolManager.SpawnMonster(CameraManager.Instance.RandomPosInGrid(spawnPos.ToString()), mobName);
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
