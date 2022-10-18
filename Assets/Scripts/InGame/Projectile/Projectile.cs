using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    private ProjectilePool projectilePool;
    protected Rigidbody2D rb;
    protected Image image;

    protected ProjectileType projectileType;

    protected int speed;
    private int splashRange;

    protected float atkDis; //공격 사거리

    private bool isMyProjectile; //플레이어, 적 총알 구분
    private bool isPenetrate;   //관통 여부

    //투사체 활성화 세팅
    protected abstract void ActiveSetting(Transform caster, Vector2 endPos, SkillData skillData);
    protected abstract void Move();

    #region Setter
    public void SetIsMyProjectile(bool isMyProjectile)
    {
        this.isMyProjectile = isMyProjectile;
    }
    #endregion

    #region MonoBehaviour Method
    private void Awake()
    {
        projectilePool = FindObjectOfType<ProjectilePool>(true);
        rb = GetComponent<Rigidbody2D>();

        Init();
    }

    private void Update()
    {
        Move();
    }

    private void OnBecameInvisible()
    {
        Remove(); //카메라 밖으로 나갔을 때 오브젝트 비활성화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isCollision = (isMyProjectile && collision.CompareTag("Monster"))
            || (!isMyProjectile && collision.CompareTag("Player"));

        if (isCollision)
        {
            if (isMyProjectile)
            {
                var monster = collision.GetComponent<Monster>();

                if (monster != null)
                {
                    //TODO :: 몬스터 hp 감소
                }
                else
                {
                    Debug.LogError("[Projectile.OnTriggerEnter2D] isMyProjectile : true\ncollision : " + collision);
                }
            }
            else
            {
                var player = collision.GetComponent<Player>();

                if (player != null)
                {
                    //TODO :: 플레이어 hp 감소
                }
                else
                {
                    Debug.LogError("[Projectile.OnTriggerEnter2D] isMyProjectile : false\ncollision : " + collision);
                }
            }

            if (projectileType == ProjectileType.Boom)
            {
                transform.localScale = Vector2.one * splashRange;
                speed = 0;

                Invoke("Remove", 1f); //폭발 지속시간
            }
            //관통 x, 오브젝트 비활성화
            else if (!isPenetrate)
            {
                Remove();
            }
        }
    }
    #endregion

    protected void Remove()
    {
        if (projectilePool != null)
        {
            //오브젝트 풀에 해당 투사체 오브젝트 반환
            projectilePool.ReturnProjectile(projectileType, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.LogError("[Projectile.Remove] projectilePool is null");
        }
    }

    #region Public Method
    public void Init()
    {
        projectileType = ProjectileType.Straight;
        speed = 0;
        atkDis = 0;
        isPenetrate = false;

        //rigid body 이동 초기화
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public void Fire(Transform caster, Vector2 endPos, SkillData skillData)
    {
        transform.position = caster.position; //시전자 위치로 초기화

        speed = skillData.speed;
        atkDis = skillData.atkDis;
        projectileType = skillData.projectileType;
        splashRange = skillData.splashRange;
        transform.localScale = Vector2.one * skillData.projectileSizeMulti; //크기 배율만큼 설정

        ActiveSetting(caster, endPos, skillData); //투사체 활성화 세팅

        gameObject.SetActive(true);
    }
    #endregion
}
