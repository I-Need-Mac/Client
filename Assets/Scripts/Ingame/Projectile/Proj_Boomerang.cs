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

        //�̵� ��ġ�� �ݴ��� ��ġ�� ���ư� ��ġ�� ����
        returnPos = (Vector3)endPos - transform.position;
        returnPos.Normalize();
        returnPos *= -1;
    }

    protected override void Move()
    {
        if (!isReturn)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, speed * 0.2f * Time.deltaTime);

            //���� ��ġ�� �̵� ��ǥ ��ġ�� ���ϸ� �ݴ�� �̵�
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
