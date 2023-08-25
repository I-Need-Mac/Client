using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Irons : ActiveSkill
{
    public Irons(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        //int weight = 0;

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        List<int> angles = new List<int>();
        do
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                angles.Add(i);
            }
            angles.FisherYateShuffle();

            for (int i = 0; i < skillData.projectileCount; i++)
            {
                ProjectileStraight projectile = SkillManager.Instance.SpawnProjectile<ProjectileStraight>(skillData);
                projectile.transform.localPosition = shooter.position;
                //Vector2 pos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
                //pos -= (Vector2)shooter.position;
                //float angle = 0.0f;
                //if (UnityEngine.Random.Range(0, 100) < 20)
                //{
                //    pos *= -1;
                //}
                //else
                //{
                //    if (weight < (skillData.projectileCount / 5))
                //    {
                //        ++weight;
                //    }
                //    else
                //    {
                //        DebugManager.Instance.PrintDebug("[TEST]: ");
                //        weight = 0;
                //        pos *= -1;
                //        angle += UnityEngine.Random.Range(1, 10) * 10;
                //    }
                //}
                //angle += Quaternion.FromToRotation(Vector3.up, pos).eulerAngles.z;
                projectile.transform.rotation = Quaternion.Euler(0, 0, angles[i] * 360.0f / skillData.projectileCount);
                yield return intervalTime;
            }

            angles.Clear();
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);

    }
}
