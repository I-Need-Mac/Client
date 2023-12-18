using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : ActiveSkill
{
    public Possession(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        yield return intervalTime;
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
}
