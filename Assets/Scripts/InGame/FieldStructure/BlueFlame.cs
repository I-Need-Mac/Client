using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlame : FieldStructure
{
    [SerializeField] private float damage;
    [SerializeField] private float burnTime;
    [SerializeField] private float burnDamage;
    [SerializeField] private float burnSlow;
    [SerializeField] private float duration;

    private float currentBurnTime;

    protected override void Awake()
    {
        base.Awake();

        currentBurnTime = burnTime;
    }

    private void Update()
    {
        if (currentBurnTime > 0.0f)
        {
            currentBurnTime -= Time.deltaTime;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (currentBurnTime <= 0.0f)
        {
            if (collision.TryGetComponent(out Monster monster))
            {
                monster.Hit(-(int)damage);
            }
            Player player = collision.GetComponentInParent<Player>();
            if (player != null)
            {
                StartCoroutine(player.Invincible());
                player.playerManager.playerData.CurrentHpModifier(-(int)damage);
                player.Slow(0.5f, burnSlow);
            }
            currentBurnTime = burnTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            StartCoroutine(monster.FireDot(burnTime, damage));
            return;
        }
        Player player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            StartCoroutine(player.FireDot(burnTime, damage));
        }
    }
}
