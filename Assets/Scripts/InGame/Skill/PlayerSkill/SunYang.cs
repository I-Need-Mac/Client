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

        float diagonalCorrection = Mathf.Abs(look.x) == Mathf.Abs(look.y) ? 0.7f : 1.0f;

        float projectileSpacing = 5.0f;

        float offsetY = projectileSpacing * (i - (projectiles.Length - 1) * 0.5f) * diagonalCorrection;
        Vector2 offset = new Vector2(look.y, -look.x) * offsetY;
        projectiles[i].transform.localPosition = look * skillData.attackDistance + offset;
    }
        yield return Move();
    }
    private IEnumerator Move()
    {
        Projectile projectile1 = projectiles[0];
        Projectile projectile2 = projectiles[1];
        float angle = 0.0f;
        float weight = 0.0f;
        do
        {
            weight += 0.002f;
            angle -= Time.fixedDeltaTime * skillData.speed + weight;
            projectile1.transform.RotateAround(shooter.position, Vector3.forward, angle);
            projectile2.transform.RotateAround(shooter.position, Vector3.back, angle);
            yield return frame;
        } while (Vector2.Distance(projectile1.transform.position, projectile2.transform.position) > 0.1f);
        for(int i = 0; i < projectiles.Length; i++)
        {
            SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        }       
    }
}