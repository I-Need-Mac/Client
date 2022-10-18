using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Boomerang : Projectile
{
    private Vector2 endPos;
    private Vector2 returnPos;

    private bool isReturn;

    protected override void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData)
    {
        isReturn = false;
        this.endPos = endPos;

        //이동 위치의 반대편 위치를 돌아갈 위치로 지정
        returnPos = (Vector3)endPos - transform.position;
        returnPos.Normalize();
        returnPos *= -1;
    }

    protected override void Move()
    {
        if (!isReturn)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, speed * 0.2f * Time.deltaTime);

            //현재 위치가 이동 목표 위치에 달하면 반대로 이동
            if (Vector2.Distance(transform.position, endPos) <= 0.5f)
            {
                isReturn = true;
            }
        }
        else
        {
            transform.Translate(returnPos * Time.deltaTime * speed);
        }
    }
}
