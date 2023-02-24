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

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterPoolManager monsterPoolManager;
    [SerializeField] private float spawnTime = 1f;

    private Dictionary<string, IEnumerator> monsters;

    private WaitForSeconds time;

    private void Awake()
    {
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
            //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), (int)LayerConstant.MONSTER));
            Monster monster = monsterPoolManager.SpawnMonster(CameraManager.Instance.RandomPosInGrid(spawnPos.ToString()), mobName);
            yield return time;
        }
    }
}
