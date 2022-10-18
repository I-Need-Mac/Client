using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Straight : Projectile
{
    private Vector2 movePos;

    protected override void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData)
    {
        movePos = (Vector3)endPos - transform.position;
        movePos.Normalize();
    }

    protected override void Move()
    {
        transform.Translate(movePos * Time.deltaTime * speed);
    }
}
