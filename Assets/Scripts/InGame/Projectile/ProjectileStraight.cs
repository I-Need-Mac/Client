
using UnityEngine;

public class ProjectileStraight : Projectile
{
    protected override void Move()
    {
        transform.Translate(skillData.speed * Time.fixedDeltaTime * direction);
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position;
        transform.localScale = Vector3.one * skillData.projectileSizeMulti;
        direction = pos;
        direction.Normalize();
        gameObject.SetActive(true);
    }

}
