using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churk : ActiveSkill
{
    private List<Transform> allTargets = new List<Transform>();

    public Churk(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        foreach (Transform target in allTargets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.KNOCKBACK, skillData.attackDistance);
                monster.Hit(skillData.damage);
            }
        }
        Debug.Log("척 발동");
        yield return frame;
    }
}
