using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [Header("--- Projectile Pool ---")]
    [SerializeField] private ObjectPool proj_straightPool;
    [SerializeField] private ObjectPool proj_protectPool;
    [SerializeField] private ObjectPool proj_satellitePool;
    [SerializeField] private ObjectPool proj_dropPool;
    [SerializeField] private ObjectPool proj_boomerangPool;
    [SerializeField] private ObjectPool proj_rangePool;

    //ProjectileType에 맞는 오브젝트 풀에서 투사체 가져오기
    public Projectile GetProjectile(ProjectileType projectileType)
    {
        ObjectPool projectilePool = GetProjectilePool(projectileType);

        if (projectilePool != null)
        {
            GameObject obj = projectilePool.GetObject();

            if (obj != null)
            {
                Projectile projectile = obj.GetComponent<Projectile>();

                if (projectile != null)
                {
                    return projectile;
                }
                else
                {
                    Debug.LogError("[GetProjectile] Can't GetComponent Projectile. Check prefab in inspector.\n" +
                        "projectileType : " + projectileType);
                }
            }
            else
            {
                Debug.LogError("[GetProjectile] obj is null. Check prefab in inspector.\n" +
                    "projectileType : " + projectileType);

                throw new NullReferenceException();
            }
        }
        else
        {
            Debug.LogError("[GetProjectile] projectilePool is null. \n" +
                "projectileType : " + projectileType);

            throw new NullReferenceException();
        }

        return null;
    }

    //projectileType에 맞는 오브젝트 풀에 투사체 반환
    public void ReturnProjectile(ProjectileType projectileType, GameObject obj)
    {
        ObjectPool projectilePool = GetProjectilePool(projectileType);

        if (projectilePool != null)
        {
            projectilePool.ReturnObject(obj);
        }
        else
        {
            Debug.LogError("[ReturnProjectile] projectilePool is null. \n" +
                "projectileType : " + projectileType);
        }
    }

    //projectileType에 맞는 오브젝트 풀 return
    private ObjectPool GetProjectilePool(ProjectileType projectileType)
    {
        ObjectPool projectilePool = null;

        switch (projectileType)
        {
            case ProjectileType.Straight:
                projectilePool = proj_straightPool;
                break;

            case ProjectileType.Protect:
                projectilePool = proj_protectPool;
                break;

            case ProjectileType.Satellite:
                projectilePool = proj_satellitePool;
                break;

            case ProjectileType.Drop:
                projectilePool = proj_dropPool;
                break;

            case ProjectileType.Boomerang:
                projectilePool = proj_boomerangPool;
                break;

            case ProjectileType.Range:
                projectilePool = proj_rangePool;
                break;
        }

        return projectilePool;
    }
}
