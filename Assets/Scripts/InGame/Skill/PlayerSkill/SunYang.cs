using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunYang : ActiveSkill
{
    private Projectile[] projectiles;

    public SunYang(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);
        projectiles = new Projectile[2];

        for (int i = 0; i < 2; i++)
        {
            projectiles[i] = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter, false);
            projectiles[i].transform.localPosition = (i == 0) ? Vector2.right*skillData.attackDistance : Vector2.right * -skillData.attackDistance;
        }

        yield return Move();
    }

    private IEnumerator Move()
    {
        Transform projectile1 = projectiles[0].transform;
        Transform projectile2 = projectiles[1].transform;

        float minDistance = 0.1f;
        float angle = 0.0f;
        float weight = 0.0f;

        //while (Vector2.Distance(projectile1.position,projectile2.position)>minDistance)
        //{
        //    weight += 0.002f;
        //    angle -= Time.fixedDeltaTime * skillData.speed + weight;
        //    projectile1.transform.RotateAround(shooter.position, Vector3.forward, angle);
        //    projectile2.transform.RotateAround(shooter.position, -Vector3.forward, angle);
        //    yield return frame;
        //}
        float radius = skillData.attackDistance;

        while (Vector2.Distance(projectile1.position, projectile2.position) > minDistance)
        {
            weight += 0.002f;
            angle -= 1+ weight;

            projectile1.position = shooter.position + Quaternion.Euler(0, 0, angle) * Vector2.right * radius;
            projectile2.position = shooter.position + Quaternion.Euler(0, 0, -angle) * Vector2.left * radius;

            yield return frame;
        }
        for (int i=0; i<projectiles.Length;i++)
        {
            SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        }
    }
}
