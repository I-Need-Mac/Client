using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inn : ActiveSkill
{
    Projectile projectile;
    public Inn(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    List<Transform> allTargets = new List<Transform>();
    public List<Transform> targets = new List<Transform>();
    public override IEnumerator Activation()
    {
        allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        foreach (Transform target in allTargets )
        {
            target.GetComponent<Monster>().SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.PULL,skillData.attackDistance);
            target.GetComponent<Monster>().Hit(skillData.damage);
        }
        Debug.Log("인 발동");
        yield return null;
    }
    //IEnumerator ShrinkProjectile()
    //{
    //    float initialScale = projectile.transform.localScale.x;

    //    while (initialScale > 0.0f)
    //    {
    //        float newScale = Mathf.Max(0.0f, initialScale - 10f * Time.deltaTime);
    //        projectile.transform.localScale = new Vector2(newScale, newScale);
    //        yield return null;
    //        initialScale = newScale;
    //    }
    //    if(projectile.transform.localScale.x<1f)
    //    {
    //        SkillManager.Instance.DeSpawnProjectile(projectile);
    //    }
    //}
}
