
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

    private Collider2D monsterCollider;
    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;
    private WaitForSeconds tick = new WaitForSeconds(0.05f);

    private bool isPlayer;
    private bool isHit;
    private BehaviorTreeManager btManager;

    private float posWeight;
    private WaitForSeconds delay;

    private SpineManager spineManager;
    private SoundRequester soundRequester;
    private SoundSituation.SOUNDSITUATION situation;

    public bool isAttack { get; private set; }
    public Transform target { get; private set; }
    public Vector2 lookDirection { get; private set; } //바라보는 방향

    private void OnEnable()
    {
        monsterCollider = GetComponent<Collider2D>();
        monsterCollider.enabled = true;
        spineManager = GetComponent<SpineManager>();
        soundRequester = GetComponentInChildren<SoundRequester>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        monsterDirection = Vector2.zero;
        lookDirection = Vector2.right;
        situation = SoundSituation.SOUNDSITUATION.IDLE;
        MonsterSetting(monsterId.ToString());
        currentHp = monsterData.hp;
        delay = new WaitForSeconds(1.0f / monsterData.atkSpeed);
        posWeight = monsterCollider.offset.y;
        isAttack = false;
        isHit = false;
    }

    private void Start()
    {
        //StartCoroutine(Move());
        btManager = new BehaviorTreeManager(SetAI(monsterData.attackType));
        spineManager.SetAnimation("Idle", true);
    }

    private void Update()
    {
        btManager.Active();
    }

    #region AI
    private Node SetAI(AttackTypeConstant attackType)
    {
        switch (attackType)
        {
            case AttackTypeConstant.Bold:
                return BoldAI();
            case AttackTypeConstant.Shy:
                return ShyAI();
            default:
                return null;
        }
    }

    private Node BoldAI()
    {
        return new SelectorNode
                (new List<Node>()
                {
                    new SequenceNode
                    (new List<Node>()
                    {
                        new ActionNode(IsAttack),
                        new ActionNode(IsAttackable),
                        new ActionNode(Attack)
                    }),
                    new SequenceNode
                    (new List<Node>()
                    {
                        new ActionNode(IsVisible),
                        new ActionNode(Run)
                    }),
                    new ActionNode(Idle)
                });
    }

    private Node ShyAI()
    {
        return new SelectorNode
                (new List<Node>()
                {
                    new SequenceNode
                    (new List<Node>()
                    {
                        new ActionNode(IsHit),
                        new SelectorNode
                        (new List<Node>()
                        {
                            new SequenceNode
                            (new List<Node>()
                            {
                                new ActionNode(IsAttack),
                                new ActionNode(IsAttackable),
                                new ActionNode(Attack)
                            }),
                            new ActionNode(Run)
                        })
                    }),
                    new ActionNode(Idle),
                });
    }
    #endregion

    #region Logic
    private NodeConstant IsAttack()
    {
        DebugManager.Instance.PrintDebug("[BTtest]: IsAttack");
        return spineManager.GetAnimationName().Equals("Attack") ? NodeConstant.RUNNING : NodeConstant.SUCCESS;
    }

    private NodeConstant IsAttackable()
    {
        Vector2 diff = target.position - transform.position;
        float distance = diff.magnitude;
        DebugManager.Instance.PrintDebug("[BTtest]: IsAttackable > " + distance);
        if (distance < monsterData.atkDistance && ((Mathf.Abs(diff.y) <= Mathf.Abs(posWeight))))
        {
            DebugManager.Instance.PrintDebug("[테스트]");
            return NodeConstant.SUCCESS;
        }
        return NodeConstant.FAILURE;
    }

    private NodeConstant Attack()
    {
        DebugManager.Instance.PrintDebug("[BTtest]: Attack");
        monsterRigidbody.velocity = Vector2.zero;
        if (!isAttack)
        {
            spineManager.SetAnimation("Attack", false);
            spineManager.AddAnimation("Idle", true);
            StartCoroutine("AttackDelay");
        }
        return NodeConstant.SUCCESS;
    }

    private NodeConstant IsVisible()
    {
        DebugManager.Instance.PrintDebug("[BTtest]: IsVisible");
        return (target.position - transform.position).magnitude <= monsterData.viewDistance ? NodeConstant.SUCCESS : NodeConstant.FAILURE;
    }

    private NodeConstant Run()
    {
        if (isAttack)
        {
            isAttack = false;
            StopCoroutine("AttackDelay");
        }

        Vector2 diff = target.position - transform.position;
        float distance = diff.magnitude;
        DebugManager.Instance.PrintDebug("[BTtest]: Run - " + distance);

        if (distance <= monsterData.atkDistance && ((Mathf.Abs(diff.y) <= Mathf.Abs(posWeight))))
        {
            return NodeConstant.SUCCESS;
        }

        spineManager.SetAnimation("Run", true, 0, monsterData.moveSpeed);
        monsterDirection = diff.normalized;
        spineManager.SetDirection(transform, monsterDirection);
        monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;
        return NodeConstant.RUNNING;
    }

    private NodeConstant Idle()
    {
        if (isAttack)
        {
            isAttack = false;
            StopCoroutine("AttackDelay");
        }

        DebugManager.Instance.PrintDebug("[BTtest]: Idle");
        isAttack = false;
        monsterRigidbody.velocity = Vector2.zero;
        spineManager.SetAnimation("Idle", true);
        return NodeConstant.SUCCESS;
    }

    private NodeConstant IsHit()
    {
        DebugManager.Instance.PrintDebug("[BTtest]: IsHit");
        return isHit ? NodeConstant.SUCCESS : NodeConstant.FAILURE;
    }

    private IEnumerator AttackDelay()
    {
        //yield return delay;
        isAttack = true;
        yield return delay;
        isAttack = false;
    }
    #endregion

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
                DebugManager.Instance.PrintDebug("[DamageTest]");
                currentHp -= projectile.skillData.damage;
                isHit = true;
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