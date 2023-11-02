
using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkSung : ActiveSkill
{
    List<Projectile> projectiles = new List<Projectile>();
    public ParkSung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    List<Transform> allTargets = new List<Transform>();
    public List<Transform> targets = new List<Transform>();
    public override IEnumerator Activation()
    {
        //allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        //List<Transform> skillTargets = allTargets
        //.OrderBy(monster => Vector2.Distance(shooter.position, monster.position))
        //.Take(skillData.projectileCount)
        //.ToList();

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Transform target = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

            if (!targets.Contains(target))
            {
                targets.Add(target);

                if (targets.Count >= 5)
                {
                    break;
                }
            }
        }


        foreach (Transform target in targets)
        {
            if (target != null)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
                projectile.transform.position = target.position+new Vector3(0,0.3f,0);
                target.GetComponent<Monster>().SkillEffectActivation(SKILL_EFFECT.STUN, 3);
                projectiles.Add(projectile);
            }
        }
        Debug.Log("박승 발동");
        yield return new WaitForSeconds(int.Parse(skillData.skillEffectParam[0]));
        for(int i = 0; i< projectiles.Count; i++)
        {
            SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        }
    }

}

