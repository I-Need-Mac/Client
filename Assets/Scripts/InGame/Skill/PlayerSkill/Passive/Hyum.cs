using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hyum : PassiveSkill
{
    public Hyum(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum)
    {
    }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        do
        {
            for (int i = 0; i < skillData.skillEffect.Count; i++)
            {
                CALC_MODE mode = (CALC_MODE)Enum.Parse(typeof(CALC_MODE), skillData.skillEffectParam[i], true);
                PassiveEffect.PassiveEffectActivation(float.Parse(skillData.skillEffectParam[i]), skillData.skillEffect[i], mode);
            }
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0);
    }
}
