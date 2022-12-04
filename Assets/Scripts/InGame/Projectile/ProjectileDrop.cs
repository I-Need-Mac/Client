using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDrop : Projectile
{
    private const float COEFFICIENT = 0.4f;

    protected override void Move()
    {
        transform.Translate(skillData.speed / COEFFICIENT * Time.fixedDeltaTime * direction);
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position;
        transform.localScale *= skillData.projectileSizeMulti;
        projectileRigidBody.gravityScale = skillData.attackDistance * COEFFICIENT;
        direction = new Vector3(0, skillData.attackDistance, 0);
        direction.x += Random.Range(-1 * skillData.attackDistance, skillData.attackDistance) * COEFFICIENT / 2;
        direction.Normalize();
        gameObject.SetActive(true);
    }
}
