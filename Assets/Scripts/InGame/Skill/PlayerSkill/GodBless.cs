using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBless : Skill
{
    public GodBless(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
        projectile.transform.localScale = Vector2.zero;
        projectile.CollisionRadius(skillData.splashRange);
        projectile.CollisionPower(false);
        projectiles.Add(projectile);
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        while (true)
        {
            if (skillData.splashRange < 10)
            {
                for (int i = 0; i < skillData.projectileCount; i++)
                {
                    projectiles[0].transform.localScale = Vector2.one;
                    projectiles[0].CollisionPower(true);
                    yield return duration;
                    projectiles[0].CollisionPower(false);
                    projectiles[0].transform.localScale = Vector2.zero;
                }
            }
            else    //일정범위 초과시 맵 전체 타격
            {
                foreach (Monster monster in MonsterSpawner.Instance.monsters)
                {
                    //데미지 처리
                }
            }
            
            yield return coolTime;
        }
    }
}
