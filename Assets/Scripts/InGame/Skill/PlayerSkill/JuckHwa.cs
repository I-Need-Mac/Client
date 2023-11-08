using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuckHwa : ActiveSkill
{
    private List<Projectile> projectiles = new List<Projectile>();
    public JuckHwa(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        float ignitionlTime = 0.1f;
        float extinguishTime = 0.3f;
        float activateTime = 0.0f;
        Debug.Log("적화 발동");
        while (activateTime < skillData.duration)
        {
            //float currentTime=Time.time;
            if (ignitionlTime<=0.0f)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
                projectile.transform.position = shooter.position;
                projectiles.Add(projectile);
                ignitionlTime = 0.2f;
            }
            else
            {
                ignitionlTime -= Time.fixedDeltaTime;
            }
            if (extinguishTime<=0.0f)
            {
                if (projectiles.Count > 0)
                {
                    SkillManager.Instance.DeSpawnProjectile(projectiles[0]);
                    projectiles.RemoveAt(0);
                    extinguishTime = 0.3f;
                }
            }
            else
            {
                extinguishTime -= Time.fixedDeltaTime;
            }            
            activateTime += Time.fixedDeltaTime;
            yield return frame;
        }
    }
}
