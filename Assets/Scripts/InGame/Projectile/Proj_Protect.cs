using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Protect : Projectile
{
    private Transform caster;

    private float curTime;
    private float activeTime;

    protected override void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData)
    {
        this.caster = caster;
        curTime = 0;
        //activeTime = ;
    }

    protected override void Move()
    {
        transform.position = caster.position;

        curTime += Time.deltaTime;

        //투사체 지속 시간 검사 및 오브젝트 비활성화
        if (curTime >= /*activeTime*/3f)
        {
            Remove();
        }
    }
}
