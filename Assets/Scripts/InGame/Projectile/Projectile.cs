using UnityEngine;
using UnityEngine.UI;

public abstract class Projectile : MonoBehaviour
{
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
    //protected SkillData skillData;

    //각 투사체 타입별로 따로 구현
    //Fire -> 발사 / Move -> 투사체의 움직임
    protected abstract void Move();
    public abstract void Fire(Transform caster, Vector3 destination);
    
    private void Awake()
    {
        projectilePoolManager = FindObjectOfType<ProjectilePoolManager>();
        projectileRigidBody = GetComponent<Rigidbody2D>();
        //skillData = new SkillData();
    }


    //풀에서 꺼내 쓸 때 스킬 정보를 업데이트하는 함수
    //스킬 레벨업시 데이터 변동을 고려하기 위함
    //Init 역할도 함
    //public void SkillDataUpdate(SkillData skillData)
    //{
    //    this.skillData = skillData;
    //}
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
    }
}
