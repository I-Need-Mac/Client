using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : ActiveSkill
{
    //Projectile projectile;
    //List<Transform> allTargets = new List<Transform>();
    //public List<Transform> targets = new List<Transform>();
    //변수는 제일 위쪽으로, 사용 범위 생각해서 접근제한자 붙히기
    //미사용 변수는 제거
    private List<Transform> allTargets = new List<Transform>();

    public Inn(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    
    public override IEnumerator Activation()
    {
        allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        foreach (Transform target in allTargets )
        {
            //target.GetComponent<Monster>().SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.PULL,skillData.attackDistance);
            //target.GetComponent<Monster>().Hit(skillData.damage);
            if (target.TryGetComponent(out Monster monster))
            {
                monster.SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.PULL, skillData.attackDistance);
                monster.Hit(skillData.damage);
            }
        }
        Debug.Log("인 발동");
        yield return frame;

        SkillManager.Instance.DeSpawnProjectile(projectile);
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
