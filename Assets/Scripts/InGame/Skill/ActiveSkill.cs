using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : Skill
{
    private Dictionary<string, Dictionary<string, object>> skillTable;

    protected int skillNum;
    protected Transform shooter;
    protected List<Projectile> projectiles;
    protected ActiveData skillData;
    protected Vector2 originSize;

    protected WaitForSeconds coolTime;
    protected WaitForSeconds intervalTime;
    protected WaitForSeconds duration;

    public abstract void Init();
    public abstract IEnumerator Activation();

    public ActiveSkill(int skillId, Transform shooter, int skillNum)
    {
        skillTable = CSVReader.Read("SkillTable");
        this.projectiles = new List<Projectile>();
        this.skillData = new ActiveData();
        this.shooter = shooter;
        SetSkillData(skillId);
        this.skillNum = skillNum;
    }

    public void DeActivation()
    {
        foreach (Projectile projectile in projectiles)
        {
            if (projectile.gameObject.activeInHierarchy)
            {
                SkillManager.Instance.DeSpawnProjectile(projectile);
            }
        }
        projectiles.Clear();
    }

    public void SkillUpdate()
    {
        DeActivation();
        SetSkillData(skillData.skillId);
        Init();
    }

    public void SkillLevelUp()
    {
        DeActivation();
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
            try
            {
                List<SKILL_EFFECT> list2 = new List<SKILL_EFFECT>()
            {
                (SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), Convert.ToString(data["SkillEffect"]), true),
            };
                skillData.SetSkillEffect(list2);
            }
            catch
            {
                skillData.SetSkillEffect(new List<SKILL_EFFECT>());
            }
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
