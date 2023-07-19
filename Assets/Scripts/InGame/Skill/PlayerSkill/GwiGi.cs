using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwiGi : ActiveSkill
{
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public GwiGi(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override void Init()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
            projectile.SetAlpha(0.0f);
            projectile.CollisionPower(false);
            projectiles.Add(projectile);
        }
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime / 1000.0f);
        }

        while (true)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.transform.localPosition = Vector2.up * skillData.attackDistance;
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile.SetAlpha(1.0f);
                SkillManager.Instance.CoroutineStarter(Move(projectile));
            }
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime / 1000.0f);
        }
    }

    private IEnumerator Move(Projectile projectile)
    {
        projectile.CollisionPower(true);
        Vector3 rotate = CameraManager.Instance.GetMousePoint().x >= 0 ? Vector3.back : Vector3.forward;
        float angle = 0.0f;
        float weight = skillData.speed;
        while (angle < 95.0f)
        {
            weight += 0.1f;
            angle += weight;
            projectile.transform.RotateAround(shooter.position, rotate, weight);
            yield return waitForFixedUpdate;
        }
        projectile.CollisionPower(false);
        projectile.SetAlpha(0.0f);
    }

}
