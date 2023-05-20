
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
    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;

    private bool isMovable;
    private bool isAttackable;
    private bool isPlayer;
    [SerializeField] private float currentHp;

    private SpineAnimatorManager spineAnimatorManager;
    private SoundRequester soundRequester;
    private SoundSituation.SOUNDSITUATION situation;

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
        isMovable = false;
        isAttackable = false;
        MonsterSetting(monsterId.ToString());
        currentHp = monsterData.hp;
    }

    private void Update()
    {
        PlayAnimations();
        Move();
    }

    private void Move()
    {
        try
        {
            Vector2 diff = target.position - transform.position;
            float distance = diff.sqrMagnitude;

            if (distance <= monsterData.viewDistance)
            {
                spineAnimatorManager.SetDirection(transform, monsterDirection);
                if (isAttackable = distance <= monsterData.atkDistance)
                {
                    monsterRigidbody.velocity = Vector2.zero;

                }
                else
                {
                    monsterDirection = diff.normalized;
                    monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;

                }
                isMovable = !isAttackable;
            }
            else
            {

                isAttackable = false;
                isMovable = false;
            }
        }
        catch
        {
            DebugManager.Instance.PrintDebug("[ERROR]: 타겟이 없습니다 ");
        }
        
    }

    private void PlayAnimations()
    {
        spineAnimatorManager.SetSpineSpeed(monsterData.moveSpeed);
        spineAnimatorManager.PlayAnimation("isAttackable", isAttackable);
        spineAnimatorManager.PlayAnimation("isMovable", isMovable);
    }

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