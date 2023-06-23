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

    public virtual void DeActivation()
    {
        try
        {
            for (int i = 0; i < skillData.skillEffect.Count; i++)
            {
                CALC_MODE mode = (CALC_MODE)Enum.Parse(typeof(CALC_MODE), skillData.skillEffectParam[i * 2], true);
                PassiveEffect.PassiveEffectActivation(-float.Parse(skillData.skillEffectParam[i * 2 + 1]), skillData.skillEffect[i], mode);
            }
        }
        catch
        {
            DebugManager.Instance.PrintDebug("[SYSTEM]: 해제할 효과가 없습니다");
        }
    }

    public Skill(int skillId, Transform shooter)
    {
        skillTable = CSVReader.Read("SkillTable");
        this.projectiles = new List<Projectile>();
        this.skillData = new SkillData();
        this.shooter = shooter;
        SetSkillData(skillId);
    }

    public void SkillUpdate()
    {
        DeActivation();

        foreach (Projectile projectile in projectiles)
        {
            if (projectile.gameObject.activeInHierarchy)
            {
                SkillManager.Instance.DeSpawnProjectile(projectile);
            }
        }
        projectiles.Clear();
        SetSkillData(skillData.skillId);
        Init();
    }

    public void SkillLevelUp()
    {
        DeActivation();

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
        coolTime = new WaitForSeconds(skillData.coolTime / 1000.0f);
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
        skillData.SetIntervalTime(Convert.ToInt32(data["IntervalTime"]));
        intervalTime = new WaitForSeconds(skillData.intervalTime / 1000.0f);
        skillData.SetDuration(Convert.ToInt32(data["Duration"]));
        duration = new WaitForSeconds(skillData.duration / 1000.0f);
        skillData.SetSpeed(float.Parse(Convert.ToString(data["Speed"])));
        skillData.SetSplashRange(float.Parse(Convert.ToString(data["SplashRange"])));
        skillData.SetProjectileSizeMulti(float.Parse(Convert.ToString(data["ProjectileSizeMulti"])));
        skillData.SetIsPenetrate(Convert.ToBoolean(data["IsPenetrate"]));

        PlayerData playerData = GameManager.Instance.player.playerManager.playerData;
        skillData.SetProjectileSizeMulti(skillData.projectileSizeMulti + playerData.projectileSize);
        skillData.SetProjectileCount(skillData.projectileCount + playerData.projectileAdd);
        skillData.SetSplashRange(skillData.splashRange + playerData.projectileSplash);
        skillData.SetSpeed(skillData.speed + playerData.projectileSpeed);
    }

}
