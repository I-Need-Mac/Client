using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAttack : FieldStructure
{
    [SerializeField] private int[] monsterList;

    protected override void Awake()
    {
        base.Awake();

        ((CircleCollider2D)front).radius = float.Parse(this.fieldStructureData.gimmickParam[0]);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Player player))
        {
            front.enabled = false;
            MonsterSpawner.Instance.SpawnMonster(monsterList[UnityEngine.Random.Range(0, monsterList.Length)], transform.position);
        }
    }
}
