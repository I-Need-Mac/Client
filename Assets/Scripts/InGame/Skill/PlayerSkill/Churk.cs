using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churk : ActiveSkill
{
    private List<Transform> allTargets = new List<Transform>();

    Projectile projectile;
    public Churk(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER,(int)LayerConstant.ITEM);
        projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        Debug.Log("척 발동");
        yield return expansionProjectile();
    }
    IEnumerator expansionProjectile()
    {
        float initialScale = skillData.projectileSizeMulti;

        while (initialScale < skillData.attackDistance * 2)
        {
            float newScale = Mathf.Max(0.0f, initialScale + 10.0f * Time.deltaTime);
            projectile.transform.localScale = new Vector2(newScale, newScale);
            yield return frame;
            initialScale = newScale;
        }
        yield return frame;
        SkillManager.Instance.DeSpawnProjectile(projectile);       
    }
}
