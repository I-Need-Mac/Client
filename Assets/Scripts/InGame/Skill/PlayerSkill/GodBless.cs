using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBless : ActiveSkill
{
    private List<Projectile> projectiles = new List<Projectile>();

    public GodBless(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
            projectile.transform.localScale = Vector2.one * skillData.splashRange;
            projectiles.Add(projectile);

            if (skillData.splashRange >= 10)
            {
                projectile.CollisionPower(false);

                foreach (Monster monster in MonsterSpawner.Instance.monsters)
                {
                    if (CameraManager.Instance.IsTargetVisible(monster.transform.position))
                    {
                        monster.Hit(skillData.damage);
                    }
                }
            }
        }

        yield return intervalTime;

        foreach (Projectile projectile in projectiles)
        {
            SkillManager.Instance.DeSpawnProjectile(projectile);
        }
    }
}
