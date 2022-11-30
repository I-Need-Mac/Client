
using UnityEngine;

public class ProjectileStraight : Projectile
{
    protected override void Move()
    {
        transform.Translate(skillData.speed * Time.deltaTime * direction);
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position;
        direction = pos;
        direction.Normalize();
        gameObject.SetActive(true);
    }

}
