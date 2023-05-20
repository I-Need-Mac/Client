
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Monster : MonoBehaviour
{
    [field: SerializeField] public int monsterId { get; private set; }
    [field: SerializeField] public MonsterData monsterData;
    [SerializeField] private float currentHp;

    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;

    //private bool isMovable;
    //private bool isAttackable;
    private bool isPlayer;
    private WaitForSeconds duration;

    private SpineAnimatorManager spineAnimatorManager;
    private SoundRequester soundRequester;
    private SoundSituation.SOUNDSITUATION situation;

    private Transform body;

    public Transform target { get; private set; }
   
    public Vector2 lookDirection { get; private set; } //바라보는 방향

    private void OnEnable()
    {
        spineAnimatorManager = GetComponent<SpineAnimatorManager>();
        soundRequester = GetComponentInChildren<SoundRequester>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        monsterDirection = Vector2.zero;
        lookDirection = Vector2.right;
        situation = SoundSituation.SOUNDSITUATION.IDLE;
        body = transform.Find("Character");
        //isMovable = false;
        //isAttackable = false;
        MonsterSetting(monsterId.ToString());
        currentHp = monsterData.hp;
        duration = new WaitForSeconds((1 / monsterData.atkSpeed) * 1.5f);
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    private void Update()
    {
        //PlayAnimations();
        //Move();
    }

    //private void Move()
    //{
    //    try
    //    {
    //        Vector2 diff = target.position - transform.position;
    //        float distance = diff.sqrMagnitude;

    //        if (distance <= monsterData.viewDistance)
    //        {
    //            spineAnimatorManager.SetDirection(transform, monsterDirection);
    //            if (distance <= monsterData.atkDistance)
    //            {
    //                StartCoroutine(Attack());
    //                DebugManager.Instance.PrintDebug("[MOBTEST]: ");
    //            }
    //            else
    //            {
    //                monsterDirection = diff.normalized;
    //                monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;

    //            }
    //            isMovable = !isAttackable;
    //        }
    //        else
    //        {
    //            monsterRigidbody.velocity = Vector2.zero;
    //            isAttackable = false;
    //            isMovable = false;
    //        }
    //    }
    //    catch
    //    {
    //        DebugManager.Instance.PrintDebug("[ERROR]: 타겟이 없습니다 ");
    //    }
    //}

    private IEnumerator Move()
    {
        spineAnimatorManager.PlayAnimation("isMovable", false);
        spineAnimatorManager.PlayAnimation("isAttackable", false);
        monsterRigidbody.velocity = Vector2.zero;

        while (true)
        {
            yield return null;

            if (target == null)
            {
                continue;
            }

            Vector2 diff = target.position - transform.position;
            float distance = diff.magnitude;
            DebugManager.Instance.PrintDebug("[MOBTEST]: " + distance);
            if (distance <= monsterData.viewDistance)
            {
                monsterDirection = diff.normalized;
                if (distance <= monsterData.atkDistance)
                {
                    DebugManager.Instance.PrintDebug("[MOBTEST]: attack");
                    spineAnimatorManager.SetDirection(transform, monsterDirection, body);
                    monsterRigidbody.velocity = Vector2.zero;
                    spineAnimatorManager.SetSpineSpeed(monsterData.atkSpeed);
                    //spineAnimatorManager.PlayAnimation("attack");
                    spineAnimatorManager.PlayAnimation("isMovable", false);
                    spineAnimatorManager.PlayAnimation("isAttackable", true);
                    yield return duration;
                }
                else
                {
                    DebugManager.Instance.PrintDebug("[MOBTEST]: move");
                    spineAnimatorManager.SetDirection(transform, monsterDirection, body);
                    monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;
                    spineAnimatorManager.SetSpineSpeed(monsterData.moveSpeed);
                    spineAnimatorManager.PlayAnimation("isMovable", true);
                    spineAnimatorManager.PlayAnimation("isAttackable", false);
                }
            }
            else
            {
                DebugManager.Instance.PrintDebug("[MOBTEST]: idle");
                monsterRigidbody.velocity = Vector2.zero;
                spineAnimatorManager.PlayAnimation("isMovable", false);
                spineAnimatorManager.PlayAnimation("isAttackable", false);
            }
        }
    }

    //private void PlayAnimations(float animationSpeed)
    //{
    //    spineAnimatorManager.SetDirection(transform, monsterDirection);
    //    spineAnimatorManager.SetSpineSpeed(animationSpeed);
    //    spineAnimatorManager.PlayAnimation("isAttackable", isAttackable);
    //    spineAnimatorManager.PlayAnimation("isMovable", isMovable);
    //}

    public void SetTarget(Transform target, bool isPlayer)
    {
        this.target = target;
        this.isPlayer = isPlayer;
    }

    public void MonsterSetting(string monsterId)
    {
        Dictionary<string, Dictionary<string, object>> monsterTable = CSVReader.Read("MonsterTable");
        if (monsterTable.ContainsKey(monsterId))
        {
            Dictionary<string, object> table = monsterTable[monsterId];
            monsterData.SetMonsterName(Convert.ToString(table["MonsterName"]));
            monsterData.SetHp(Convert.ToInt32(table["HP"]));
            monsterData.SetSizeMultiple(float.Parse(Convert.ToString(table["SizeMultiple"])));
            monsterData.SetAttack(Convert.ToInt32(table["Attack"]));
            monsterData.SetMoveSpeed(float.Parse(Convert.ToString(table["MoveSpeed"])));
            monsterData.SetAtkSpeed(float.Parse(Convert.ToString(table["AtkSpeed"])));
            monsterData.SetViewDistance(float.Parse(Convert.ToString(table["ViewDistance"])));
            monsterData.SetAtkDistance(float.Parse(Convert.ToString(table["AtkDistance"])));
            monsterData.SetSkillID(Convert.ToInt32(table["SkillID"]));
            monsterData.SetGroupSource(Convert.ToString(table["GroupSource"]));
            monsterData.SetGroupSourceRate(Convert.ToInt32(table["GroupSourceRate"]));
            monsterData.SetMonsterPrefabPath(Convert.ToString(table["MonsterPrefabPath"]));
            monsterData.SetAttackType((AttackTypeConstant)Enum.Parse(typeof(AttackTypeConstant), Convert.ToString(table["AttackType"])));
        }
    }

    private void DropItem()
    {
        if (UnityEngine.Random.Range(0, 10001) <= monsterData.groupSourceRate)
        {
            ItemManager.Instance.DropItems(monsterData.groupSource, transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile))
        {
            if (projectile.isHit)
            {
                currentHp -= projectile.skillData.damage;
                if (currentHp <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        //soundRequester.ChangeSituation(SoundSituation.SOUNDSITUATION.DIE);
        DropItem();
        MonsterSpawner.Instance.DeSpawnMonster(this);
    }

}