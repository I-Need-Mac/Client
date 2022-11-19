using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;

    private Stack<Projectile> pool;

    private void Awake()
    {
        pool = new Stack<Projectile>();
        AddProjectile();
    }

    //projectile을 사용하기 위해 pool에서 꺼내는 함수
    public Projectile GetProjectile()
    {
        if (pool.Count == 0)
        {
            AddProjectile();
        }
        Projectile projectile = pool.Pop();
        projectile.gameObject.transform.SetParent(transform);
        //projectile.gameObject.SetActive(true);
        return projectile;
    }

    //미사용 projectile을 pool에 반환하는 함수
    public void ReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.gameObject.transform.SetParent(transform);
        pool.Push(projectile);
    }

    //pool에 더이상 projectile이 존재하지 않는 경우 추가 생성해주는 함수
    //n개 단위로 생성할 정도로 많은 양을 사용하는 것이 아니기 때문에 1개씩 생성
    public void AddProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform);
        projectile.gameObject.SetActive(false);
        pool.Push(projectile);
    }

}
