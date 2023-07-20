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
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
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

            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }
    }
}
