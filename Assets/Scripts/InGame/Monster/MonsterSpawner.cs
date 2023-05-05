using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : SingletonBehaviour<MonsterSpawner>
{
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int stageId = 10101;

    private int spawnAmount;
    private int spawnCount;
    private Dictionary<int, ObjectPool<Monster>> spawner;
    private Queue<RemainMonster> remainMonsters;

    private Dictionary<string, Dictionary<string, object>> spawnTable;
    private Dictionary<string, object> spawnData;
    private int spawnId;
    private int currentSpawnTime;

    private struct RemainMonster
    {
        public int id;
        public SponeMobLocation location;

        public RemainMonster(int id, SponeMobLocation location)
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

    private void Start()
    {
        StartCoroutine(Spawn());
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

    private IEnumerator Spawn()
    {
        while (true)
        {
            if (spawnTable.Keys.Count - 3 <= spawnId)
            {
                yield break;
            }

            if (Timer.Instance.currentTime >= currentSpawnTime)
            {
                int mobAmount = Convert.ToInt32(spawnData["SponeMobAmount"]);
                int mobId = Convert.ToInt32(spawnData["SponeMobID"]);
                string sponeLocation = spawnData["SponeMobLocation"].ToString();
                Player player = GameManager.Instance.player;
                
                if (Enum.TryParse(sponeLocation, true, out SponeMobLocation location))
                {
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
                                if (location == SponeMobLocation.ROUND)
                                {
                                    location = (SponeMobLocation)UnityEngine.Random.Range(0, (int)location);
                                }
                                else if (location == SponeMobLocation.FACE)
                                {
                                    if (player.lookDirection.x < 0)
                                    {
                                        location = SponeMobLocation.LEFT;
                                    }
                                    else
                                    {
                                        location = SponeMobLocation.RIGHT;
                                    }
                                }
                                else if (location == SponeMobLocation.BACK)
                                {
                                    if (player.lookDirection.x < 0)
                                    {
                                        location = SponeMobLocation.RIGHT;
                                    }
                                    else
                                    {
                                        location = SponeMobLocation.LEFT;
                                    }
                                }
                                monster = SpawnMonster(mobId, CameraManager.Instance.RandomPosInGrid(location));
                            }
                        }
                        else
                        {
                            remainMonsters.Enqueue(new RemainMonster(mobId, location));
                        }
                    }
                    if (spawnTable.ContainsKey((++spawnId).ToString()))
                    {
                        spawnData = spawnTable[spawnId.ToString()];
                        currentSpawnTime = Convert.ToInt32(spawnData["SponeTime"]);
                    }
                }
                else
                {
                    //특정 위치 소환
                    Vector3 sponePos = GameManager.Instance.map.transform.Find("FieldStructure").transform.Find(sponeLocation).transform.position;
                    Monster monster = SpawnMonster(mobId, sponePos);
                }
            }
            yield return null;
        }
    }
    //private void Spawn(Dictionary<string, object> data)
    //{
    //    int mobAmount = Convert.ToInt32(data["SponeMobAmount"]);
    //    int mobId = Convert.ToInt32(data["SponeMobID"]);
    //    string sponeLocation = data["SponeMobLocation"].ToString();
    //    Player player = GameManager.Instance.player;

    //    if (Enum.TryParse(sponeLocation, true, out SponeMobLocation location))
    //    {
    //        for (int i = 0; i < mobAmount; i++)
    //        {
    //            if (spawnCount < spawnAmount)
    //            {
    //                Monster monster;

    //                if (remainMonsters.Count > 0)
    //                {
    //                    RemainMonster remainMonster = remainMonsters.Dequeue();
    //                    monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(remainMonster.location));
    //                }
    //                else
    //                {
    //                    if (location == SponeMobLocation.ROUND)
    //                    {
    //                        location = (SponeMobLocation)UnityEngine.Random.Range(0, (int)location);
    //                    }
    //                    else if (location == SponeMobLocation.FACE)
    //                    {
    //                        if (player.lookDirection.x < 0)
    //                        {
    //                            location = SponeMobLocation.LEFT;
    //                        }
    //                        else
    //                        {
    //                            location = SponeMobLocation.RIGHT;
    //                        }
    //                    }
    //                    else if (location == SponeMobLocation.BACK)
    //                    {
    //                        if (player.lookDirection.x < 0)
    //                        {
    //                            location = SponeMobLocation.RIGHT;
    //                        }
    //                        else
    //                        {
    //                            location = SponeMobLocation.LEFT;
    //                        }
    //                    }
    //                    monster = SpawnMonster(mobId, CameraManager.Instance.RandomPosInGrid(location));
    //                }
    //            }
    //            else
    //            {
    //                remainMonsters.Enqueue(new RemainMonster(mobId, location));
    //            }
    //        }
    //        if (sponeTable.ContainsKey((++spawnId).ToString()))
    //        {
    //            sponeData = sponeTable[spawnId.ToString()];
    //            currentSpawnTime = Convert.ToInt32(sponeData["SponeTime"]);
    //        }
    //    }
    //    else 
    //    {
    //        //특정 위치 소환
    //        Vector3 sponePos = GameManager.Instance.map.transform.Find("FieldStructure").transform.Find(sponeLocation).transform.position;
    //        Monster monster = SpawnMonster(mobId, sponePos);
    //    }
    //}

}


