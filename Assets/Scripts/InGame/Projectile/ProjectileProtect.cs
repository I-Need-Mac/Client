using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProtect : Projectile
{
    Transform caster;

    protected override void Move()
    {
        transform.position = caster.position;
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        this.caster = caster;
        transform.position = caster.position;
        transform.localScale *= skillData.projectileSizeMulti;
        gameObject.SetActive(true);
    }

}
