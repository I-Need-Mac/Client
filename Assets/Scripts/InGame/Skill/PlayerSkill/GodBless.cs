using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBless : ActiveSkill
{
    public GodBless(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        List<Transform> targets = Scanner.GetVisibleTargets(shooter, (int)LayerConstant.MONSTER);

        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        projectile.transform.localScale = Vector3.one * 5.0f;

        yield return new WaitForSeconds(3.0f);
        foreach (Transform target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.Hit(GameManager.Instance.player.playerManager.TotalDamage(skillData.damage));
            }
        }

        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
}
