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
            //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), (int)LayerConstant.MONSTER));
            Monster monster = monsterPoolManager.SpawnMonster(CameraManager.Instance.RandomPosInGrid("E"), "MobName_101");
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
