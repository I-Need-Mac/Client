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
                SkillManager.Instance.CoroutineStarter(Despawn(projectile));
            }
            if (projectiles.Count > 3&&Vector2.Distance(projectiles[0].transform.position, projectiles[projectiles.Count - 1].transform.position) < 0.5f)
            {
                List<Vector2> fourPoints = new List<Vector2>();
                fourPoints.Add(projectiles[0].transform.position);
                fourPoints.Add(projectiles[projectiles.Count / 3].transform.position);
                fourPoints.Add(projectiles[(2 * projectiles.Count) / 3].transform.position);
                fourPoints.Add(projectiles[projectiles.Count - 1].transform.position);
                float area = CalculateMinimumArea(fourPoints.ToArray());
                Vector2 center = CalculateCentroid(fourPoints.ToArray());
                if (area > 0.5f)
                {
                    Debug.Log("소환!");
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
                    }
                    yield return Active(area,center);
                }
                else
                {
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
                    }
                }
                projectiles.Clear();
            }
            elapsedTime += Time.fixedDeltaTime;
            yield return frame;
        }
    }
    private float CalculateMinimumArea(Vector2[] polygon)
    {
        int n = polygon.Length;

        float area = 0f;
        for (int i = 0; i < n; i++)
        {
            int j = (i + 1) % n;
            area += polygon[i].x * polygon[j].y;
            area -= polygon[j].x * polygon[i].y;
        }
        area = Mathf.Abs(area) * 0.5f;

        return area;
    }
    Vector2 CalculateCentroid(Vector2[] points)
    {
        if (points == null || points.Length == 0)
        {
            return Vector2.zero;
        }

        Vector2 centroid = Vector2.zero;

        foreach (Vector2 point in points)
        {
            centroid += point;
        }

        centroid /= points.Length;

        return centroid;
    }
    private IEnumerator Active(float area,Vector2 center)
    {
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        projectile.transform.localScale = Vector2.one* area;
        projectile.transform.localPosition = center;      
        yield return new WaitForSeconds(1f);
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
    private IEnumerator Despawn(Projectile projectile)
    {
        float despawnTime = 5f;
        do
        {
            if (despawnTime<=0)
            {
                SkillManager.Instance.DeSpawnProjectile(projectile);
                projectiles.RemoveAt(0);
                despawnTime = 5f;
            }
            else
            {
                despawnTime -= Time.fixedDeltaTime;
            }
            yield return frame;
        } while (projectiles.Count > 0);
    }
}