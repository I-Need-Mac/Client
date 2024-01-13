using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeRae : ActiveSkill
{
    public SeRae(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum)
    {
    }

    public override IEnumerator Activation()
    {
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Vector2 pos = Scanner.GetTarget(SKILLCONSTANT.SKILL_TARGET.RANDOM, shooter, skillData.attackDistance);
            pos.x += UnityEngine.Random.Range(-2.0f, 2.0f);
            pos.y += UnityEngine.Random.Range(-2.0f, 2.0f);

            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
            projectile.transform.position = pos;
            yield return intervalTime;
            SkillManager.Instance.DeSpawnProjectile(projectile);
        }
    }
}
