using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpineManager : MonoBehaviour
{
    const string SPINE_HOME = "Arts/SpineSample/";

    [SerializeField] private string characterName;
    [SerializeField] private AnimationReferenceAsset[] animationClips;
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    public AnimationConstant animationState { get; set; }

    private string currentAnimation;

    private void Awake()
    {
        animationState = AnimationConstant.IDLE;
    }

    private void Start()
    {
        SpineSetting();
    }

    private void SpineSetting()
    {
        string path = SPINE_HOME + characterName + "/";
        SkeletonAnimation character = transform.Find("Character").GetComponent<SkeletonAnimation>();
        character.skeletonDataAsset = Resources.Load<SkeletonDataAsset>(path + "SkeletonData");
        animationClips[0] = Resources.Load<AnimationReferenceAsset>(path + "ReferenceAssets/IDLE");
        animationClips[1] = Resources.Load<AnimationReferenceAsset>(path + "ReferenceAssets/RUN");
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
        skeletonAnimation.timeScale = 1f;
        switch (animationState)
        {
            case AnimationConstant.IDLE:
                PlayAnimation(animationClips[(int)AnimationConstant.IDLE], true, 1f);
                break;
            case AnimationConstant.WALK:
                PlayAnimation(animationClips[(int)AnimationConstant.WALK], true, 1f);
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
