using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aliento : Skill
{
    public Aliento(int skillId, Transform shooter) : base(skillId, shooter) { }

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
                Vector2 pos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
                pos -= (Vector2)shooter.position;
                projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90.0f);
                yield return intervalTime;
            }

            yield return coolTime;
        }
    }
}
