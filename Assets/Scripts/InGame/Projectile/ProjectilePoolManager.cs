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
        pools = new Dictionary<PROJECTILE_TYPE, ProjectilePool>
        {
            { PROJECTILE_TYPE.STRAIGHT, straight },
            { PROJECTILE_TYPE.PROTECT, protect },
            { PROJECTILE_TYPE.SATELLITE, satellite },
            { PROJECTILE_TYPE.DROP, drop },
            { PROJECTILE_TYPE.BOOMERANG, boomerang },
            { PROJECTILE_TYPE.RANGE, range },
            { PROJECTILE_TYPE.BOOM, boom }
        };

    }

    public Projectile SpawnProjectile(PROJECTILE_TYPE type, SkillData skillData)
    {
        Projectile projectile = pools[type].GetProjectile();
        projectile.SkillDataUpdate(skillData); //스킬 정보 업데이트
        return projectile;
    }

    public void DeSpawnProjectile(Projectile projectile, PROJECTILE_TYPE type)
    {
        pools[type].ReleaseProjectile(projectile);
    }

}
