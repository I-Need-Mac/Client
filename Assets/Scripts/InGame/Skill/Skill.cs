using System.Collections;

public interface Skill
{
    public abstract void Init();
    public abstract IEnumerator Activation();
    public void DeActivation();
    public void SkillUpdate();
    public void SkillLevelUp();
    public void SetSkillData(int skillId);
}
