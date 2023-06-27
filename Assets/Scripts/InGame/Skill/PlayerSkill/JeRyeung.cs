using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeRyeung : ActiveSkill
{
    public JeRyeung(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter);
        projectile.SetAlpha(0.0f);
        projectiles.Add(projectile);
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        while (true)
        {
            List<Transform> targets;
            if (UnityEngine.Random.Range(0, 100) < int.Parse(skillData.skillEffectParam[0]))
            {
                targets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER, (int)LayerConstant.ITEM);
            }
            else
            {
                targets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
            }

            foreach (Transform target in targets)
            {
                if (target.TryGetComponent(out Monster monster))
                {
                    monster.Die();
                }
                else if (target.TryGetComponent(out Item item))
                {
                    ItemManager.Instance.DeSpawnItem(item);
                }
            }
            yield return coolTime;
        }
    }
}
