using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemEffect
{
    public static void ItemEffectActivation(int param, ItemConstant type)
    {
        switch (type)
        {
            case ItemConstant.EXP:
                GameManager.Instance.ExpUp(param);
                break;
            case ItemConstant.KILLALL:
                foreach(Monster monster in MonsterSpawner.Instance.monsters)
                {
                    monster.Die(true);
                }
                break;
            case ItemConstant.MOVESTOP:
                foreach (Monster monster in MonsterSpawner.Instance.monsters)
                {
                    monster.SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.SLOW, 0.0f, param);
                }
                break;
            case ItemConstant.HEAL:
                GameManager.Instance.player.playerManager.playerData.CurrentHpModifier(param);
                break;
            case ItemConstant.MAGNET:
                SkillManager.Instance.CoroutineStarter(Magnet(param));
                break;
            case ItemConstant.GETBOX:
                GameManager.Instance.box += param;
                break;
            case ItemConstant.GETKEY:
                GameManager.Instance.key += param;
                break;
            default:
                DebugManager.Instance.PrintError("[ERROR]: 없는 아이템 효과입니다");
                break;
        }
    }

    private static IEnumerator Magnet(int param)
    {
        GameManager.Instance.player.playerManager.playerData.GetItemRangeModifier(param);
        GameManager.Instance.player.UpdateGetItemRange();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.player.playerManager.playerData.GetItemRangeModifier(-param);
        GameManager.Instance.player.UpdateGetItemRange();
    }
}