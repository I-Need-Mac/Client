using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juhon : ActiveSkill
{
    public Juhon(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                projectile.transform.localScale = Vector2.one * skillData.projectileSizeMulti;
                projectile.CollisionPower(true);
                yield return intervalTime;
                projectile.CollisionPower(false);

            }
            projectile.transform.localScale = Vector2.zero;
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);

    }
    
}
