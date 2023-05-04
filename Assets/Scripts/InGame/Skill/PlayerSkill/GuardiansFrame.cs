using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardiansFrame : Skill
{
    public GuardiansFrame(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
            Vector3 rotate = Vector3.forward * 360 * i / skillData.projectileCount;
            projectile.transform.Rotate(rotate);
            projectile.transform.localPosition = projectile.transform.up * skillData.attackDistance;
            projectiles.Add(projectile);
        }
    }

    public override IEnumerator Activation()
    {
        WaitForSeconds coolTime = new WaitForSeconds(skillData.coolTime / 10000.0f);
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        float time = 0.0f;
        while (true)
        {
            if (time >= skillData.duration)
            {
                yield return coolTime;
                time = 0.0f;
            }

            foreach (Projectile projectile in projectiles)
            {
                projectile.transform.RotateAround(shooter.position, Vector3.back, skillData.speed * Time.deltaTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }
}
