using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSkill : Skill
{
    private MonsterSkillData skillData;

    public abstract IEnumerator Activation();

    public MonsterSkill(int skillId, Transform shooter)
    {
        skillData = new MonsterSkillData();
    }

    public IEnumerator SkillActivation()
    {
        throw new System.NotImplementedException();
    }

    public void SkillLevelUp()
    {
        SetSkillData(skillData.skillId + 1);
    }

    public void SetSkillData(int skillId)
    {
        throw new System.NotImplementedException();
    }
}
