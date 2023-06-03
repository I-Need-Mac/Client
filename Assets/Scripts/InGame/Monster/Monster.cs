
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [field: SerializeField] public int monsterId { get; private set; }
    [field: SerializeField] public MonsterData monsterData { get; private set; }

    private CapsuleCollider2D monsterCollider;
    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;

    private bool spineSwitch;
    private bool isPlayer;
    private bool isHit;
    private BehaviorTreeManager btManager;

    private float weightY;
    private WaitForSeconds delay;
    private WaitForSeconds tick;

    private MonsterCollider attackCollider;
    private SpineManager spineManager;
    private SoundRequester soundRequester;
    private SoundSituation.SOUNDSITUATION situation;

    public bool isAttack { get; private set; }
    public Transform target { get; private set; }
    public Vector2 lookDirection { get; private set; } //바라보는 방향

    private void Awake()
    {
        attackCollider = GetComponentInChildren<MonsterCollider>();
        monsterCollider = GetComponent<CapsuleCollider2D>();
        spineManager = GetComponent<SpineManager>();
        soundRequester = GetComponent<SoundRequester>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        tick = new WaitForSeconds(0.4f);
    }

    private void OnEnable()
    {
        monsterCollider.enabled = true;
        monsterDirection = Vector2.zero;
        lookDirection = Vector2.right;
        situation = SoundSituation.SOUNDSITUATION.IDLE;
        MonsterSetting(monsterId.ToString());
        delay = new WaitForSeconds(1.0f / monsterData.atkSpeed);
        weightY = monsterCollider.size.y;
        isAttack = false;
        isHit = false;
        spineSwitch = true;
    }

    private void Start()
    {
        //StartCoroutine(Move());
        btManager = new BehaviorTreeManager(SetAI(monsterData.attackType));
        spineManager.SetAnimation("Idle", true);
        attackCollider.SetAttackDistance(monsterData.atkDistance);
    }

    private void FixedUpdate()
    {
        if (spineSwitch)
        {
            btManager.Active();
        }
        monsterRigidbody.velocity = Vector3.zero;
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
        return spineManager.GetAnimationName().Equals("Attack") ? NodeConstant.RUNNING : NodeConstant.SUCCESS;
    }

    private NodeConstant IsAttackable()
    {
        Vector2 diff = target.position - transform.position;
        float distance = diff.magnitude;
        if (distance < monsterData.atkDistance && ((Mathf.Abs(diff.y) <= Mathf.Abs(weightY))))
        {
            return NodeConstant.SUCCESS;
        }
        return NodeConstant.FAILURE;
    }

    private NodeConstant Attack()
    {
        monsterRigidbody.velocity = Vector3.zero;
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

        if (distance <= monsterData.atkDistance && ((Mathf.Abs(diff.y) <= Mathf.Abs(weightY))))
        {
            return NodeConstant.SUCCESS;
        }

        spineManager.SetAnimation("Run", true, 0, monsterData.moveSpeed);
        monsterDirection = diff.normalized;
        spineManager.SetDirection(transform, monsterDirection);
        //monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;
        monsterRigidbody.MovePosition(monsterRigidbody.position + (monsterDirection * monsterData.moveSpeed * Time.fixedDeltaTime));
        monsterRigidbody.velocity = Vector3.zero;
        return NodeConstant.RUNNING;
    }

    private NodeConstant Idle()
    {
        if (isAttack)
        {
            isAttack = false;
            StopCoroutine("AttackDelay");
        }

        isAttack = false;
        monsterRigidbody.velocity = Vector3.zero;
        spineManager.SetAnimation("Idle", true);
        return NodeConstant.SUCCESS;
    }

    private NodeConstant IsHit()
    {
        return isHit ? NodeConstant.SUCCESS : NodeConstant.FAILURE;
    }

    private IEnumerator AttackDelay()
    {
        yield return tick;
        isAttack = true;
        attackCollider.AttackColliderSwitch(true);
        //yield return tick;
        yield return delay;
        attackCollider.AttackColliderSwitch(false);
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
            monsterData.SetCurrentHp(monsterData.hp);
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
                monsterData.SetCurrentHp(monsterData.currentHp - (int)projectile.totalDamage);
                isHit = true;
                if (monsterData.currentHp <= 0)
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

    #region SKILL_EFFECT
    public IEnumerator Stun(float n)
    {
        spineSwitch = false;
        float originSpeed = monsterData.moveSpeed;
        monsterData.SetMoveSpeed(0.0f);
        spineManager.SetAnimation("Idle", true);
        yield return new WaitForSeconds(n);
        monsterData.SetMoveSpeed(originSpeed);
        spineSwitch = true;
    }

    public void Slow(float n)
    {
        monsterData.SetMoveSpeed(monsterData.moveSpeed * n * 0.01f);
    }

    public void NuckBack(float n)
    {
        Vector2 vec = transform.position - target.position;
        monsterRigidbody.AddForce(vec.normalized * n * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    public void Execute(float n)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < n)
        {
            Die();
        }
    }

    public IEnumerator Restraint(float n)
    {
        float originSpeed = monsterData.moveSpeed;
        monsterData.SetMoveSpeed(0.0f);
        yield return new WaitForSeconds(n);
        monsterData.SetMoveSpeed(originSpeed);
    }

    public void Pull(float n)
    {
        Vector2 vec = target.position - transform.position;
        monsterRigidbody.AddForce(vec.normalized * n * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    #endregion
}