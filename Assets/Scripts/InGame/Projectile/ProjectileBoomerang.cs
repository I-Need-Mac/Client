using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoomerang : Projectile
{
    private bool isReturn;
    private float distance;
    private Vector3 v = Vector3.zero;
    private float speed;

    protected override void Move()
    {
        //float speed = Time.fixedDeltaTime * skillData.speed;
        float speed = Time.fixedDeltaTime * skillData.speed;
        if (!isReturn)
        {
            if(distance > skillData.attackDistance * 0.8f)
            {
                speed *= 0.2f;
            }
            transform.Translate(direction * speed);
            //transform.position = Vector3.SmoothDamp(transform.position, transform.position + direction * skillData.attackDistance, ref v, 0.5f);
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
