using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStatue : FieldStructure
{
    protected override void Awake()
    {
        base.Awake();

        this.hp = float.Parse(this.fieldStructureData.gimmickParam[0]);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Projectile projectile))
        {
            this.hp -= projectile.skillData.damage;
            if (this.hp <= 0)
            {
                ItemManager.Instance.DropItems(this.fieldStructureData.gimmickParam[1], transform);
                gameObject.SetActive(false);
            }
        }
    }
}
