using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoomerang : Projectile
{
    bool isReturn = false;
    int attackDistanceTemp = 7;

    protected override void Move()
    {
        if (!isReturn)
        {
            transform.Translate(5 * Time.fixedDeltaTime * direction);
            if(Vector2.Distance(transform.position, direction * attackDistanceTemp) <= 2f)
            {
                DebugManager.Instance.PrintDebug("돌아가!");
                direction *= -1;
                isReturn = true;
            }
        }
        transform.Translate(5 * Time.fixedDeltaTime * direction);
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position;
        isReturn = false;
        direction = pos;
        direction.Normalize();
        gameObject.SetActive(true);
    }

}
