using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoomerang : Projectile
{
    private bool isReturn;
    private float distance;

    protected override void Move()
    {
        float speed = Time.fixedDeltaTime * skillData.speed;
        if (!isReturn)
        {
            transform.Translate(direction * speed);
            distance += speed;
            isReturn = distance >= skillData.attackDistance;
        }
        else
        {
            transform.Translate(direction * speed * -1);
        }
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position;
        transform.localScale *= skillData.projectileSizeMulti;
        direction = pos;
        direction.Normalize();
        isReturn = false;
        distance = 0f;
        gameObject.SetActive(true);
    }

}
