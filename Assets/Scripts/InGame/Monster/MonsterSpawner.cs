using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterSpawner : SingletonBehaviour<MonsterSpawner>
{
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int stageId = 10101;

    private MonsterPoolManager monsterPoolManager;
    private int spawnAmount;
    private int spawnCount;
    private Dictionary<int, MonsterPool> spawner;
    private Queue<int> remainMonsters;

    private Dictionary<string, Dictionary<string, object>> sponeData;
    private int spawnIndex;
    private int currentSpawnTime;
    
    protected override void Awake()
    {
        monsterPoolManager = GetComponentInChildren<MonsterPoolManager>();
        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        spawner = new Dictionary<int, MonsterPool>();
        remainMonsters = new Queue<int>();
        sponeData = CSVReader.Read(CSVReader.Read("StageTable", stageId.ToString(), "MonsterSponeID").ToString());
        spawnIndex = 1;
    }

    private void Start()
    {
        SpawnerInit();
    }

    private void Update()
    {
        if (sponeData.ContainsKey(spawnIndex.ToString()))
        {
            Dictionary<string, object> data = sponeData[spawnIndex.ToString()];
            currentSpawnTime = int.Parse(data["SponeTime"].ToString());
            if (Timer.Instance.currentTime >= currentSpawnTime)
            {
                Spawn(data);
            }
        }
        
    }

    private void SpawnerInit()
    {
        foreach (Transform child in transform)
        {
            spawner.Add(int.Parse(child.name), child.GetComponent<MonsterPool>());
        }
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

    public Monster SpawnMonster(int monsterId, Vector2 pos)
    {
        Monster monster = spawner[monsterId].GetObject();
        monster.gameObject.layer = (int)LayerConstant.MONSTER;
        monster.transform.localScale = Vector2.one * monster.monsterData.sizeMultiple;
        monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
        monster.gameObject.SetActive(true);
        ++spawnCount;
        return monster;
    }

    public void DeSpawnMonster(Monster monster)
    {
        spawner[monster.monsterId].ReleaseObject(monster);
        --spawnCount;
    }

    private void Spawn(Dictionary<string, object> data)
    {
        currentSpawnTime = int.Parse(data["SponeTime"].ToString());
        
        if (spawnCount < spawnAmount)
        {
            for (int i = 0; i < int.Parse(data["SponeMobAmount"].ToString()); i++)
            {
                SponeMobLocation location = (SponeMobLocation)Enum.Parse(typeof(SponeMobLocation), data["SponeMobLocation"].ToString().ToUpper());
                Monster monster = SpawnMonster(int.Parse(data["SponeMobID"].ToString()), CameraManager.Instance.RandomPosInGrid(location));
            }
            ++spawnIndex;
        }
    }

}


