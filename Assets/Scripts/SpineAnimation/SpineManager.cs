using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineManager : MonoBehaviour
{
    [SerializeField] private AnimationReferenceAsset[] animationClips;
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    public AnimationConstant animationState { get; set; }
    private string currentAnimation;

    private void Start()
    {
        animationState = AnimationConstant.IDLE;
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
                skeletonAnimation.timeScale = 0f;
                //PlayAnimation(animationClips[(int)PlayerAnimationConstant.IDLE], true, 1f);
                break;
            case AnimationConstant.WALK:
                skeletonAnimation.timeScale = 1f;
                PlayAnimation(animationClips[(int)AnimationConstant.WALK], true, 1f);
                break;
        }
    }

    public void SetDirection(Vector3 direction)
    {
        float x = skeletonAnimation.transform.localScale.x;
        if (direction.x < 0)
        {
            if (x > 0)
            {
                x *= -1;
            }
        }
        else if (direction.x > 0)
        {
            if (x < 0)
            {
                x *= -1;
            }
        }
        skeletonAnimation.transform.localScale = new Vector3(x, skeletonAnimation.transform.localScale.y, skeletonAnimation.transform.localScale.z);
    }
}
