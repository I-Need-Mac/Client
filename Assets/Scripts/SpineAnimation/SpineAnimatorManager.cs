using Spine.Unity;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpineAnimatorManager : MonoBehaviour
{
    private const string IMAGE_PATH = "/Animation/Spine/";

    private SkeletonMecanim skeletonMecanim;
    private Animator animator;

    private string path;

    public AnimationConstant animationState { get; set; }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        path = PathFinding();
        skeletonMecanim = transform.Find("Character").GetComponent<SkeletonMecanim>();
        skeletonMecanim.skeletonDataAsset = ResourcesManager.Load<SkeletonDataAsset>(path + "SkeletonData");
        skeletonMecanim.Initialize(true);

        animator = transform.Find("Character").GetComponent<Animator>();
        animator.runtimeAnimatorController = ResourcesManager.Load<RuntimeAnimatorController>(path + "Controller");
        DebugManager.Instance.PrintDebug("#######" + ResourcesManager.Load<SkeletonDataAsset>(path + "SkeletonData"));
    }

    private string PathFinding()
    {
        return GetComponent<Monster>().monsterData.monsterImage + IMAGE_PATH;
    }

}
