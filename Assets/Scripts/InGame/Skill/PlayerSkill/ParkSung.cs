
using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkSung : ActiveSkill
{
    private List<Projectile> projectiles = new List<Projectile>();
    private List<Transform> allTargets = new List<Transform>();
    private List<Transform> targets = new List<Transform>();

    public ParkSung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        while (targets.Count<skillData.projectileCount)
        {
            Transform target = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);
            if (!targets.Contains(target))
            {
                targets.Add(target);
                if (targets.Count>=allTargets.Count)
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
                if (target.TryGetComponent(out Monster monster))
                {
                    monster.SkillEffectActivation(skillData.skillEffect[0], float.Parse(skillData.skillEffectParam[0]));
                }
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

