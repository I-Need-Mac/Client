using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlame : FieldStructure
{
    private float damage;
    private float dotTime;
    private float dotDamage;
    private float slow;
    private float currentDotTime;

    protected override void Awake()
    {
        base.Awake();

        damage = float.Parse(this.fieldStructureData.gimmickParam[0]);
        dotTime = float.Parse(this.fieldStructureData.gimmickParam[1]);
        dotDamage = float.Parse(this.fieldStructureData.gimmickParam[2]);
        slow = float.Parse(this.fieldStructureData.gimmickParam[3]);
        currentDotTime = dotTime;
    }

    private void Update()
    {
        if (currentDotTime > 0.0f)
        {
            currentDotTime -= Time.deltaTime;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentDotTime <= 0.0f)
        {
            if (collision.transform.parent.TryGetComponent(out Player player))
            {
                player.playerManager.playerData.CurrentHpModifier(-(int)damage);
            }
            if (collision.TryGetComponent(out Monster monster))
            {
                monster.Hit(-(int)damage);
            }
            currentDotTime = dotTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Player player))
        {
            FireDot(player);
        }
        if (collision.TryGetComponent(out Monster monster))
        {
            FireDot(monster);
        }
    }

    private IEnumerator FireDot(Player player)
    {
        WaitForSeconds tick = new WaitForSeconds(1.0f);
        for (int i = 0; i < dotTime; i++)
        {
            player.playerManager.playerData.CurrentHpModifier(-(int)dotDamage);
            yield return tick;
        }
    }

    private IEnumerator FireDot(Monster monster)
    {
        WaitForSeconds tick = new WaitForSeconds(1.0f);
        for (int i = 0; i < dotTime; i++)
        {
            monster.Hit(-(int)dotDamage);
            yield return tick;
        }
    }
}
