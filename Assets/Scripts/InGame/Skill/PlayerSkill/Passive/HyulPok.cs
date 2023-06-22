using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyulPok : Skill
{
    public HyulPok(int skillId, Transform shooter) : base(skillId, shooter)
    {
    }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        int N = int.Parse(skillData.skillEffectParam[0]);
        int M = int.Parse(skillData.skillEffectParam[1]);

        GameManager.Instance.player.playerManager.playerData.HpModifier(-N);
        GameManager.Instance.player.playerManager.playerData.AttackModifier(M / 100.0f);

        yield return null;
    }
}
