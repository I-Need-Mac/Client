using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwiGi : Skill
{
    public GwiGi(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
            projectile.SetAlpha(0.0f);
            projectiles.Add(projectile);
        }
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return coolTime;
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
            yield return coolTime;
        }
    }

    private IEnumerator Move(Projectile projectile)
    {
        Vector3 rotate = GameManager.Instance.player.lookDirection.x >= 0 ? Vector3.back : Vector3.forward;
        float angle = 0.0f;
        float weight = skillData.speed * Time.deltaTime;
        while (angle < 95.0f)
        {
            weight += 0.001f;
            angle += weight;
            projectile.transform.RotateAround(shooter.position, rotate, weight);
            yield return null;
        }
        projectile.SetAlpha(0.0f);
    }

}
