using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JuHyung : ActiveSkill
{
    public JuHyung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        //for (int i = 0; i < skillData.projectileCount; i++)
        //{
        //    Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        //    projectile.transform.localPosition = shooter.position;
        //    Vector2 pos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
        //    pos -= (Vector2)shooter.position;
        //    projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90.0f);
        //    yield return intervalTime;
        //}
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        SkillManager.Instance.CoroutineStarter(Chaining(projectile));
        yield return intervalTime;
    }

    //ProjectileCount: 전이 개수
    private IEnumerator Chaining(Projectile projectile)
    {
        //첫 전이 대상 몬스터
        Transform firstTarget = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);
        if (firstTarget == null)
        {
            yield break;
        }

        List<Transform> aeTarget = Scanner.RangeTarget(firstTarget, skillData.attackDistance * 0.5f, (int)LayerConstant.MONSTER);
        if (aeTarget.Count < 1)
        {
            yield break;
        }

        Dictionary<Transform, float> map = new Dictionary<Transform, float>();      //key: 몬스터, value: 거리
        foreach (Transform target in aeTarget)
        {
            map.Add(target, Vector2.Distance(firstTarget.position, target.position));
        }
        aeTarget = map.OrderBy(entry => entry.Value)
                      .Select(entry => entry.Key)
                      .ToList()
                      .Take(skillData.projectileCount)
                      .ToList();

        foreach (Transform target in aeTarget)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.Hit(skillData.damage);
            }
        }

        yield return frame;
    }

}
