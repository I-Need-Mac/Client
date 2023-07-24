using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwiGi : ActiveSkill
{
    public GwiGi(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
                projectile.SetAlpha(0.0f);
                projectile.CollisionPower(false);
                projectile.transform.localPosition = Vector2.up * skillData.attackDistance;
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile.SetAlpha(1.0f);
                SkillManager.Instance.CoroutineStarter(Move(projectile));
            }

            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);
    }

    private IEnumerator Move(Projectile projectile)
    {
        projectile.CollisionPower(true);
        float angle = 0.0f;
        if (Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance).x >= 0)
        {
            do
            {
                angle -= Time.fixedDeltaTime * skillData.speed;
                projectile.transform.RotateAround(shooter.position, Vector3.forward, angle);
                yield return frame;
                DebugManager.Instance.PrintDebug("[GwiGi]: " + projectile.transform.localEulerAngles.z);
            } while (projectile.transform.localEulerAngles.z > 240.0f);
        }
        else
        {
            while (projectile.transform.localEulerAngles.z < 100.0f)
            {
                angle += Time.fixedDeltaTime * skillData.speed;
                projectile.transform.RotateAround(shooter.position, Vector3.forward, angle);
                yield return frame;
                DebugManager.Instance.PrintDebug("[GwiGi]: " + projectile.transform.localEulerAngles.z);
            }
        }
        projectile.CollisionPower(false);
        projectile.SetAlpha(0.0f);
    }

}
