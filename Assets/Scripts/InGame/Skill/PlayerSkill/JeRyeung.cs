using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeRyeung : ActiveSkill
{
    public JeRyeung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            bool isDrop = true;
            List<Transform> targets;

            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
            projectile.SetAlpha(1.0f);

            if (UnityEngine.Random.Range(0, 100) < int.Parse(skillData.skillEffectParam[0]))
            {
                targets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER, (int)LayerConstant.ITEM);
                isDrop = false;
            }
            else
            {
                targets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
            }

            foreach (Transform target in targets)
            {
                if (target.TryGetComponent(out Monster monster))
                {
                    monster.Die(isDrop);
                }
                else if (target.TryGetComponent(out Item item))
                {
                    ItemManager.Instance.DeSpawnItem(item);
                }
            }
            yield return intervalTime;
            projectile.SetAlpha(0.0f);

            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);
    }
}
