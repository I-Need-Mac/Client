using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Drop : Projectile
{
    protected override void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData)
    {
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;

        float x = Random.Range(1, 10);
        x *= transform.position.x <= endPos.x ? 1 : -1; //대상과 시전자 위치 검사하여 좌우 방향 설정

        rb.AddForce(new Vector2(x, atkDis), ForceMode2D.Impulse);
    }

    protected override void Move()
    {
    }
}
