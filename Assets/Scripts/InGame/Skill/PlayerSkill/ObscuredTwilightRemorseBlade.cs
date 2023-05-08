using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscuredTwilightRemorseBlade : Skill
{
    public ObscuredTwilightRemorseBlade(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
        projectile.transform.localScale = Vector2.zero;

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
            projectiles[0].transform.localScale = Vector2.one * skillData.projectileSizeMulti;
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                projectiles[0].CollisionPower(true);
                yield return intervalTime;
                projectiles[0].CollisionPower(false);
                
            }
            projectiles[0].transform.localScale = Vector2.zero;
            yield return coolTime;
        }

        //SkillManager.Instance.DeSpawnProjectile(_projectile);
        //projectiles.Clear();
    }
    
}
