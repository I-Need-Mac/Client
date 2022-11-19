using UnityEngine;
using UnityEngine.UI;

public abstract class Projectile : MonoBehaviour
{
    private const float RELEASE_TIME = 2f; //투사체 소멸 시간

    private ProjectilePoolManager projectilePoolManager;

    protected Rigidbody2D projectileRigidBody;
    protected int coolTime;
    protected int attackDistance;
    protected int damage;
    protected int projectileCount;
    protected int speed;
    protected int splashRange;
    protected int projectileSizeMulti;
    protected bool isPenetrate;
    protected PROJECTILE_TYPE projectileType;
    protected Vector3 direction;
    //protected SkillData skillData;

    //각 투사체 타입별로 따로 구현
    //Fire -> 발사 / Move -> 투사체의 움직임
    protected abstract void Move();
    public abstract void Fire(Transform caster, Vector3 pos);
    
    private void Awake()
    {
        projectilePoolManager = FindObjectOfType<ProjectilePoolManager>();
        projectileRigidBody = GetComponent<Rigidbody2D>();
        direction = Vector3.right;
        //skillData = new SkillData();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //카메라에 안잡힐 때
    private void OnBecameInvisible()
    {
        Invoke("ReleaseProjectile", RELEASE_TIME);
    }

    private void ReleaseProjectile()
    {
        projectilePoolManager.DeSpawnProjectile(this, projectileType);
    }

    //풀에서 꺼내 쓸 때 스킬 정보를 업데이트하는 함수
    //스킬 레벨업시 데이터 변동을 고려하기 위함
    //Init 역할도 함
    public void SkillDataUpdate(SkillData skillData)
    {
        coolTime = skillData.coolTime;
        attackDistance = skillData.attackDistance;
        damage = skillData.damage;
        projectileCount = skillData.projectileCount;
        speed = skillData.speed;
        splashRange = skillData.splashRange;
        projectileSizeMulti = skillData.projectileSizeMulti;
        isPenetrate = skillData.isPenetrate;
        projectileType = skillData.projectileType;
    }
}
