using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juhon : ActiveSkill
{
    public Juhon(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override void Init()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
        projectile.transform.localScale = Vector2.zero;
        projectiles.Add(projectile);
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        while (true)
        {
            projectiles[0].transform.localScale = Vector2.one * skillData.projectileSizeMulti;
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                projectiles[0].CollisionPower(true);
                yield return intervalTime;
                projectiles[0].CollisionPower(false);
                
            }
            projectiles[0].transform.localScale = Vector2.zero;
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        //SkillManager.Instance.DeSpawnProjectile(_projectile);
        //projectiles.Clear();
    }
    
}
