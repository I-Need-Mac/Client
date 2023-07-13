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

    private Dictionary<string, Dictionary<string, object>> spawnTable;
    private Dictionary<string, object> spawnData;
    private int spawnId;
    private int currentSpawnTime;

    public List<Monster> monsters = new List<Monster>();

    private struct RemainMonster
    {
        public int id;
        public SpawnMobLocation location;

        public RemainMonster(int id, SpawnMobLocation location)
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
        remainMonsters = new Queue<RemainMonster>();

        spawnTable = CSVReader.Read(CSVReader.Read("StageTable", stageId.ToString(), "MonsterSponeID").ToString());
        spawnId = 1;
        spawnData = spawnTable[spawnId.ToString()];
        currentSpawnTime = Convert.ToInt32(spawnData["SponeTime"]);
    }

    public Monster SpawnMonster(int monsterId, Vector2 pos)
    {
        if (!spawner.ContainsKey(monsterId))
        {
            string prefabPath = CSVReader.Read("MonsterTable", monsterId.ToString(), "MonsterPrefabPath").ToString();
            spawner.Add(monsterId, new ObjectPool<Monster>(ResourcesManager.Load<Monster>(prefabPath), transform));
        }
        Monster monster = spawner[monsterId].GetObject();
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

    public IEnumerator Spawn()
    {
        monsters.Clear();
        while (spawnTable.Keys.Count > spawnId)
        {
            if (Timer.Instance.currentTime >= currentSpawnTime)
            {
                if (currentSpawnTime != -1)
                {
                    int mobAmount = Convert.ToInt32(spawnData["SponeMobAmount"]);
                    int mobId = Convert.ToInt32(spawnData["SponeMobID"]);
                    string sponeLocation = spawnData["SponeMobLocation"].ToString();

                    if (Enum.TryParse(sponeLocation, true, out SpawnMobLocation spawnLocation))
                    {
                        if (spawnLocation == SpawnMobLocation.ROUND)
                        {
                            Vector2[] spawnPoses = CameraManager.Instance.Round(mobAmount);
                            Monster monster;
                            if (spawnCount < spawnAmount)
                            {
                                if (remainMonsters.Count > 0)
                                {
                                    RemainMonster remainMonster = remainMonsters.Dequeue();
                                    monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
                                }
                                else
                                {
                                    foreach (Vector2 pos in spawnPoses)
                                    {
                                        monster = SpawnMonster(mobId, pos);
                                        yield return null;
                                    }
                                }
                            }
                            else
                            {
                                remainMonsters.Enqueue(new RemainMonster(mobId, SpawnMobLocation.FACE));
                            }
                        }
                        else
                        {
                            SpawnMobLocation location = spawnLocation;
                            for (int i = 0; i < mobAmount; i++)
                            {
                                if (spawnCount < spawnAmount)
                                {
                                    Monster monster;

                                    if (remainMonsters.Count > 0)
                                    {
                                        RemainMonster remainMonster = remainMonsters.Dequeue();
                                        monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
                                    }
                                    else
                                    {
                                        if (spawnLocation == SpawnMobLocation.RANDOMROUND)
                                        {
                                            location = (SpawnMobLocation)(i % (System.Enum.GetValues(typeof(SpawnMobLocation)).Length - 1));
                                            DebugManager.Instance.PrintDebug("SpawnTest: " + location);
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
                                        monster = SpawnMonster(mobId, CameraManager.Instance.RandomPosInGrid(location));
                                    }
                                }
                                else
                                {
                                    remainMonsters.Enqueue(new RemainMonster(mobId, location));
                                }
                                yield return tick;
                            }
                        }

                    }
                    else
                    {
                        //특정 위치 소환
                        Vector2 sponePos = GameManager.Instance.map.transform.Find("SpawnPoint").Find(sponeLocation).position;
                        for (int i = 0; i < mobAmount; i++)
                        {
                            if (spawnCount < spawnAmount)
                            {
                                Monster monster;

                                if (remainMonsters.Count > 0)
                                {
                                    RemainMonster remainMonster = remainMonsters.Dequeue();
                                    monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
                                }
                                else
                                {
                                    monster = SpawnMonster(mobId, sponePos);
                                }
                            }
                            else
                            {
                                remainMonsters.Enqueue(new RemainMonster(mobId, SpawnMobLocation.FACE));
                            }
                            yield return tick;
                        }
                    }
                }

                if (spawnTable.ContainsKey((++spawnId).ToString()))
                {
                    spawnData = spawnTable[spawnId.ToString()];
                    currentSpawnTime = Convert.ToInt32(spawnData["SponeTime"]);
                }
            }
            yield return null;
        }
    }

}


