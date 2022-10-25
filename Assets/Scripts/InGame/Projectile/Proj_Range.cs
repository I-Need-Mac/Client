using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Range : Projectile
{
    private Transform caster;

    private float dirX;
    private float curTime;
    private float activeTime;

    protected override void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData)
    {
        this.caster = caster;
        curTime = 0;
        //activeTime = ;

        //���� ������ ��ġ �˻��Ͽ� �¿� ���� ����
        dirX = transform.position.x <= endPos.x ? 1 : -1;

        transform.position = new Vector2(
            caster.position.x + (caster.localScale.x * 0.5f) + (transform.localScale.x * 0.5f),
            caster.position.y);
    }

    protected override void Move()
    {
        //���� ������ ��ġ �˻��Ͽ� �¿� ���� ����
        float x = dirX < 0 ?
            caster.position.x - (caster.localScale.x * 0.5f) - (transform.localScale.x * 0.5f)
            : caster.position.x + (caster.localScale.x * 0.5f) + (transform.localScale.x * 0.5f);

        transform.position = new Vector2(x, caster.position.y);

        curTime += Time.deltaTime;

        //����ü ���� �ð� �˻� �� ������Ʈ ��Ȱ��ȭ
        if (curTime >= /*activeTime*/0.3f)
        {
            Remove();
        }
    }
}
