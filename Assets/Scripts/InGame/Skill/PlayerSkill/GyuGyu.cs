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
                ProjectileBounce projectile = (ProjectileBounce)SkillManager.Instance.SpawnProjectile(skillData);
                projectile.transform.localPosition = shooter.position;
                Vector2 direction = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
                projectile.Fire(direction);
                yield return intervalTime;
            }

            yield return coolTime;
        }
    }
}
