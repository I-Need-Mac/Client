using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public SkillData skillData { get; private set; }

    private List<Projectile> projectiles;
    private Player player;

    public Skill(string skillId, Player player)
    {
        projectiles = new List<Projectile>();
        skillData = new SkillData();
        SkillDataLoad(FindSkill(skillId));
        this.player = player;
    }

    //Satellite type skill activation
    public IEnumerator SatelliteSkill()
    {
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = ProjectilePoolManager.Instance.SpawnProjectile(skillData);
            projectile.angle = (360f / skillData.projectileCount) * (i + 1);
            Vector3 spawnPos = new Vector3(Mathf.Cos(projectile.angle * Mathf.Deg2Rad), Mathf.Sin(projectile.angle * Mathf.Deg2Rad), 0);
            projectile.Fire(player.transform, spawnPos);
            projectiles.Add(projectile);
        }
        yield return new WaitForSeconds(5); //지속시간인데 이거 물어봐야함 스킬데이터에없음
    }

    //Protect type skill activation
    public void ProtectSkill()
    {
        Projectile projectile = ProjectilePoolManager.Instance.SpawnProjectile(skillData);
        projectile.Fire(player.transform, Vector3.zero);
        projectiles.Add(projectile);
    }

    //Shoot type skill activation
    public IEnumerator ShootSkill()
    {
        if (!skillData.isEffect)
        {
            yield return new WaitForSeconds(skillData.coolTime);
        }
        while (true)
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                Projectile projectile = ProjectilePoolManager.Instance.SpawnProjectile(skillData);
                projectile.Fire(player.transform, player.lookDirection);
                projectiles.Add(projectile);
                yield return new WaitForSeconds(0.2f); //발사 간격
            }
            yield return new WaitForSeconds(skillData.coolTime);
        }
    }

    private void SkillLevelUp()
    {
        SkillDataLoad(FindSkill(Convert.ToString(skillData.skillId + 1)));
    }

    #region Skill Load

    private void SkillDataLoad(Dictionary<string, object> skillInfo)
    {
        if (skillInfo == null)
        {
            DebugManager.Instance.PrintDebug("존재하지 않는 스킬입니다");
            return;
        }
        skillData.SetCoolTime(Convert.ToInt32(skillInfo["Cooltime"]));
        skillData.SetAttackDistance(Convert.ToInt32(skillInfo["AttackDistance"]));
        skillData.SetDamage(Convert.ToInt32(skillInfo["Damage"]));
        skillData.SetSkillEffectParam(Convert.ToInt32(skillInfo["SkillEffectParam"]));
        skillData.SetSkillCut(Convert.ToBoolean(Convert.ToString(skillInfo["Skill_Cut"]).ToLower()));
        skillData.SetIsEffect(Convert.ToBoolean(Convert.ToString(skillInfo["IsEffect"]).ToLower()));
        skillData.SetIsUltimate(Convert.ToBoolean(Convert.ToString(skillInfo["IsUltimate"]).ToLower()));
        skillData.SetName(Convert.ToString(skillInfo["Name"]));
        skillData.SetDesc(Convert.ToString(skillInfo["Desc"]));
        skillData.SetIcon(Convert.ToString(skillInfo["Icon"]));
        skillData.SetCutDire(Convert.ToString(skillInfo["Cut_dire"]));
        skillData.SetSkillImage(Convert.ToString(skillInfo["SkillImage"]));
        skillData.SetSkillEffect((SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), Convert.ToString(skillInfo["SkillEffect"]).ToUpper()));
        skillData.SetSkillTarget((SKILL_TARGET)Enum.Parse(typeof(SKILL_TARGET), Convert.ToString(skillInfo["SkillTarget"]).ToUpper()));
        //skillData.SetCalcDamageType((CALC_DAMAGE_TYPE)Enum.Parse(typeof(CALC_DAMAGE_TYPE), Convert.ToString(skillInfo["SkillCalcType"]).ToUpper()));

        skillData.SetProjectileCount(Convert.ToInt32(skillInfo["ProjectileCount"]));
        skillData.SetSpeed(Convert.ToInt32(skillInfo["Speed"]));
        skillData.SetSplashRange(Convert.ToInt32(skillInfo["SplashRange"]));
        skillData.SetProjectileSizeMulti(Convert.ToInt32(skillInfo["ProjectileSizeMulti"]));
        skillData.SetIsPenetrate(Convert.ToBoolean(skillInfo["IsPenetrate"]));
        skillData.SetProjectileType((PROJECTILE_TYPE)Enum.Parse(typeof(PROJECTILE_TYPE), Convert.ToString(skillInfo["ProjectileType"]).ToUpper()));
    }

    private Dictionary<string, object> FindSkill(string skillId)
    {
        Dictionary<string, Dictionary<string, object>> skillTable = CSVReader.Read("SkillTable");

        if (skillTable.ContainsKey(skillId))
        {
            skillData.SetSkillId(Convert.ToInt32(skillId));
            return skillTable[skillId];
        }
        //찾는 스킬이 없을 경우 null
        return null;
    }
    #endregion
}
