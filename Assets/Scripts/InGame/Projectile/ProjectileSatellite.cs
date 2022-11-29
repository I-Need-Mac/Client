
using UnityEngine;

public class ProjectileSatellite : Projectile
{
    private Transform caster;

    protected override void Move()
    {
        //transform.position = caster.position + (transform.position - caster.position).normalized * radius;
        //transform.RotateAround(caster.position, Vector3.forward, tempSpeed * Time.fixedDeltaTime);
        //transform.position = caster.position + new Vector3(
        //    Mathf.Cos((angle + Time.fixedDeltaTime)%360f * Mathf.Deg2Rad),
        //    Mathf.Sin((angle + Time.fixedDeltaTime)%360f * Mathf.Deg2Rad), 0f
        //    ) * radius;
        //Quaternion.AngleAxis(angle, axis);
        //Quaternion.AngleAxis(angle, Vector3.forward);

        //angle = Time.fixedDeltaTime * tempSpeed;
        //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //Vector3 v = caster.position - transform.position;
        //v = q * v;
        //transform.position = caster.position + v;

        //angle += Time.fixedDeltaTime * tempSpeed;
        //transform.position = caster.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;

        Quaternion rotate = Quaternion.Euler(0, 0, skillData.speed * Time.fixedDeltaTime);
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
