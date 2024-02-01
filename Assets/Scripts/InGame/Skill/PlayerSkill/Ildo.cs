using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Ildo : ActiveSkill
{
    public Ildo(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        //Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        //projectile.transform.localPosition = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
        //projectile.CollisionPower(false);     
        //int padongCount = 0;
        //do
        //{
        //    yield return intervalTime;
        //    SkillManager.Instance.CoroutineStarter(Padong(projectile));
        //    padongCount++;
        //} while (padongCount < skillData.projectileCount);
        //yield return frame;
        //SkillManager.Instance.DeSpawnProjectile(projectile);

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
            projectile.transform.position = shooter.position;
            projectile.CollisionPower(false);
            SkillManager.Instance.CoroutineStarter(Effect(projectile));
            yield return intervalTime;
        }

    }

    private IEnumerator Effect(Projectile projectile)
    {
        Vector2 targetPos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
        Vector2 direction = (targetPos - (Vector2)shooter.position).normalized;
        Vector3 speed = direction * skillData.speed * Time.fixedDeltaTime;
        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        float diff = 0.0f;
        float distance = Vector2.Distance(projectile.transform.position, targetPos);
        do
        {
            //projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPos, skillData.speed * Time.fixedDeltaTime);
            projectile.transform.position = Vector3.SmoothDamp(projectile.transform.position, targetPos, ref speed, 0.25f);
            yield return frame;
            diff = Vector2.Distance(projectile.transform.position, targetPos);
            projectile.SetAlpha(diff / distance);
        } while (diff > 0.25f);

        projectile.SetAlpha(1.0f);
        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
        projectile.transform.position = targetPos + new Vector2(0.0f, 2.5f);
        projectile.CollisionPower(true);

        do
        {
            projectile.transform.position = Vector3.Lerp(projectile.transform.position, targetPos, 15.0f * Time.fixedDeltaTime);
            diff = Vector2.Distance(projectile.transform.position, targetPos);
            yield return frame;
        } while (diff > 0.5f);

        yield return new WaitForSeconds(0.5f);
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }

    //private IEnumerator Padong(Projectile projectile)
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(projectile.transform.position, skillData.splashRange);
    //    foreach(Collider2D target in hits)
    //    {
    //        if(target.TryGetComponent(out Monster monster))
    //        {
    //            monster.SkillEffectActivation(skillData.skillEffect[1], float.Parse(skillData.skillEffectParam[1]));
    //            monster.Hit(skillData.damage);
    //        }
    //    }
    //    yield return frame;
    //}
}
