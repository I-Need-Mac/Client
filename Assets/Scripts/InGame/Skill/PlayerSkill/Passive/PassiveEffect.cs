using SKILLCONSTANT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PassiveEffect
{
    public static void PassiveEffectActivation(float param, SKILL_EFFECT type, CALC_MODE mode)
    {
        PlayerData playerData = GameManager.Instance.player.playerManager.playerData;
        float data;
        switch (type)
        {
            case SKILL_EFFECT.MAGNET:
                data = CalcData(playerData.getItemRange, param, mode);
                GameManager.Instance.player.UpdateGetItemRange();
                playerData.GetItemRangeModifier((int)data);
                break;
            case SKILL_EFFECT.SHIELD:
                data = CalcData(playerData.shield, param, mode);
                playerData.ShieldModifier((int)data);
                break;
            case SKILL_EFFECT.MOVESPEED:
                data = CalcData(playerData.moveSpeed, param, mode);
                playerData.MoveSpeedModifier(data);
                break;
            case SKILL_EFFECT.EXP:
                data = CalcData(playerData.expBuff, param, mode);
                playerData.ExpBuffModifier((int)data);
                break;
            case SKILL_EFFECT.PROJECTILESIZE:
                data = CalcData(playerData.projectileSize, param, mode);
                playerData.ProjectileSizeModifier(data);
                break;
            case SKILL_EFFECT.PROJECTILECOUNT:
                data = CalcData(playerData.projectileAdd, param, mode);
                playerData.ProjectileAddModifier((int)data);
                break;
            case SKILL_EFFECT.PROJECTILESPLASH:
                data = CalcData(playerData.projectileSplash, param, mode);
                playerData.ProjectileSplashModifier(data);
                break;
            case SKILL_EFFECT.PROJECTILESPEED:
                data = CalcData(playerData.projectileSpeed, param, mode);
                playerData.ProjectileSpeedModifier(data);
                break;
            case SKILL_EFFECT.HP:
                data = CalcData(playerData.hp, param, mode);
                playerData.HpModifier((int)data);
                break;
            case SKILL_EFFECT.HPREGEN:
                data = CalcData(playerData.hpRegen, param, mode);
                playerData.HpRegenModifier((int)data);
                break;
            case SKILL_EFFECT.ATTACK:
                data = CalcData(playerData.attack, param, mode);
                playerData.AttackModifier(data);
                break;
            case SKILL_EFFECT.ATTACKSPEED:
                data = CalcData(playerData.coolDown, param, mode);
                playerData.CoolDownModifier((int)data);
                break;
            case SKILL_EFFECT.ARMOR:
                data = CalcData(playerData.armor, param, mode);
                playerData.ArmorModifier((int)data);
                break;
            default:
                DebugManager.Instance.PrintDebug("[ERROR]: 없는 패시브 스킬입니다");
                break;
        }

        SkillManager.Instance.SkillUpdate();
    }

    private static float CalcData(float origin, float param, CALC_MODE mode)
    {
        if (mode == CALC_MODE.PLUS)
        {
            return origin + param;
        }
        else
        {
            return origin * param * 0.01f;
        }
    }
}
