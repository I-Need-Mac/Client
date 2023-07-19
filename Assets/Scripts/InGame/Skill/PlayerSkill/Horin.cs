using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horin : ActiveSkill
{
    public Horin(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override void Init()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
            originSize = projectile.transform.localScale;
            projectile.transform.localScale = Vector2.zero;
            Vector3 rotate = Vector3.forward * 360 * i / skillData.projectileCount;
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            projectile.transform.Rotate(rotate);
            projectile.transform.localPosition = projectile.transform.up * skillData.attackDistance;
            projectiles.Add(projectile);
        }
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        float weight = 0.004f;
        float time = 0.0f;
        float size = 0.0f;

        while (true)
        {
            if (time >= skillData.duration && size <= 0.0f)
            {
                time = 0.0f;
                foreach (Projectile projectile in projectiles)
                {
                    projectile.CollisionPower(false);
                }
                yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
                foreach (Projectile projectile in projectiles)
                {
                    projectile.CollisionPower(true);
                }
            }

            if (time >= skillData.duration)
            {
                size -= weight;
            }
            else if (size < 1)
            {
                size += weight;
            }
            foreach (Projectile projectile in projectiles)
            {
                projectile.transform.RotateAround(shooter.position, Vector3.back, skillData.speed * Time.deltaTime);
                projectile.transform.localScale = originSize * size;
            }
            time += Time.deltaTime;
            yield return null;
        }
    }
}
