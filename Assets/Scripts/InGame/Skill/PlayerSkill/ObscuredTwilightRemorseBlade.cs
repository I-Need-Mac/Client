using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscuredTwilightRemorseBlade : Skill
{
    public ObscuredTwilightRemorseBlade(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
        Transform collider = projectile.transform.Find("SkillCollider");
        //projectile.transform.localScale = new Vector2(skillData.attackDistance, 0.025f * skillData.projectileSizeMulti);
        collider.localPosition = projectile.transform.right * skillData.attackDistance * 0.5f;
        collider.rotation = Quaternion.Euler(0, 0, 0);
        projectiles.Add(projectile);
    }

    public override IEnumerator Activation()
    {
        WaitForSeconds coolTime = new WaitForSeconds(skillData.coolTime / 10000.0f);
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        WaitForSeconds intervalTime = new WaitForSeconds(skillData.intervalTime);
        Projectile _projectile = null;
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            float angle = 1.0f;
            while (angle < 360.0f)
            {
                foreach (Projectile projectile in projectiles)
                {
                    projectile.transform.Find("SkillCollider").RotateAround(shooter.position, Vector3.back, skillData.speed * Time.deltaTime);
                    angle += 1.0f;
                    _projectile = projectile;
                }
                yield return null;
            }
            yield return intervalTime;
        }

        SkillManager.Instance.DeSpawnProjectile(_projectile);
        projectiles.Clear();
    }
    
}
