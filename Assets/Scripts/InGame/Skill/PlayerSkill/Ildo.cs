using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Ildo : ActiveSkill
{
    public Ildo(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        projectile.transform.localPosition = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
        projectile.CollisionPower(false);     
        int padongCount = 0;
        do
        {
            yield return intervalTime;
            SkillManager.Instance.CoroutineStarter(Padong(projectile));
            padongCount++;
        } while (padongCount < skillData.projectileCount);
        yield return frame;
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }

    private IEnumerator Padong(Projectile projectile)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(projectile.transform.position, skillData.splashRange);
        foreach(Collider2D target in hits)
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.SkillEffectActivation(skillData.skillEffect[1], float.Parse(skillData.skillEffectParam[1]));
                monster.Hit(skillData.damage);
            }
        }
        yield return frame;
    }
}
