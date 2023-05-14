using BFM;
using SKILLCONSTANT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Structure
public struct SkillInfo
{
    public Skill skill;
    public IEnumerator activation;

    public SkillInfo(Skill skill, IEnumerator activation)
    {
        this.skill = skill;
        this.activation = activation;
    }
}
#endregion

public class SkillManager : SingletonBehaviour<SkillManager>
{
    [SerializeField] private int skillNum = 10801;

    private Dictionary<string, Dictionary<string, object>> skillTable;
    private Dictionary<int, ObjectPool<Projectile>> skillPools;

    private RaycastHit2D[] targets;

    public Dictionary<int, SkillInfo> skillList { get; private set; } = new Dictionary<int, SkillInfo>();

    protected override void Awake()
    {
        skillTable = CSVReader.Read("SkillTable");
        skillPools = new Dictionary<int, ObjectPool<Projectile>>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SkillAdd(skillNum, GameManager.Instance.player.transform);
        }

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    SkillLevelUp(skillList[12001]);
        //}
    }

    public Projectile SpawnProjectile(SkillData skillData)
    {
        return SpawnProjectile(skillData, transform);
    }

    public Projectile SpawnProjectile(SkillData skillData, Transform shooter)
    {
        int poolId = skillData.skillId / 100;
        Dictionary<string, object> data = skillTable[skillData.skillId.ToString()];
        if (!skillPools.ContainsKey(poolId))
        {
            string prefabPath = data["SkillPrefabPath"].ToString();
            skillPools.Add(poolId, new ObjectPool<Projectile>(ResourcesManager.Load<Projectile>(prefabPath), shooter));
        }
        Projectile projectile = skillPools[poolId].GetObject();
        projectile.gameObject.layer = (int)LayerConstant.SKILL;
        projectile.transform.localPosition = Vector2.zero;
        projectile.SetProjectile(skillData);
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void DeSpawnProjectile(Projectile projectile)
    {
        skillPools[projectile.skillData.skillId / 100].ReleaseObject(projectile);
        projectile.transform.parent = transform;
    }

    public void SkillAdd(int skillId, Transform shooter)
    {
        foreach (int id in skillList.Keys)
        {
            if (id / 100 == skillId / 100)
            {
                DebugManager.Instance.PrintDebug("[ERROR]: 이미 존재하는 스킬입니다");
                return;
            }
        }

        Skill skill;
        switch(skillId / 100)
        {
            case 101:
                skill = new Juhon(skillId, shooter);
                break;
            case 102:
                skill = new Bujung(skillId, shooter);
                break;
            case 103:
                skill = new GangSin(skillId, shooter);
                break;
            case 104:
                skill = new GodBless(skillId, shooter);
                break;
            case 105:
                skill = new Possession(skillId, shooter);
                break;
            case 108:
                skill = new JuHyung(skillId, shooter);
                break;
            case 120:
                skill = new Horin(skillId, shooter);
                break;
            default:
                skill = null;
                break;
        }

        skill.Init();
        IEnumerator activation = skill.Activation();
        StartCoroutine(activation);
        SkillInfo skillInfo = new SkillInfo(skill, activation);
        skillList.Add(skillId, skillInfo);
    }

    public void SkillLevelUp(SkillInfo skillInfo)
    {
        StopCoroutine(skillInfo.activation);
        skillInfo.skill.SkillLevelUp();
        IEnumerator activation = skillInfo.skill.Activation();
        StartCoroutine(activation);
    }

    public Transform MeleeTarget(Transform shooter, float attackDistance)
    {
        Vector2 shooterPos = shooter.position;
        targets = Physics2D.CircleCastAll(shooterPos, attackDistance, Vector2.zero, 0, 1 << (int)LayerConstant.MONSTER);
        Transform resultTarget = null;
        float distance = float.MaxValue;
        foreach (RaycastHit2D target in targets)
        {
            if (target.transform == shooter)
            {
                continue;
            }

            float diff = (shooterPos - (Vector2)target.transform.position).sqrMagnitude;
            if (diff < distance)
            {
                distance = diff;
                resultTarget = target.transform;
            }
        }
        return resultTarget;
    }

    public Transform BossTarget()
    {
        Transform resultTarget = null;
        foreach (Monster monster in MonsterSpawner.Instance.monsters)
        {
            if (monster.monsterId / 100 == 4)
            {
                resultTarget = monster.transform;
            }
        }

        return resultTarget;
    }
}
