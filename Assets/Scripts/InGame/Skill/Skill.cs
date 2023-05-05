using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    private Dictionary<string, Dictionary<string, object>> skillTable;

    protected Transform shooter;
    protected List<Projectile> projectiles;
    protected SkillData skillData;
    protected WaitForSeconds intervalTime;

    public abstract void Init();
    public abstract IEnumerator Activation();

    public Skill(int skillId, Transform shooter)
    {
        skillTable = CSVReader.Read("SkillTable");
        this.projectiles = new List<Projectile>();
        this.skillData = new SkillData();
        this.shooter = shooter;
        SetSkillData(skillId);
    }

    public void SkillLevelUp()
    {
        foreach (Projectile projectile in projectiles)
        {
            SkillManager.Instance.DeSpawnProjectile(projectile);
        }
        projectiles.Clear();
        SetSkillData(skillData.skillId + 1);
        Init();
    }

    public void SetSkillData(int skillId)
    {
        Dictionary<string, object> data = skillTable[skillId.ToString()];

        skillData.SetSkillId(skillId);
        skillData.SetCoolTime(Convert.ToInt32(data["Cooltime"]));
        skillData.SetAttackDistance(Convert.ToInt32(data["AttackDistance"]));
        skillData.SetDamage(Convert.ToInt32(data["Damage"]));
        skillData.SetSkillEffectParam(Convert.ToInt32(data["SkillEffectParam"]));
        skillData.SetSkillCut(Convert.ToBoolean(Convert.ToString(data["Skill_Cut"]).ToLower()));
        skillData.SetIsEffect(Convert.ToBoolean(Convert.ToString(data["IsEffect"]).ToLower()));
        skillData.SetIsUltimate(Convert.ToBoolean(Convert.ToString(data["IsUltimate"]).ToLower()));
        skillData.SetName(Convert.ToString(data["Name"]));
        skillData.SetDesc(Convert.ToString(data["Desc"]));
        skillData.SetIcon(Convert.ToString(data["Icon"]));
        skillData.SetCutDire(Convert.ToString(data["Cut_dire"]));
        skillData.SetSkillImage(Convert.ToString(data["SkillImage"]));
        skillData.SetSkillEffect((SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), Convert.ToString(data["SkillEffect"]).ToUpper()));
        skillData.SetSkillTarget((SKILL_TARGET)Enum.Parse(typeof(SKILL_TARGET), Convert.ToString(data["SkillTarget"]).ToUpper()));
        //skillData.SetCalcDamageType((CALC_DAMAGE_TYPE)Enum.Parse(typeof(CALC_DAMAGE_TYPE), Convert.ToString(data["SkillCalcType"]).ToUpper()));

        skillData.SetProjectileCount(Convert.ToInt32(data["ProjectileCount"]));
        skillData.SetIntervalTime((float)Convert.ToDouble(data["IntervalTime"]));
        intervalTime = new WaitForSeconds(skillData.intervalTime);
        skillData.SetDuration((float)Convert.ToDouble(data["Duration"]));
        skillData.SetSpeed(Convert.ToInt32(data["Speed"]));
        skillData.SetSplashRange(Convert.ToInt32(data["SplashRange"]));
        skillData.SetProjectileSizeMulti(Convert.ToInt32(data["ProjectileSizeMulti"]));
        skillData.SetIsPenetrate(Convert.ToBoolean(data["IsPenetrate"]));
        //skillData.SetProjectileType((PROJECTILE_TYPE)Enum.Parse(typeof(PROJECTILE_TYPE), Convert.ToString(data["ProjectileType"]).ToUpper()));
        //skillData.SetSkillPrefabPath(data["SkillPrefabPath"].ToString());
    }
}
