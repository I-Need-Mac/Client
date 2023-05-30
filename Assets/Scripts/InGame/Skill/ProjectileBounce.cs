using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBounce : Projectile
{
    private Rigidbody2D rigid;
    private int bounced;
    private int bounceCount;
    private int boomProbability;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (TryGetComponent(out rigid))
        {
            rigid = GetComponentInChildren<Rigidbody2D>();
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        DebugManager.Instance.PrintDebug("bouncetest: " + collision.gameObject.layer);
        if ((collision.gameObject.layer == (int)LayerConstant.MONSTER)
            || (collision.gameObject.layer == (int)LayerConstant.GIMMICK))
        {
            float radius = ((CircleCollider2D)projectileCollider).radius;
            ++bounced;
            DebugManager.Instance.PrintDebug("bouncetest: " + boomProbability);
            if (UnityEngine.Random.Range(0, 101) <= boomProbability)
            {
                ((CircleCollider2D)projectileCollider).radius = skillData.splashRange;
                Invoke("Remove", skillData.duration);
                ((CircleCollider2D)projectileCollider).radius = radius;
            }

            if (bounced >= bounceCount)
            {
                Remove();
            }
        }
    }

    public void Fire(Vector2 direction)
    {
        bounced = 0;
        bounceCount = Convert.ToInt32(skillData.skillEffectParam[0]);
        boomProbability = Convert.ToInt32(skillData.skillEffectParam[1]);
        rigid.velocity = direction.normalized * skillData.speed;
    }
}
