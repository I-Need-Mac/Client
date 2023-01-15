using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpineManager : MonoBehaviour
{
    const string SPINE_HOME = "Arts/SpineSample/";
    const string SPINE_STATE = "ReferenceAssets/";

    [SerializeField] private string characterName;
    [SerializeField] private AnimationReferenceAsset[] animationClips;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private float spineSpeed = 1;

    public AnimationConstant animationState { get; set; }

    private string currentAnimation;
    private string path;

    private void Awake()
    {
        animationState = AnimationConstant.IDLE;
        path = SPINE_HOME + characterName + "/";
        skeletonAnimation.timeScale = spineSpeed;
        SpineSetting();
    }

    private void Update()
    {
        skeletonAnimation.timeScale = spineSpeed;
    }

    private void SpineSetting()
    {
        SkeletonAnimation character = transform.Find("Character").GetComponent<SkeletonAnimation>();
        character.skeletonDataAsset = Resources.Load<SkeletonDataAsset>(path + "SkeletonData");

        SpineClipSetting();
    }

    private void SpineClipSetting()
    {
        AnimationReferenceAsset[] clips = Resources.LoadAll<AnimationReferenceAsset>(path + SPINE_STATE);

        for (int i = 0; i < clips.Length; i++)
        {
            int index = (int)(AnimationConstant)Enum.Parse(typeof(AnimationConstant), clips[i].name.ToUpper());
            animationClips[index] = clips[i];
        }
    }

    private void PlayAnimation(AnimationReferenceAsset clip, bool loop, float timeScale)
    {
        if (clip.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, clip, loop).TimeScale = timeScale;
        currentAnimation = clip.name;
    }

    public void SetCurrentAnimation()
    {
        switch (animationState)
        {
            case AnimationConstant.IDLE:
                PlayAnimation(animationClips[(int)AnimationConstant.IDLE], true, 1f);
                break;
            case AnimationConstant.RUN:
                PlayAnimation(animationClips[(int)AnimationConstant.RUN], true, 1f);
                break;
        }
    }

    public void SetDirection(Vector3 direction)
    {
        float x = skeletonAnimation.transform.localScale.x;
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
        skeletonAnimation.transform.localScale = new Vector3(x, skeletonAnimation.transform.localScale.y, skeletonAnimation.transform.localScale.z);
    }
}
