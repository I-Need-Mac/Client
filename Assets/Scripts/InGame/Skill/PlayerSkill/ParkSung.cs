using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkSung : ActiveSkill
{
    public ParkSung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    List<Transform> Alltargets = new List<Transform>();
    public override IEnumerator Activation()
    {    
        Alltargets = Scanner.RangeTarget(shooter, skillData.attackDistance, 6);
        List<Transform> Skilltargets = Alltargets
        .OrderBy(monster => Vector2.Distance(shooter.position, monster.position))
        .Take(5)
        .ToList();
        foreach (Transform targets in Skilltargets)
        {
            if (targets != null)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
                projectile.transform.parent = targets;
                projectile.transform.localPosition = new Vector3(0,1,0);
                projectile.transform.localScale = Vector3.one;            
                targets.GetComponent<Monster>().SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.STUN, 5);
            }        
            Debug.Log(targets.name);
        }        
        
        Debug.Log("박승 발동");
        yield return intervalTime;
    }

}

