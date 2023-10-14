using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkSung : ActiveSkill
{
    List<Projectile> projectiles = new List<Projectile>();
    public ParkSung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    List<Transform> Alltargets = new List<Transform>();
    public override IEnumerator Activation()
    {       
        Alltargets = Scanner.RangeTarget(shooter, skillData.attackDistance, 6);
        List<Transform> Skilltargets = Alltargets
        .OrderBy(monster => Vector2.Distance(shooter.position, monster.position))
        .Take(skillData.projectileCount)
        .ToList();
        
        foreach (Transform target in Skilltargets)
        {
            if (target != null)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
                projectile.transform.parent = target;
                projectile.transform.localPosition = new Vector3(0, 1, 0);
                projectile.transform.localScale = Vector3.one;
                target.GetComponent<Monster>().SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.STUN, skillData.duration);
                target.GetComponent<Monster>().Hit(skillData.damage);
                projectiles.Add(projectile);
            }
        }
        Debug.Log("박승 발동");
        yield return new WaitForSeconds(skillData.duration);
        for(int i = 0; i< projectiles.Count; i++)
        {
            SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        }
        yield return intervalTime;
    }

}

