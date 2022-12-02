
using UnityEngine;

public class ProjectileSatellite : Projectile
{
    private Transform caster;

    protected override void Move()
    {
        //angle += Time.fixedDeltaTime * skillData.speed;
        //transform.position = caster.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * 5;

        Quaternion rotate = Quaternion.Euler(0, 0, (skillData.speed * 10) * Time.fixedDeltaTime);
        direction = (rotate * direction).normalized;
        transform.position = caster.position + direction * skillData.attackDistance;
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position + pos * skillData.attackDistance;
        direction = transform.position - caster.position;
        this.caster = caster;
        gameObject.SetActive(true);
    }
    //new Vector3(Mathf.Cos(45f * Mathf.Deg2Rad), Mathf.Sin(45f * Mathf.Deg2Rad), 0)
}
