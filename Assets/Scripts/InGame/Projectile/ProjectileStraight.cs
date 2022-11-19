
using UnityEngine;

public class ProjectileStraight : Projectile
{
    protected override void Move()
    {
        movePos.Normalize();
        transform.Translate(7 * Time.fixedDeltaTime * movePos);
    }

    public override void Fire(Transform caster, Vector3 direction)
    {
        transform.position = caster.position;
        movePos = direction;
        gameObject.SetActive(true);
    }

}
