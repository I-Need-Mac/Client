
using UnityEngine;

public class ProjectileStraight : Projectile
{
    Vector3 movePos = Vector3.zero;

    protected override void Move()
    {
        movePos.Normalize();
        transform.Translate(movePos * Time.fixedDeltaTime * 7);
    }

    public override void Fire(Transform caster, Vector3 direction)
    {
        transform.position = caster.position;
        movePos = direction;
        gameObject.SetActive(true);
    }

}
