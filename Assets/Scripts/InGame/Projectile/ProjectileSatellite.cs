
using UnityEngine;

public class ProjectileSatellite : Projectile
{
    private Transform caster;
    private Vector3 pos;
    private int count = 3;
    private float tempSpeed = 30f;
    private float radius = 4f;

    protected override void Move()
    {
        //transform.position = caster.position + (transform.position - caster.position).normalized * radius;
        //transform.RotateAround(caster.position, Vector3.forward, tempSpeed * Time.fixedDeltaTime);
        //transform.position = caster.position + new Vector3(
        //    Mathf.Cos((angle + Time.fixedDeltaTime)%360f * Mathf.Deg2Rad),
        //    Mathf.Sin((angle + Time.fixedDeltaTime)%360f * Mathf.Deg2Rad), 0f
        //    ) * radius;
        angle += Time.fixedDeltaTime * tempSpeed;
        transform.position = caster.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        this.pos = pos;
        transform.position = caster.position + pos * radius;
        this.caster = caster;
        gameObject.SetActive(true);
    }
    //new Vector3(Mathf.Cos(45f * Mathf.Deg2Rad), Mathf.Sin(45f * Mathf.Deg2Rad), 0)
}
