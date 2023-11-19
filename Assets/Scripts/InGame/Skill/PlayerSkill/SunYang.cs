using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SunYang : ActiveSkill
{
    private Projectile[] projectiles;

    private Vector2 look;

    public SunYang(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        projectiles = new Projectile[skillData.projectileCount];

        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
            if (shooter.TryGetComponent(out Player player))
            {
                look = player.lookDirection;
            }
            projectiles[i].transform.localPosition = look * skillData.attackDistance;
        }
        Debug.Log($"{look.x},{look.y}");

        yield return Move();

    }
    private IEnumerator Move()
    {
        Transform projectile1 = projectiles[0].transform;
        Transform projectile2 = projectiles[1].transform;

        float minDistance = 0.1f;
        float angle = 0f;
        float weight = 0.0f;
        do
        {
            weight += 0.2f;
            angle -= 1 + weight;

            projectile1.position = shooter.position + Quaternion.Euler(0, 0, angle) * look * skillData.attackDistance;
            projectile2.position = shooter.position + Quaternion.Euler(0, 0, -angle) * -look * skillData.attackDistance;

            yield return frame;
        } while (Vector2.Distance(projectile1.position, projectile2.position) > minDistance);
        for (int i = 0; i < projectiles.Length; i++)
        {
            SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        }
    }
}
