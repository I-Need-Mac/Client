using BFM;
using System;
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

    private Dictionary<string, Dictionary<string, object>> monsterTable;
    private Dictionary<string, Dictionary<string, object>> sponeTable;
    private Dictionary<string, object> sponeData;
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
        monsterTable = CSVReader.Read("MonsterTable");

        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        spawner = new Dictionary<int, ObjectPool<Monster>>();
        remainMonsters = new Queue<RemainMonster>();

        sponeTable = CSVReader.Read(CSVReader.Read("StageTable", stageId.ToString(), "MonsterSponeID").ToString());
        spawnId = 1;
        sponeData = sponeTable[spawnId.ToString()];
        currentSpawnTime = int.Parse(sponeData["SponeTime"].ToString());
    }

    //private void Start()
    //{
    //    SpawnerInit();
    //}

    private void Update()
    {
        if (Timer.Instance.currentTime >= currentSpawnTime)
        {
            Spawn(sponeData);
        }
    }

    //private void SpawnerInit()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        spawner.Add(int.Parse(child.name), child.GetComponent<MonsterPool>());
    //    }
    //}

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

    private void Spawn(Dictionary<string, object> data)
    {
        int mobAmount = int.Parse(data["SponeMobAmount"].ToString());
        int mobId = int.Parse(data["SponeMobID"].ToString());
        string sponeLocation = data["SponeMobLocation"].ToString();
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
                    ++spawnCount;
                }
                else
                {
                    remainMonsters.Enqueue(new RemainMonster(mobId, location));
                }
            }
            if (sponeTable.ContainsKey((++spawnId).ToString()))
            {
                sponeData = sponeTable[spawnId.ToString()];
                currentSpawnTime = int.Parse(sponeData["SponeTime"].ToString());
            }
        }
        else 
        {
            //특정 위치 소환
            Vector3 sponePos = GameManager.Instance.map.transform.Find("FieldStructure").transform.Find(sponeLocation).transform.position;
            Monster monster = SpawnMonster(mobId, sponePos);
        }


    }

}


