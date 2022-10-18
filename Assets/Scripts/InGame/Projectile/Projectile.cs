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

    protected float atkDis; //���� ��Ÿ�

    private bool isMyProjectile; //�÷��̾�, �� �Ѿ� ����
    private bool isPenetrate;   //���� ����

    //����ü Ȱ��ȭ ����
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
        Remove(); //ī�޶� ������ ������ �� ������Ʈ ��Ȱ��ȭ
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
                    //TODO :: ���� hp ����
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
                    //TODO :: �÷��̾� hp ����
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

                Invoke("Remove", 1f); //���� ���ӽð�
            }
            //���� x, ������Ʈ ��Ȱ��ȭ
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
            //������Ʈ Ǯ�� �ش� ����ü ������Ʈ ��ȯ
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

        //rigid body �̵� �ʱ�ȭ
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public void Fire(Transform caster, Vector2 endPos, SkillData skillData)
    {
        transform.position = caster.position; //������ ��ġ�� �ʱ�ȭ

        speed = skillData.speed;
        atkDis = skillData.atkDis;
        projectileType = skillData.projectileType;
        splashRange = skillData.splashRange;
        transform.localScale = Vector2.one * skillData.projectileSizeMulti; //ũ�� ������ŭ ����

        ActiveSetting(caster, endPos, skillData); //����ü Ȱ��ȭ ����

        gameObject.SetActive(true);
    }
    #endregion
}
