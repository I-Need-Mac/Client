using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horin : ActiveSkill
{
    Projectile[] projectiles;

    public Horin(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            projectiles = new Projectile[skillData.projectileCount];
            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i] = SkillManager.Instance.SpawnProjectile(skillData, shooter);
                originSize = projectiles[i].transform.localScale * skillData.projectileSizeMulti;
                projectiles[i].transform.localScale = Vector2.zero;
                projectiles[i].transform.localPosition = Vector2.up * skillData.attackDistance;
                projectiles[i].transform.localEulerAngles = Vector3.zero;
                float angle = 360 * i / skillData.projectileCount;
                projectiles[i].transform.RotateAround(shooter.position, Vector3.back, angle);
                projectiles[i].CollisionPower(false);
            }

            yield return Move();
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);
    }

    private IEnumerator Move()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].CollisionPower(true);
        }

        float time = 0.0f;
        while(time < skillData.duration)
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                if (projectiles[i].transform.localScale.x < 1.0f && time <= 1.0f)
                {
                    projectiles[i].transform.localScale = originSize * time;
                }
                if (skillData.duration - time <= 1.0f)
                {
                    projectiles[i].transform.localScale = originSize * (skillData.duration - time);
                }
                projectiles[i].transform.RotateAround(shooter.position, Vector3.back, skillData.speed);
            }
            time += Time.fixedDeltaTime;
            yield return frame;
        }

        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].CollisionPower(false);
        }
    }
}
