using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class JuHyung : Skill
{
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
                closestMonster = SkillManager.Instance.MeleeTarget(closestMonster, skillData.attackDistance);

                if (closestMonster != null && !prevMonsters.Contains(closestMonster))
                {
                    int num = closestMonster.GetComponent<Monster>().uniqueId;
                    projectile.transform.localPosition = closestMonster.position;
                    prevMonsters.Add(closestMonster);
                    DebugManager.Instance.PrintDebug("[TEST]: FIRE " + num);
                    yield return intervalTime;
                }
                --count;
            }
            
            SkillManager.Instance.DeSpawnProjectile(projectile);
            prevMonsters.Clear();
            yield return coolTime;
        }
    }

}
