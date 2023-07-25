using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class MonsterSpawner : SingletonBehaviour<MonsterSpawner>
{
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int stageId = 10101;

    private int spawnAmount;
    private int spawnCount;
    private Dictionary<int, ObjectPool<Monster>> spawner;
    private Queue<RemainMonster> remainMonsters;
    private WaitForSeconds tick = new WaitForSeconds(0.05f);

    //private Dictionary<string, Dictionary<string, object>> spawnTable;
    //private Dictionary<string, object> spawnData;
    //private int spawnId;
    //private int currentSpawnTime;

    public List<Monster> monsters;
    
    private struct RemainMonster
    {
        public int id;
        public string location;

        public RemainMonster(int id, string location)
        {
            this.id = id;
            this.location = location;
        }
    }
    
    protected override void Awake()
    {
        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        spawner = new Dictionary<int, ObjectPool<Monster>>();

        //spawnTable = CSVReader.Read(CSVReader.Read("StageTable", stageId.ToString(), "MonsterSpawnID").ToString());
        //spawnId = 1;
        //spawnData = spawnTable[spawnId.ToString()];
        //currentSpawnTime = Convert.ToInt32(spawnData["SpawnTime"]);
    }

    public Monster SpawnMonster(int monsterId, Vector2 pos)
    {
        
        //if (!spawner.ContainsKey(monsterId))
        //{   
        //    string prefabPath = CSVReader.Read("MonsterTable", monsterId.ToString(), "MonsterPrefabPath").ToString();
        //    spawner.Add(monsterId, new ObjectPool<Monster>(ResourcesManager.Load<Monster>(prefabPath), transform));
        //}
        Monster monster = spawner[monsterId].GetObject();
        monster.monsterId = monsterId;
        monster.gameObject.layer = (int)LayerConstant.MONSTER;
        monster.SpawnSet();
        monster.transform.localScale = Vector3.one * monster.monsterData.sizeMultiple;
        monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
        monster.SetTarget(GameManager.Instance.player.transform, true);
        monster.gameObject.SetActive(true);
        monsters.Add(monster);
        ++spawnCount;
        return monster;
    }

    public void DeSpawnMonster(Monster monster)
    {
        spawner[monster.monsterId].ReleaseObject(monster);
        monsters.Remove(monster);
        --spawnCount;
    }

    private Queue<MonsterSpawnData> spawnQueue;

    private void SpawnerInit()
    {
        monsters = new List<Monster>();
        remainMonsters = new Queue<RemainMonster>();
        spawnQueue = new Queue<MonsterSpawnData>();

        Dictionary<string, Dictionary<string, object>> stageData = CSVReader.Read(CSVReader.Read("StageTable", stageId.ToString(), "MonsterSpawnID").ToString());
        foreach (string spawnId in stageData.Keys)
        {
            try
            {
                int spawnMobId = Convert.ToInt32(stageData[spawnId]["SpawnMobID"]);
                if (!spawner.ContainsKey(spawnMobId))
                {
                    try
                    {
                        string prefabPath = CSVReader.Read("MonsterTable", spawnMobId.ToString(), "MonsterPrefabPath").ToString();
                        spawner.Add(spawnMobId, new ObjectPool<Monster>(ResourcesManager.Load<Monster>(prefabPath), transform));
                    }
                    catch
                    {
                        DebugManager.Instance.PrintDebug("[ERROR] 현재 존재하지 않는 몬스터입니다 MonsterID: " + spawnMobId);
                    }
                }
                spawnQueue.Enqueue(new MonsterSpawnData(Convert.ToInt32(spawnId), Convert.ToInt32(stageData[spawnId]["SpawnTime"]), spawnMobId, Convert.ToInt32(stageData[spawnId]["SpawnMobAmount"]), Convert.ToString(stageData[spawnId]["SpawnMobLocation"])));
            }
            catch
            {
                DebugManager.Instance.PrintDebug("[ERROR] 빈 줄이 삽입되어 있습니다: " + spawnId);
            }
        }
    }

    public IEnumerator Spawn()
    {
        WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
        SpawnerInit();

        MonsterSpawnData spawnData = spawnQueue.Dequeue();
        while (true)
        {
            yield return fixedUpdate;

            if (remainMonsters.Count != 0)
            {
                RemainMonster remainMonster = remainMonsters.Dequeue();
                Monster monster;
                if (Enum.TryParse(remainMonster.location, true, out SpawnMobLocation spawnMobLocation))
                {
                    monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(spawnMobLocation));
                }
                else
                {
                    Vector2 spawnPos = GameManager.Instance.map.transform.Find("SpawnPoint").Find(remainMonster.location).position;
                    monster = SpawnMonster(remainMonster.id, spawnPos);
                }
            }

            if (Timer.Instance.currentTime < spawnData.spawnTime)
            {
                continue;   //아직 스폰타임이 아니면 스킵
            }
            DebugManager.Instance.PrintDebug("[SpawnerTest]: " + spawnQueue.Count);
            if (spawnData.spawnTime != -1)
            {
                Monster monster;
                if (Enum.TryParse(spawnData.spawnLocation, true, out SpawnMobLocation spawnLocation))
                {
                    if (spawnLocation == SpawnMobLocation.ROUND)
                    {
                        Vector2[] spawnPoses = CameraManager.Instance.Round(spawnData.spawnMobAmount);

                        foreach (Vector2 pos in spawnPoses)
                        {
                            if (spawnCount < spawnAmount)
                            {
                                monster = SpawnMonster(spawnData.spawnMobId, pos);
                            }
                            else
                            {
                                remainMonsters.Enqueue(new RemainMonster(spawnData.spawnMobId, "top"));
                            }
                            yield return tick;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < spawnData.spawnMobAmount; i++)
                        {
                            SpawnMobLocation location = spawnLocation;
                            if (spawnLocation == SpawnMobLocation.RANDOMROUND)
                            {
                                location = (SpawnMobLocation)(i % (System.Enum.GetValues(typeof(SpawnMobLocation)).Length - 1));
                            }

                            if (location == SpawnMobLocation.FACE)
                            {
                                if (GameManager.Instance.player.lookDirection.x < 0)
                                {
                                    location = SpawnMobLocation.LEFT;
                                }
                                else
                                {
                                    location = SpawnMobLocation.RIGHT;
                                }
                            }
                            else if (location == SpawnMobLocation.BACK)
                            {
                                if (GameManager.Instance.player.lookDirection.x < 0)
                                {
                                    location = SpawnMobLocation.RIGHT;
                                }
                                else
                                {
                                    location = SpawnMobLocation.LEFT;
                                }
                            }
                            monster = SpawnMonster(spawnData.spawnMobId, CameraManager.Instance.RandomPosInGrid(location));
                            yield return tick;
                        }
                    }
                }
                else
                {
                    Vector2 spawnPos = GameManager.Instance.map.transform.Find("SpawnPoint").Find(spawnData.spawnLocation).position;
                    for (int i = 0; i < spawnData.spawnMobAmount; i++)
                    {
                        if (spawnCount < spawnAmount)
                        {
                            monster = SpawnMonster(spawnData.spawnMobId, spawnPos);
                        }
                        else
                        {
                            remainMonsters.Enqueue(new RemainMonster(spawnData.spawnMobId, spawnData.spawnLocation));
                        }
                        yield return tick;
                    }
                }

            }

            if (spawnQueue.Count == 0)
            {
                if (remainMonsters.Count == 0)
                {
                    DebugManager.Instance.PrintDebug("[SpawnerTest]: End");
                    yield break;  //더이상 스폰할 몬스터가 없을 경우 종료
                }
            }
            else
            {
                spawnData = spawnQueue.Dequeue();
            }
        }
        
    }

    //public IEnumerator Spawn2()
    //{
    //    monsters.Clear();
    //    while (spawnTable.Keys.Count > spawnId)
    //    {
    //        if (Timer.Instance.currentTime >= currentSpawnTime)
    //        {
    //            if (currentSpawnTime != -1)
    //            {
    //                int mobAmount = Convert.ToInt32(spawnData["SpawnMobAmount"]);
    //                int mobId = Convert.ToInt32(spawnData["SpawnMobID"]);
    //                string sponeLocation = spawnData["SpawnMobLocation"].ToString();

    //                if (Enum.TryParse(sponeLocation, true, out SpawnMobLocation spawnLocation))
    //                {
    //                    if (spawnLocation == SpawnMobLocation.ROUND)
    //                    {
    //                        Vector2[] spawnPoses = CameraManager.Instance.Round(mobAmount);
    //                        Monster monster;
    //                        if (spawnCount < spawnAmount)
    //                        {
    //                            if (remainMonsters.Count > 0)
    //                            {
    //                                RemainMonster remainMonster = remainMonsters.Dequeue();
    //                                monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
    //                            }
    //                            else
    //                            {
    //                                foreach (Vector2 pos in spawnPoses)
    //                                {
    //                                    monster = SpawnMonster(mobId, pos);
    //                                    yield return null;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            remainMonsters.Enqueue(new RemainMonster(mobId, SpawnMobLocation.FACE));
    //                        }
    //                    }
    //                    else
    //                    {
    //                        SpawnMobLocation location = spawnLocation;
    //                        for (int i = 0; i < mobAmount; i++)
    //                        {
    //                            if (spawnCount < spawnAmount)
    //                            {
    //                                Monster monster;

    //                                if (remainMonsters.Count > 0)
    //                                {
    //                                    RemainMonster remainMonster = remainMonsters.Dequeue();
    //                                    monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
    //                                }
    //                                else
    //                                {
    //                                    if (spawnLocation == SpawnMobLocation.RANDOMROUND)
    //                                    {
    //                                        location = (SpawnMobLocation)(i % (System.Enum.GetValues(typeof(SpawnMobLocation)).Length - 1));
    //                                        DebugManager.Instance.PrintDebug("SpawnTest: " + location);
    //                                    }

    //                                    if (location == SpawnMobLocation.FACE)
    //                                    {
    //                                        if (GameManager.Instance.player.lookDirection.x < 0)
    //                                        {
    //                                            location = SpawnMobLocation.LEFT;
    //                                        }
    //                                        else
    //                                        {
    //                                            location = SpawnMobLocation.RIGHT;
    //                                        }
    //                                    }
    //                                    else if (location == SpawnMobLocation.BACK)
    //                                    {
    //                                        if (GameManager.Instance.player.lookDirection.x < 0)
    //                                        {
    //                                            location = SpawnMobLocation.RIGHT;
    //                                        }
    //                                        else
    //                                        {
    //                                            location = SpawnMobLocation.LEFT;
    //                                        }
    //                                    }
    //                                    monster = SpawnMonster(mobId, CameraManager.Instance.RandomPosInGrid(location));
    //                                }
    //                            }
    //                            else
    //                            {
    //                                remainMonsters.Enqueue(new RemainMonster(mobId, location));
    //                            }
    //                            yield return tick;
    //                        }
    //                    }

    //                }
    //                else
    //                {
    //                    //특정 위치 소환
    //                    Vector2 sponePos = GameManager.Instance.map.transform.Find("SpawnPoint").Find(sponeLocation).position;
    //                    for (int i = 0; i < mobAmount; i++)
    //                    {
    //                        if (spawnCount < spawnAmount)
    //                        {
    //                            Monster monster;

    //                            if (remainMonsters.Count > 0)
    //                            {
    //                                RemainMonster remainMonster = remainMonsters.Dequeue();
    //                                monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
    //                            }
    //                            else
    //                            {
    //                                monster = SpawnMonster(mobId, sponePos);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            remainMonsters.Enqueue(new RemainMonster(mobId, SpawnMobLocation.FACE));
    //                        }
    //                        yield return tick;
    //                    }
    //                }
    //            }

    //            if (spawnTable.ContainsKey((++spawnId).ToString()))
    //            {
    //                spawnData = spawnTable[spawnId.ToString()];
    //                currentSpawnTime = Convert.ToInt32(spawnData["SpawnTime"]);
    //            }
    //        }
    //        yield return null;
    //    }
    //}

}


