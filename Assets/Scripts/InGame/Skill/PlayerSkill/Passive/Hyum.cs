using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hyum : Skill
{
    public Hyum(int skillId, Transform shooter) : base(skillId, shooter)
    {
    }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        for (int i = 0; i < skillData.skillEffect.Count; i++)
        {
            CALC_MODE mode = (CALC_MODE)Enum.Parse(typeof(CALC_MODE), skillData.skillEffectParam[i * 2], true);
            PassiveEffect.PassiveEffectActivation(float.Parse(skillData.skillEffectParam[i * 2 + 1]), skillData.skillEffect[i], mode);
        }
        yield return null;
    }
}
