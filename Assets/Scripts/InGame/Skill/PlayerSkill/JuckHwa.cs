using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuckHwa : ActiveSkill
{
    private List<Projectile> projectiles = new List<Projectile>();
    public JuckHwa(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        float ignitionlTime = 0.0f;

        float activateTime = 0.0f;
        while (activateTime < skillData.duration)
        {
            if (ignitionlTime >= 0.2f)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
                projectile.transform.position = shooter.position;
                projectiles.Add(projectile);
                ignitionlTime = 0.0f;
            }
            else
            {
                ignitionlTime += Time.fixedDeltaTime;
            }

            activateTime += Time.fixedDeltaTime;
            yield return frame;
        }

        foreach (var projectile in projectiles)
        {
            SkillManager.Instance.DeSpawnProjectile(projectile);
        }
    }
    //private IEnumerator ExtinguishProjectile()
    //{
    //    float time = 0.0f;
    //    while (time < skillData.duration)
    //    {
    //        for (int i = 0; i < projectiles.Count; i++)
    //        {
    //            if (skillData.duration - time <= 0.5f)
    //            {
    //                projectiles[i].transform.localScale = originSize * (skillData.duration - time);
    //            }
    //        }
    //        time += Time.fixedDeltaTime;
    //        yield return frame;
    //    }

    //    for (int i = 0; i < projectiles.Count; i++)
    //    {
    //        SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
    //    }
    //}
}

