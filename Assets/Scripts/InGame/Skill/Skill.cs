using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Skill
{
    private Dictionary<string, Dictionary<string, object>> skillTable;

    protected Transform shooter;
    protected List<Projectile> projectiles;
    protected SkillData skillData;
    protected Vector2 originSize;

    protected WaitForSeconds coolTime;
    protected WaitForSeconds intervalTime;
    protected WaitForSeconds duration;

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
            if (projectile.gameObject.activeInHierarchy)
            {
                SkillManager.Instance.DeSpawnProjectile(projectile);
            }
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
        coolTime = new WaitForSeconds(skillData.coolTime / 10000.0f);
        skillData.SetAttackDistance(Convert.ToInt32(data["AttackDistance"]));
        skillData.SetDamage(Convert.ToInt32(data["Damage"]));

        List<string> list = data["SkillEffectParam"] as List<string>;
        if (list == null)
        {
            list = new List<string>
            {
                data["SkillEffectParam"].ToString(),
            };
            skillData.SetSkillEffectParam(list);
        }
        else
        {
            skillData.SetSkillEffectParam(list);
        }
        
        skillData.SetSkillCut(Convert.ToBoolean(Convert.ToString(data["Skill_Cut"]).ToLower()));
        skillData.SetIsEffect(Convert.ToBoolean(Convert.ToString(data["IsEffect"]).ToLower()));
        skillData.SetIsUltimate(Convert.ToBoolean(Convert.ToString(data["IsUltimate"]).ToLower()));
        skillData.SetName(Convert.ToString(data["Name"]));
        skillData.SetDesc(Convert.ToString(data["Desc"]));
        skillData.SetIcon(Convert.ToString(data["Icon"]));
        skillData.SetCutDire(Convert.ToString(data["Cut_dire"]));
        skillData.SetSkillImage(Convert.ToString(data["SkillImage"]));
        try
        {
            List<SKILL_EFFECT> list2 = new List<SKILL_EFFECT>();
            foreach (string str in (data["SkillEffect"] as List<string>))
            {
                list2.Add((SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), str, true));
            }
            skillData.SetSkillEffect(list2);
        }
        catch
        {
            List<SKILL_EFFECT> list2 = new List<SKILL_EFFECT>()
            {
                (SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), Convert.ToString(data["SkillEffect"]), true),
            };
            skillData.SetSkillEffect(list2);
        }
        skillData.SetSkillTarget((SKILL_TARGET)Enum.Parse(typeof(SKILL_TARGET), Convert.ToString(data["SkillTarget"]).ToUpper()));
        skillData.SetProjectileCount(Convert.ToInt32(data["ProjectileCount"]));
        skillData.SetIntervalTime((float)Convert.ToDouble(data["IntervalTime"]));
        intervalTime = new WaitForSeconds(skillData.intervalTime);
        skillData.SetDuration((float)Convert.ToDouble(data["Duration"]));
        duration = new WaitForSeconds(skillData.duration);
        skillData.SetSpeed(Convert.ToInt32(data["Speed"]));
        skillData.SetSplashRange(Convert.ToInt32(data["SplashRange"]));
        skillData.SetProjectileSizeMulti(Convert.ToInt32(data["ProjectileSizeMulti"]));
        skillData.SetIsPenetrate(Convert.ToBoolean(data["IsPenetrate"]));
    }

}
