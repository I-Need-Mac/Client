using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterPoolManager monsterPoolManager;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private GRID spawnPos = GRID.A;

    private void Awake()
    {
    }

    private void Start()
    {
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), (int)LayerConstant.MONSTER));
            Monster monster = monsterPoolManager.SpawnMonster(CameraManager.Instance.RandomPosInGrid(spawnPos.ToString()), "MobName_101");
            yield return new WaitForSeconds(spawnTime);
        }
    }
}

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