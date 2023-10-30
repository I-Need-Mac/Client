using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAttack : FieldStructure
{
    [SerializeField] private int[] monsterList;
    [SerializeField] private int[] mobCountList;

    protected override void Awake()
    {
        base.Awake();

        ((CircleCollider2D)top).radius = float.Parse(this.fieldStructureData.gimmickParam[0]);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Player player))
        {
            top.enabled = false;
            int randomNumber = UnityEngine.Random.Range(0, monsterList.Length);
            for (int i = 0; i < mobCountList[randomNumber]; i++)
            {
                MonsterSpawner.Instance.SpawnMonster(monsterList[randomNumber], transform.position);
            }
        }
    }
}