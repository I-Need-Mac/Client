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
    private Spine.AnimationState animationState;

    public Animator animator { get; private set; }

    private string path;

    //public string animationState { get; set; }

    private void Awake()
    {
        Init();
    }

    private void PlayAnimation(Vector2 tracker, Vector2 target)
    {
        if (tracker == Vector2.zero)
        {

        }
    }

    private void Init()
    {
        path = PathFinding();

        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
        skeletonMecanim.skeletonDataAsset = ResourcesManager.Load<SkeletonDataAsset>(path + "SkeletonData");

        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = ResourcesManager.Load<RuntimeAnimatorController>(path + "Controller");
    }

    private string PathFinding()
    {
        if (TryGetComponent(out Player obj))
        {
            return obj.playerData.characterSpinePath + IMAGE_PATH;
        }
        return GetComponent<Monster>().monsterData.monsterImage + IMAGE_PATH;
    }

    public void SetDirection(Transform transform, Vector3 direction)
    {
        float x = transform.localScale.x;
        if (direction.x < 0)
        {
            if (x < 0)
            {
                x *= -1;
            }
        }
        else if (direction.x > 0)
        {
            if (x > 0)
            {
                x *= -1;
            }
        }
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

}
