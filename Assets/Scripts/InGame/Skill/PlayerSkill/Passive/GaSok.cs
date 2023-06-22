using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaSok : Skill
{
    public GaSok(int skillId, Transform shooter) : base(skillId, shooter)
    {
    }

    public override void Init()
    {
    }

    public override IEnumerator Activation()
    {
        int N = int.Parse(skillData.skillEffectParam[0]);

        GameManager.Instance.player.playerManager.playerData.MoveSpeedModifier((int)(N / 100.0f));

        yield return null;
    }
}
