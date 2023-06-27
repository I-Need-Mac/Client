using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : ActiveSkill
{
    private const int CHANGE_ID = 0;
    private const string ANIMATOR_PATH = "";
    private const string SKELETONDATA_ASSET_PATH = "";

    private Player player;
    private Animator animator;
    private SkeletonMecanim mecanim;

    public Possession(int skillId, Transform shooter) : base(skillId, shooter) { }

    public override void Init()
    {
        player = shooter.GetComponent<Player>();
        animator = shooter.GetComponentInChildren<Animator>();
        mecanim = shooter.GetComponentInChildren<SkeletonMecanim>();
    }

    public override IEnumerator Activation()
    {
        if (!skillData.isEffect)
        {
            yield return coolTime;
        }

        Dictionary<string, Dictionary<string, object>> table = CSVReader.Read("CharacterTable");
        RuntimeAnimatorController originalController = animator.runtimeAnimatorController;
        SkeletonDataAsset originalDataAsset = mecanim.skeletonDataAsset;

        player.playerManager.PlayerSetting(table[CHANGE_ID.ToString()]);
        animator.runtimeAnimatorController = ResourcesManager.Load<RuntimeAnimatorController>(ANIMATOR_PATH);
        mecanim.skeletonDataAsset = ResourcesManager.Load<SkeletonDataAsset>(SKELETONDATA_ASSET_PATH);

        yield return duration;

        player.playerManager.PlayerSetting(table[GameManager.Instance.GetPlayerId().ToString()]);
        animator.runtimeAnimatorController = originalController;
        mecanim.skeletonDataAsset = originalDataAsset;
    }
}
