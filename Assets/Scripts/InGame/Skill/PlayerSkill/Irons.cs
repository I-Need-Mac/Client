using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Irons : ActiveSkill
{
    private int weight;

    public Irons(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override void Init()
    {
        weight = 0;
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        while (true)
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                ProjectileStraight projectile = (ProjectileStraight)SkillManager.Instance.SpawnProjectile(skillData);
                projectile.transform.localPosition = shooter.position;
                Vector2 pos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
                pos -= (Vector2)shooter.position;
                float angle = 0.0f;
                if (UnityEngine.Random.Range(0, 100) < 20)
                {
                    pos *= -1;
                }
                else
                {
                    if (weight < (skillData.projectileCount / 5))
                    {
                        ++weight;
                    }
                    else
                    {
                        DebugManager.Instance.PrintDebug("[TEST]: ");
                        weight = 0;
                        pos *= -1;
                        angle += UnityEngine.Random.Range(1, 10) * 10;
                    }
                }
                angle += Quaternion.FromToRotation(Vector3.up, pos).eulerAngles.z;
                projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                yield return intervalTime;
            }

            yield return PlayerStatusUI.Instance.boxIcons[skillNum].Dimmed(skillData.coolTime);
            //yield return coolTime;
        }
    }
}
