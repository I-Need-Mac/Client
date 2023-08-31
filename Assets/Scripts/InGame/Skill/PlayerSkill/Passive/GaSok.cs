using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaSok : PassiveSkill
{
    public GaSok(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum)
    {
    }

    public override IEnumerator Activation()
    {
        do
        {
            for (int i = 0; i < skillData.skillEffect.Count; i++)
            {
                PassiveEffect.PassiveEffectActivation(skillData.skillEffectParam[i], skillData.skillEffect[i], skillData.calcMode[i]);
            }
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0);
    }
}
