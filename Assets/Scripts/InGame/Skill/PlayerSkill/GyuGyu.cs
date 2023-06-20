using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyuGyu : Skill
{
    public GyuGyu(int skillId, Transform shooter) : base(skillId, shooter) { }

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
                ProjectileStraight projectile = (ProjectileStraight)SkillManager.Instance.SpawnProjectile(skillData);
                projectile.transform.localPosition = shooter.position;

                Vector2 targetPos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
                Vector2 vec = (targetPos - (Vector2)shooter.position).normalized;
                projectile.SetFireDirection(vec);
                yield return intervalTime;
            }

            yield return coolTime;
        }
    }
}
