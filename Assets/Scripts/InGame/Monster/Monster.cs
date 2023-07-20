
using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [field: SerializeField] public int monsterId { get; private set; }
    [field: SerializeField] public MonsterData monsterData { get; private set; }

    private CapsuleCollider2D monsterCollider2;
    private CapsuleCollider2D monsterCollider;
    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;

    private bool isSlow;
    private bool spineSwitch;
    private bool isPlayer;
    private int _spawnDictID;
    private BehaviorTreeManager btManager;

    private float weightX;
    private float weightY;
    private WaitForSeconds delay;
    private WaitForSeconds tick;

    private HpBar hpBar;
    private WaitForSeconds hpBarVisibleTime;

    private MonsterCollider attackCollider;
    private SpineManager spineManager;
    private SoundRequester soundRequester;
    private SoundSituation.SOUNDSITUATION situation;

    public bool isHit { get; set; }
    public bool isAttack { get; private set; }
    public Transform target { get; private set; }
    public Vector2 lookDirection { get; private set; } //바라보는 방향
    public int spawnDictId { get {return _spawnDictID;}  set { _spawnDictID= value;} }


    private void Awake()
    {
        attackCollider = GetComponentInChildren<MonsterCollider>();
        monsterCollider2 = transform.Find("Collision").GetComponent<CapsuleCollider2D>();
        monsterCollider = GetComponent<CapsuleCollider2D>();
        spineManager = GetComponent<SpineManager>();
        soundRequester = GetComponent<SoundRequester>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        tick = new WaitForSeconds(0.4f);
        hpBarVisibleTime = new WaitForSeconds(Convert.ToInt32(CSVReader.Read("BattleConfig", "HpBarVisibleTime", "ConfigValue")) / 1000.0f);
    }

    private void Start()
    {
        btManager = new BehaviorTreeManager(SetAI(monsterData.attackType));
        spineManager.SetAnimation("Idle", true);
        attackCollider.SetAttackDistance(monsterData.atkDistance);

        if (monsterData.atkDistance <= 1.0f)    //근거리
        {
            weightX = attackCollider.attackCollider.size.x * 0.3f;
            weightY = monsterCollider.size.y * 0.3f;
        }
        else    //원거리
        {
            weightX = monsterData.atkDistance;
            weightY = monsterData.atkDistance;
        }
    }

    private void FixedUpdate()
    {
        if (spineSwitch)
        {
            btManager.Active();
        }

        if (hpBar != null)
        {
            hpBar.HpBarSetting(transform.position, monsterData.currentHp, monsterData.hp);
        }
        else
        {
            
            hpBar = (HpBar)UIPoolManager.Instance.SpawnUI("HpBar", PlayerUI.Instance.transform.Find("HpBarUI"), transform.position);
        }

        monsterRigidbody.velocity = Vector2.zero;
    }

    public void SpawnSet()
    {
        Physics2D.IgnoreCollision(monsterCollider, monsterCollider2);
        
        monsterCollider.enabled = true;
        monsterCollider.enabled = true;
        monsterDirection = Vector2.zero;
        lookDirection = Vector2.right;
        situation = SoundSituation.SOUNDSITUATION.IDLE;
        MonsterDataSetting(monsterId.ToString());
        delay = new WaitForSeconds(1.0f / monsterData.atkSpeed);

        isAttack = false;
        isHit = false;
        spineSwitch = true;
        isSlow = false;
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

    #region AI_Logic
    private NodeConstant IsAttack()
    {
        return spineManager.GetAnimationName().Equals("Attack") ? NodeConstant.RUNNING : NodeConstant.SUCCESS;
    }

    private NodeConstant IsAttackable()
    {
        Vector2 diff = target.position - transform.position;
        //float distance = diff.magnitude;
        //if (distance <= monsterData.atkDistance && ((Mathf.Abs(diff.y) <= Mathf.Abs(weightY))))
        //{
        //    return NodeConstant.SUCCESS;
        //}
        if (((Mathf.Abs(diff.x) <= Mathf.Abs(weightX))) && ((Mathf.Abs(diff.y) <= Mathf.Abs(weightY))))
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

        if (distance <= monsterData.atkDistance)
        {
            return NodeConstant.SUCCESS;
        }

        spineManager.SetAnimation("Run", true, 0, monsterData.moveSpeed);
        monsterDirection = diff.normalized;
        spineManager.SetDirection(transform, monsterDirection);
        monsterRigidbody.MovePosition(monsterRigidbody.position + (monsterDirection * monsterData.moveSpeed * Time.fixedDeltaTime));
        //monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;
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

    public void MonsterDataSetting(string monsterId)
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

    public void Hit(float totalDamage)
    {
        StopCoroutine(HpBarControl());
        StartCoroutine(HpBarControl());

        isHit = true;
        monsterData.SetCurrentHp(monsterData.currentHp - (int)totalDamage);
        if (monsterData.currentHp <= 0)
        {
            Die();
        }
    }

    private IEnumerator HpBarControl()
    {
        hpBar.HpBarSwitch(true);
        yield return hpBarVisibleTime;
        hpBar.HpBarSwitch(false);
    }

    public void Die()
    {
        //soundRequester.ChangeSituation(SoundSituation.SOUNDSITUATION.DIE);
        monsterCollider.enabled = false;
        monsterCollider2.enabled = false;
        StartCoroutine(DieAnimation());
        DropItem();
        UIPoolManager.Instance.DeSpawnUI("HpBar", hpBar);
        hpBar = null;
    }

    private IEnumerator DieAnimation()
    {
        spineSwitch = false;
        try
        {
            spineManager.SetAnimation("Death", false);
        }
        catch
        {
            DebugManager.Instance.PrintDebug("[ERROR]: 스파인에 죽는 애니메이션이 없는 몬스터입니다");
        }
        yield return new WaitForSeconds(1.0f);
        MonsterSpawner.Instance.DeSpawnMonster(this);
    }

    #region SKILL_EFFECT
    public void SkillEffectActivation(SKILL_EFFECT effect, float param)
    {
        isHit = true;
        if (gameObject.activeInHierarchy)
        {
            switch (effect)
            {
                case SKILL_EFFECT.STUN:
                    StartCoroutine(Stun(param));
                    break;
                case SKILL_EFFECT.SLOW:
                    StartCoroutine(Slow(param));
                    break;
                case SKILL_EFFECT.KNOCKBACK:
                    StartCoroutine(KnockBack(param));
                    break;
                case SKILL_EFFECT.EXECUTE:
                    Execute(param);
                    break;
                case SKILL_EFFECT.RESTRAINT:
                    StartCoroutine(Restraint(param));
                    break;
                case SKILL_EFFECT.PULL:
                    StartCoroutine(Pull(param));
                    break;
                default:
                    DebugManager.Instance.PrintDebug("[ERROR]: 없는 스킬 효과입니다");
                    break;
            }
        }
    }

    private IEnumerator Stun(float n)
    {
        if (spineSwitch)
        {
            spineSwitch = false;
            float originSpeed = monsterData.moveSpeed;
            monsterData.SetMoveSpeed(0.0f);
            spineManager.SetAnimation("Idle", true);
            yield return new WaitForSeconds(n);
            monsterData.SetMoveSpeed(originSpeed);
            spineSwitch = true;
        }
    }

    private IEnumerator Slow(float n)
    {
        if (!isSlow)
        {
            isSlow = true;
            float originSpeed = monsterData.moveSpeed;
            monsterData.SetMoveSpeed(originSpeed * n * 0.01f);
            yield return new WaitForSeconds(1.0f);
            monsterData.SetMoveSpeed(originSpeed);
            isSlow = false;
        }
    }

    private IEnumerator KnockBack(float n)
    {
        if (spineSwitch)
        {
            spineSwitch = false;
            Vector2 diff = transform.position - target.position;
            monsterRigidbody.AddRelativeForce(diff.normalized * n * 0.0002f, ForceMode2D.Impulse);
            yield return tick;
            spineSwitch = true;
        }
    }

    private void Execute(float n)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < n)
        {
            Die();
        }
    }

    private IEnumerator Restraint(float n)
    {
        float originSpeed = monsterData.moveSpeed;
        monsterData.SetMoveSpeed(0.0f);
        yield return new WaitForSeconds(n);
        monsterData.SetMoveSpeed(originSpeed);
    }

    private IEnumerator Pull(float n)
    {
        if (spineSwitch)
        {
            spineSwitch = false;
            Vector2 diff = target.position - transform.position;
            monsterRigidbody.AddRelativeForce(diff.normalized * n * 0.0002f, ForceMode2D.Impulse);
            yield return tick;
            spineSwitch = true;
        }
    }

    #endregion
}