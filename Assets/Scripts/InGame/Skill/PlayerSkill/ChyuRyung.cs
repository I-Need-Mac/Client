using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChyuRyung : ActiveSkill
{
    private List<Projectile> projectiles = new List<Projectile>();
    public ChyuRyung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        float elapsedTime = 0.0f;
        while(true)
        {
            if(elapsedTime>0.1f)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
                projectile.transform.position = shooter.position;
                projectile.CollisionPower(false);
                projectiles.Add(projectile);
                elapsedTime = 0.0f;
            }
            if (projectiles.Count > 3&&Vector2.Distance(projectiles[0].transform.position, projectiles[projectiles.Count - 1].transform.position) < 0.5f)
            {
                Debug.Log("추령 발동");
                for (int i = 0; i < projectiles.Count; i++)
                {
                    SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
                }
                projectiles.Clear();
            }
            elapsedTime += Time.fixedDeltaTime;
            yield return frame;
        }
    }
    private IEnumerator Active()
    {

        yield return frame;
    }
}