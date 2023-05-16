using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraight : Projectile
{
    private float shiftingDistance = 0.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        shiftingDistance = 0.0f;
    }

    private void FixedUpdate()
    {
        if (shiftingDistance <= skillData.attackDistance)
        {
            float speed = skillData.speed * Time.deltaTime;
            transform.Translate(Vector2.up * speed);
            shiftingDistance += speed;
        }
        else
        {
            Remove();
        }
    }
}
