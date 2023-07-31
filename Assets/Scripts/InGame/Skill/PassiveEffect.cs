using SKILLCONSTANT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PassiveEffect
{
    public static void PassiveEffectActivation(float param, SKILL_PASSIVE type, CALC_MODE mode)
    {
        float data;
        switch (type)
        {
            case SKILL_PASSIVE.MAGNET:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.GetItemRangeModifier((int)data);
                GameManager.Instance.player.UpdateGetItemRange();
                break;
            case SKILL_PASSIVE.SHIELD:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ShieldModifier((int)data);
                break;
            case SKILL_PASSIVE.MOVESPEED:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.MoveSpeedModifier(data);
                break;
            case SKILL_PASSIVE.EXP:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ExpBuffModifier((int)data);
                break;
            case SKILL_PASSIVE.PROJECTILESIZE:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ProjectileSizeModifier(data);
                SkillManager.Instance.SkillDataUpdate(0.0f, 0, 0.0f, 0.0f, 0.0f, GameManager.Instance.player.playerManager.playerData.projectileSize);
                break;
            case SKILL_PASSIVE.PROJECTILECOUNT:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ProjectileAddModifier((int)data);
                SkillManager.Instance.SkillDataUpdate(0.0f, GameManager.Instance.player.playerManager.playerData.projectileAdd, 0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case SKILL_PASSIVE.PROJECTILESPLASH:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ProjectileSplashModifier(data);
                SkillManager.Instance.SkillDataUpdate(0.0f, 0, 0.0f, 0.0f, GameManager.Instance.player.playerManager.playerData.projectileSplash, 0.0f);
                break;
            case SKILL_PASSIVE.PROJECTILESPEED:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ProjectileSpeedModifier(data);
                SkillManager.Instance.SkillDataUpdate(0.0f, 0, 0.0f, GameManager.Instance.player.playerManager.playerData.projectileSpeed, 0.0f, 0.0f);
                break;
            case SKILL_PASSIVE.HP:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.HpModifier((int)data);
                break;
            case SKILL_PASSIVE.HPREGEN:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.HpRegenModifier((int)data);
                break;
            case SKILL_PASSIVE.ATTACK:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.AttackModifier(data);
                SkillManager.Instance.SkillDataUpdate(0.0f, 0, 0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case SKILL_PASSIVE.ATTACKSPEED:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.CoolDownModifier((int)data);
                SkillManager.Instance.SkillDataUpdate(0.0f, 0, 0.0f, 0.0f, 0.0f, GameManager.Instance.player.playerManager.playerData.projectileSize);
                break;
            case SKILL_PASSIVE.ARMOR:
                data = CalcData(param, mode);
                GameManager.Instance.player.playerManager.playerData.ArmorModifier((int)data);
                break;
            default:
                DebugManager.Instance.PrintDebug("[ERROR]: 없는 패시브 스킬입니다");
                break;
        }
    }

    private static float CalcData(float param, CALC_MODE mode)
    {
        if (mode == CALC_MODE.PLUS)
        {
            return param;
        }
        return param - 1.0f;
    }
}
