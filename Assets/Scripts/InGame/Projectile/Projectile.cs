using UnityEngine;
using UnityEngine.UI;

public abstract class Projectile : MonoBehaviour
{
    private ProjectilePool projectilePool;

    protected Rigidbody2D rigidBody;
    protected Image image;
    protected int projectileCount;
    protected int speed;
    protected int splashRange;
    protected int projectileSizeMulti;
    protected bool isPenetrate;
    protected PROJECTILE_TYPE projectileType;
}
