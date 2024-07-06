using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodBless : ActiveSkill
{
    public GodBless(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        List<Transform> targets = Scanner.GetVisibleTargets(shooter, (int)LayerConstant.MONSTER);

        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        projectile.transform.position = shooter.position;
        projectile.transform.localScale = Vector3.one * 10.0f;
        //SceneManager.LoadScene("EffectScenes_GodBless", LoadSceneMode.Additive);

        yield return new WaitForSeconds(2.5f);
        DebugManager.Instance.PrintError(1);
        foreach (Transform target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.Hit(GameManager.Instance.player.playerManager.TotalDamage(skillData.damage));
            }
        }
        yield return new WaitForSeconds(1.5f);

        //SceneManager.UnloadSceneAsync("EffectScenes_GodBless");
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
}
