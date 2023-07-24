using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JuHyung : ActiveSkill
{
    private float diff = 0.25f;

    public JuHyung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        List<Transform> prevMonsters = new List<Transform>();

        do
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData);
            projectile.transform.localPosition = shooter.position;

            Transform closestMonster = shooter;
            int count = skillData.projectileCount;

            closestMonster = Scanner.GetTargetTransform(skillData.skillTarget, closestMonster, skillData.attackDistance);

            while (count != 0)
            {
                if (closestMonster != null && !prevMonsters.Contains(closestMonster))
                {
                    //projectile.transform.localPosition = closestMonster.position;
                    yield return Move(projectile, closestMonster);
                    prevMonsters.Add(closestMonster);
                    yield return intervalTime;
                }
                --count;
                closestMonster = Scanner.GetTargetTransform(SKILLCONSTANT.SKILL_TARGET.MELEE, closestMonster, skillData.attackDistance);
            }

            SkillManager.Instance.DeSpawnProjectile(projectile);
            prevMonsters.Clear();
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);
    }

    private IEnumerator Move(Projectile projectile, Transform target)
    {
        while (Vector2.Distance(projectile.transform.position, target.position) > diff)
        {
            projectile.transform.Translate(((Vector2)target.position - (Vector2)projectile.transform.position).normalized * skillData.speed * Time.deltaTime);
            yield return null;
        }
    }

}
