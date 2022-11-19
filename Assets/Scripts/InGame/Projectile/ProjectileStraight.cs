
using UnityEngine;

public class ProjectileStraight : Projectile
{
    protected override void Move()
    {
        direction.Normalize();
        transform.Translate(7 * Time.fixedDeltaTime * direction);
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position;
        direction = pos;
        gameObject.SetActive(true);
    }

}
