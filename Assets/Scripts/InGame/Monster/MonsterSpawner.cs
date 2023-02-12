using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterPoolManager monsterPoolManager;
    [SerializeField] private float spawnTime = 1f;

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
            Monster monster = monsterPoolManager.SpawnMonster(transform, "temp1");
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
