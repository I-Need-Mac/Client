using UnityEngine;
using UnityEngine.UI;

public abstract class Projectile : MonoBehaviour
{
    private const float RELEASE_TIME = 5f; //투사체 소멸 시간

    protected Rigidbody2D projectileRigidBody;
    //protected int coolTime;
    //protected int attackDistance;
    //protected int damage;
    //protected int projectileCount;
    //protected int speed;
    //protected int splashRange;
    //protected int projectileSizeMulti;
    //protected bool isPenetrate;
    //protected PROJECTILE_TYPE projectileType;
    protected Vector3 direction;
    protected SkillData skillData;

    public float angle { get; set; }

    //각 투사체 타입별로 따로 구현
    //Fire -> 발사 / Move -> 투사체의 움직임
    protected abstract void Move();
    public abstract void Fire(Transform caster, Vector3 pos);
    
    private void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody2D>();
        direction = Vector3.right;
        //skillData = new SkillData();
    }

    private void Update()
    {
        Move();
    }

    //카메라에 안잡힐 때
    private void OnBecameInvisible()
    {
        Invoke("ReleaseProjectile", RELEASE_TIME);
    }

    //카메라에 다시 잡히면 Release Cancel
    private void OnBecameVisible()
    {
        CancelInvoke("ReleaseProjectile");
    }

    private void ReleaseProjectile()
    {
        ProjectilePoolManager.Instance.DeSpawnProjectile(this, skillData.projectileType);
    }

    //풀에서 꺼내 쓸 때 스킬 정보를 업데이트하는 함수
    //스킬 레벨업시 데이터 변동을 고려하기 위함
    //Init 역할도 함
    public void SkillDataUpdate(SkillData skillData)
    {
        this.skillData = skillData;
    }
}
