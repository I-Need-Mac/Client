using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crepitus : ActiveSkill
{
    private WaitForSeconds tick;
    private float size;

    public Crepitus(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    //public override void Init()
    //{
    //    tick = new WaitForSeconds(0.5f);
    //    size = skillData.attackDistance >= 10 ? 10 : skillData.attackDistance;

    //    for (int i = 0; i < skillData.projectileCount; i++)
    //    {
    //        Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData);
    //        projectile.SetAlpha(0.0f);
    //        projectile.CollisionPower(false);
    //        projectiles.Add(projectile);
    //    }
    //}

    public override IEnumerator Activation()
    {
        tick = new WaitForSeconds(0.5f);
        size = skillData.attackDistance >= 10 ? 10 : skillData.attackDistance;

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData);
                projectile.SetAlpha(0.0f);
                projectile.CollisionPower(false);
                SkillManager.Instance.CoroutineStarter(Boom(projectile));
                yield return intervalTime;
            }

            //foreach (Projectile projectile in projectiles)
            //{
            //    SkillManager.Instance.CoroutineStarter(Boom(projectile));
            //    yield return intervalTime;
            //}

            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);
    }

    private IEnumerator Boom(Projectile projectile)
    {
        projectile.transform.localPosition = Scanner.GetTarget(SKILLCONSTANT.SKILL_TARGET.RANDOM, shooter, size);
        projectile.SetAlpha(1.0f);
        yield return tick;
        projectile.SetAlpha(0.0f);
    }
}
