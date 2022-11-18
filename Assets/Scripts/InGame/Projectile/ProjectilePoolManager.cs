using BFM;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoSingleton<ProjectilePoolManager>
{
    [SerializeField] ProjectilePool straight;
    [SerializeField] ProjectilePool protect;
    [SerializeField] ProjectilePool satellite;
    [SerializeField] ProjectilePool drop;
    [SerializeField] ProjectilePool boomerang;
    [SerializeField] ProjectilePool range;
    [SerializeField] ProjectilePool boom;

    private Dictionary<PROJECTILE_TYPE, ProjectilePool> pools;

    private void Awake()
    {
        pools = new Dictionary<PROJECTILE_TYPE, ProjectilePool>();
        pools.Add(PROJECTILE_TYPE.STRAIGHT, straight);
        pools.Add(PROJECTILE_TYPE.PROTECT, protect);
        pools.Add(PROJECTILE_TYPE.SATELLITE, satellite);
        pools.Add(PROJECTILE_TYPE.DROP, drop);
        pools.Add(PROJECTILE_TYPE.BOOMERANG, boomerang);
        pools.Add(PROJECTILE_TYPE.RANGE, range);
        pools.Add(PROJECTILE_TYPE.BOOM, boom);

    }

    public Projectile SpawnProjectile(PROJECTILE_TYPE type = PROJECTILE_TYPE.STRAIGHT)
    {
        return pools[type].GetProjectile();
    }

    public void DeSpawnProjectile(Projectile projectile, PROJECTILE_TYPE type = PROJECTILE_TYPE.STRAIGHT)
    {
        pools[type].ReleaseProjectile(projectile);
    }

}
