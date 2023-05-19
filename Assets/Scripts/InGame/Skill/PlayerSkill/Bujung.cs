using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bujung : Skill
{
    public Bujung(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        while (true)
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                ProjectileAI projectile = (ProjectileAI)SkillManager.Instance.SpawnProjectile(skillData);
                projectile.transform.localPosition = shooter.position;

                Transform target = SkillManager.Instance.MeleeTarget(shooter, skillData.attackDistance);
                Vector2 vec;
                if (target == null)
                {
                    vec = (GameManager.Instance.player.lookDirection - (Vector2)shooter.position).normalized;
                }
                else
                {
                    vec = ((Vector2)target.position - (Vector2)shooter.position).normalized;
                }

                projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90.0f);
                yield return intervalTime;
            }

            yield return coolTime;
        }
    }
}