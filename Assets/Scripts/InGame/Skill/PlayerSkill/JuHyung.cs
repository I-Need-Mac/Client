using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JuHyung : ActiveSkill
{
    private float diff = 0.25f;

    public JuHyung(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        List<Transform> prevMonsters = new List<Transform>();

        while (true)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData);
            projectile.transform.localPosition = shooter.position;

            Transform closestMonster = shooter;
            int count = skillData.projectileCount;

            while (count != 0)
            {
                closestMonster = Scanner.GetTargetTransform(skillData.skillTarget, closestMonster, skillData.attackDistance);

                if (closestMonster != null && !prevMonsters.Contains(closestMonster))
                {
                    //projectile.transform.localPosition = closestMonster.position;
                    yield return Move(projectile, closestMonster);
                    prevMonsters.Add(closestMonster);
                    yield return intervalTime;
                }
                --count;
            }

            SkillManager.Instance.DeSpawnProjectile(projectile);
            prevMonsters.Clear();
            yield return coolTime;
        }
    }

    private IEnumerator Move(Projectile projectile, Transform target)
    {
        while (Vector2.Distance(projectile.transform.position, target.position) > diff)
        {
            projectile.transform.Translate(((Vector2)target.position - (Vector2)projectile.transform.position).normalized * skillData.speed * Time.deltaTime);
            yield return null;
        }
        DebugManager.Instance.PrintDebug(">>>>");
    }

}
