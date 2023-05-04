using BFM;
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
    private Dictionary<string, Dictionary<string, object>> skillTable;
    private Dictionary<int, ObjectPool<Projectile>> skillPools;

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
            SkillAdd(12001, GameManager.Instance.player.transform);
        }
    }

    public Projectile SpawnProjectile(SkillData skillData, Transform shooter)
    {
        int poolId = skillData.skillId / 100;
        Dictionary<string, object> data = skillTable[skillData.skillId.ToString()];
        if (!skillPools.ContainsKey(poolId))
        {
            string prefabPath = data["SkillPrefabPath"].ToString();
            skillPools.Add(poolId, new ObjectPool<Projectile>(ResourcesManager.Load<Projectile>(prefabPath), transform));
        }
        Projectile projectile = skillPools[poolId].GetObject();
        projectile.transform.parent = shooter;
        projectile.SetProjectile(skillData.skillId, skillData.damage, skillData.isPenetrate, skillData.skillEffect, skillData.calcDamageType);
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void DeSpawnProjectile(Projectile projectile)
    {
        skillPools[projectile.skillId / 100].ReleaseObject(projectile);
        projectile.transform.parent = transform;
    }

    public void SkillAdd(int skillId, Transform shooter)
    {
        Skill skill;
        switch(skillId / 100)
        {
            case 120:
                skill = new GuardiansFrame(skillId, shooter);
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
}
