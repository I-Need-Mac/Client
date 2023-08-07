using BFM;
using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public static readonly int SKILL_MAX_LEVEL = 8;
    public static readonly int ACTIVE_SKILL_MAX_COUNT = 6;
    public static readonly int PASSIVE_SKILL_MAX_COUNT = 5;
    public static readonly int SKILL_MAX_COUNT = ACTIVE_SKILL_MAX_COUNT + PASSIVE_SKILL_MAX_COUNT;

    [SerializeField] private int skillNum = 10801;

    private Dictionary<string, Dictionary<string, object>> skillTable;
    private Dictionary<int, ObjectPool<Projectile>> skillPools;

    //public Dictionary<int, SkillInfo> skillList { get; private set; } = new Dictionary<int, SkillInfo>();
    public Dictionary<int, Skill> skillList { get; private set; } = new Dictionary<int, Skill>();

    protected override void Awake()
    {
        skillTable = CSVReader.Read("SkillTable");
        skillPools = new Dictionary<int, ObjectPool<Projectile>>();

        foreach (string skillId in skillTable.Keys)
        {
            if (int.TryParse(skillId, out int id))
            {
                id /= 100;
                if (!skillPools.ContainsKey(id))
                {
                    string prefabPath = skillTable[skillId]["SkillPrefabPath"].ToString();
                    skillPools.Add(id, new ObjectPool<Projectile>(ResourcesManager.Load<Projectile>(prefabPath), transform));
                }
            }
        }
    }

    public Projectile SpawnProjectile(ActiveData skillData)
    {
        return SpawnProjectile(skillData, transform);
    }

    public Projectile SpawnProjectile(ActiveData skillData, Transform shooter)
    {
        int poolId = skillData.skillId / 100;
        Projectile projectile = skillPools[poolId].GetObject();
        projectile.transform.parent = shooter;
        projectile.gameObject.layer = (int)LayerConstant.SKILL;
        projectile.transform.localPosition = Vector2.zero;
        projectile.transform.localScale = Vector2.one * skillData.projectileSizeMulti;
        projectile.SetProjectile(skillData);
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void DeSpawnProjectile(Projectile projectile)
    {
        skillPools[projectile.skillData.skillId / 100].ReleaseObject(projectile);
        projectile.transform.parent = transform;
    }

    public void SkillAdd(int skillId, Transform shooter, int skillNum)
    {
        foreach (int id in skillList.Keys)
        {
            if (id / 100 == skillId / 100)
            {
                DebugManager.Instance.PrintDebug("[SkillManager]: Skill Level Up!");
                //skillList[id].skill.SkillLevelUp();
                skillList[id].SkillLevelUp();
                return;
            }
        }
        
        Skill skill;
        switch(skillId / 100)
        {
            case 101:
                skill = new Juhon(skillId, shooter, skillNum);
                break;
            case 102:
                skill = new Bujung(skillId, shooter, skillNum);
                break;
            case 103:
                skill = new GangSin(skillId, shooter, skillNum);
                break;
            case 104:
                skill = new GodBless(skillId, shooter, skillNum);
                break;
            case 105:
                skill = new Possession(skillId, shooter, skillNum);
                break;
            case 106:
                skill = new Irons(skillId, shooter, skillNum);
                break;
            case 107:
                skill = new GwiGi(skillId, shooter, skillNum);
                break;
            case 108:
                skill = new JuHyung(skillId, shooter, skillNum);
                break;
            case 109:
                skill = new MyeongGyae(skillId, shooter, skillNum);
                break;
            case 110:
                skill = new Crepitus(skillId, shooter, skillNum);
                break;
            case 111:
                skill = new GyuGyu(skillId, shooter, skillNum);
                break;
            case 112:
                skill = new Aliento(skillId, shooter, skillNum);
                break;
            case 113:
                skill = new Pok(skillId, shooter, skillNum);
                break;
            case 114:
                skill = new JeRyeung(skillId, shooter, skillNum);
                break;
            case 120:
                skill = new Horin(skillId, shooter, skillNum);
                break;
            case 202:
                skill = new InnPassive(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 203:
                skill = new HyulPok(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 204:
                skill = new DaeHum(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 205:
                skill = new GaSok(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 206:
                skill = new Hyum(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 207:
                skill = new JaeSaeng(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 208:
                skill = new HwakSan(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 209:
                skill = new HwakHo(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 210:
                skill = new JuJuGaSork(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 211:
                skill = new JuJuJyungPok(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 212:
                skill = new GwangHwa(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            case 213:
                skill = new ChangAe(skillId, shooter, skillNum + ACTIVE_SKILL_MAX_COUNT);
                break;
            default:
                DebugManager.Instance.PrintDebug("[ERROR]: 아직 미구현된 스킬입니다 (SkillID: " + skillId + ")");
                return;
        }

        if (skillId / 10000 == 1)
        {
            Dictionary<string, Dictionary<string, object>> skillTable = CSVReader.Read("SkillTable");
            PlayerUI.Instance.skillBoxUi.SkillIconInit(skillTable[skillId.ToString()]["Icon"].ToString(), skillNum);
            PlayerUI.Instance.activeSkillCount++;
        }
        else if (skillId / 10000 == 2)
        {
            Dictionary<string, Dictionary<string, object>> passiveTable = CSVReader.Read("PassiveTable");
            PlayerUI.Instance.skillBoxUi.SkillIconInit(passiveTable[skillId.ToString()]["Icon"].ToString(), skillNum + ACTIVE_SKILL_MAX_COUNT);
            PlayerUI.Instance.passiveSkillCount++;
        }

        //IEnumerator activation = skill.Activation();
        //StartCoroutine(activation);
        //SkillInfo skillInfo = new SkillInfo(skill, activation);
        //skillList.Add(skillId, skillInfo);
        StartCoroutine(skill.Activation());
        skillList.Add(skillId, skill);
    }

    public void SkillLevelUp(SkillInfo skillInfo)
    {
        //StopCoroutine(skillInfo.activation);
        skillInfo.skill.SkillLevelUp();
        //IEnumerator activation = skillInfo.skill.Activation();
        //StartCoroutine(activation);
        //skillInfo.activation = activation;
    }

    public void CoroutineStarter(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    //private void SkillDataUpdate()
    //{
    //    foreach (SkillInfo info in skillList.Values)
    //    {
    //        info.skill.SkillDataUpdate();
    //    }
    //}

    //public void SkillDataUpdate(float coolTime, int count, float damage, float speed, float splashRange, float size)
    //{
    //    foreach (SkillInfo info in skillList.Values)
    //    {
    //        info.skill.SkillDataUpdate(coolTime, count, damage, speed, splashRange, size);
    //    }
    //}

}
