using SKILLCONSTANT;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private SkillData skillData;
    private string skillId;
    //값에 변화가 생기는 데이터들 관리
    private int coolTime;
    private int attackDistance;
    private int damage;
    //투사체용
    private int projectileCount;
    private int speed;
    private int splashRange;
    private int projectileSizeMulti;
    private bool isPenetrate;
    private PROJECTILE_TYPE projectileType;

    private void SkillSetting()
    {
        SkillLoad(FindSkill(skillId));

        coolTime = skillData.coolTime;
        attackDistance = skillData.attackDistance;
        damage = skillData.damage;

        projectileCount = skillData.projectileCount;
        speed = skillData.speed;
        splashRange = skillData.splashRange;
        projectileSizeMulti = skillData.projectileSizeMulti;
        isPenetrate = skillData.isPenetrate;
        projectileType = skillData.projectileType;
    }

    private void SkillLoad(Dictionary<string, object> skillInfo)
    {
        if (skillInfo == null)
        {
            DebugManager.Instance.PrintDebug("존재하지 않는 스킬입니다");
            return;
        }

        skillData.SetSkillId(Convert.ToInt32(skillInfo["SkillID"]));
        skillData.SetCoolTime(Convert.ToInt32(skillInfo["Cooltime"]));
        skillData.SetAttackDistance(Convert.ToInt32(skillInfo["AttackDistance"]));
        skillData.SetDamage(Convert.ToInt32(skillInfo["Damage"]));
        skillData.SetSkillEffectParam(Convert.ToInt32(skillInfo["SkillEffectParam"]));
        skillData.SetSkillCut(Convert.ToBoolean(skillInfo["Skill_Cut"]));
        skillData.SetIsEffect(Convert.ToBoolean(skillInfo["IsEffect"]));
        skillData.SetIsUltimate(Convert.ToBoolean(skillInfo["IsUltimate"]));
        skillData.SetName(Convert.ToString(skillInfo["Name"]));
        skillData.SetDesc(Convert.ToString(skillInfo["Desc"]));
        skillData.SetIcon(Convert.ToString(skillInfo["Icon"]));
        skillData.SetCutDire(Convert.ToString(skillInfo["Cut_dire"]));
        skillData.SetSkillImage(Convert.ToString(skillInfo["SkillImage"]));
        skillData.SetSkillEffect((SKILL_EFFECT)skillInfo["SkillEffect"]);
        skillData.SetSkillTarget((SKILL_TARGET)skillInfo["SkillTarget"]);
        skillData.SetCalcDamageType((CALC_DAMAGE_TYPE)skillInfo["SkillCalcType"]);

        skillData.SetProjectileCount(Convert.ToInt32(skillInfo["ProjectileCount"]));
        skillData.SetSpeed(Convert.ToInt32(skillInfo["Speed"]));
        skillData.SetSplashRange(Convert.ToInt32(skillInfo["SplashRange"]));
        skillData.SetProjectileSizeMulti(Convert.ToInt32(skillInfo["ProjectileSizeMulti"]));
        skillData.SetIsPenetrate(Convert.ToBoolean(skillInfo["IsPenetrate"]));
        skillData.SetProjectileType((PROJECTILE_TYPE)skillInfo["ProjectileType"]);
    }

    private Dictionary<string, object> FindSkill(string skillId)
    {
        Dictionary<string, Dictionary<string, object>> skillTable = CSVReader.Read("SkillTable");

        if (skillTable.ContainsKey(skillId))
        {
            return skillTable[skillId];
        }
        //찾는 스킬이 없을 경우 null
        return null;
    }
}
