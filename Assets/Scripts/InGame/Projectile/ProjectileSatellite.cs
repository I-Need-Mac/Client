
using UnityEngine;

public class ProjectileSatellite : Projectile
{
    private Transform caster;
    private int count = 3;
    private float tempSpeed = 120f;
    private float radius = 4f;
    private float angle = 0f;

    protected override void Move()
    {
        transform.position = caster.position + (transform.position - caster.position).normalized * radius;
        transform.RotateAround(caster.position, Vector3.forward, tempSpeed * Time.fixedDeltaTime);
        //angle += tempSpeed * Time.fixedDeltaTime;
        //Vector3 offset = Quaternion.Euler(0f, angle, 0f) * new Vector3(radius, 0f, 0f);
        //transform.position = new Vector3(caster.position.x, caster.position.y, caster.position.z) + offset;
    }

    public override void Fire(Transform caster, Vector3 pos)
    {
        transform.position = caster.position * 10;
        this.caster = caster;
        gameObject.SetActive(true);
    }

}
