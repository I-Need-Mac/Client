using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraight : Projectile
{
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * skillData.speed * Time.deltaTime);
    }
}
