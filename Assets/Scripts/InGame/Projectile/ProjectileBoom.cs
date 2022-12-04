using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoom : Projectile
{
    private float distance;

    protected override void Move()
    {
        float speed = skillData.speed * Time.fixedDeltaTime;
        transform.Translate(speed * direction);
        distance += speed;
        
        if(distance >= skillData.attackDistance)
        {

            gameObject.SetActive(false);
        }
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        distance = 0f;
        transform.position = caster.position;
        transform.localScale *= skillData.projectileSizeMulti;
        direction = pos;
        direction.Normalize();
        gameObject.SetActive(true);
    }
}
