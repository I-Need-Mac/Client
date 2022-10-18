using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Satellite : Projectile
{
    private Transform caster;
    private Vector2 offset;

    protected override void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData)
    {
        this.caster = caster;

        transform.parent = caster.transform;
        //transform.position = (Vector2)caster.position + (Vector2.up * atkDis);
    }

    protected override void Move()
    {
        caster.parent.Rotate(0, 0, -Time.deltaTime * speed, Space.Self);
    }
}
