using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyuGyu : ActiveSkill
{
    public GyuGyu(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime / 1000.0f);
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

            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime / 1000.0f);
        }
    }
}
