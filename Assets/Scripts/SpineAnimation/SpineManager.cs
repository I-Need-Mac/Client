using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Subject
{
    PLAYER,
    MONSTER,
}

public class SpineManager : MonoBehaviour
{
    const string SPINE_HOME = "Arts/Spine/";
    const string SPINE_STATE = "ReferenceAssets";

    [SerializeField] private Subject subject = Subject.PLAYER;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private float spineSpeed = 1;

    private AnimationReferenceAsset[] animationClips;
    private string currentAnimation;
    private string path;

    public AnimationConstant animationState { get; set; }

    private void Awake()
    {
        SpineSetting();
    }

    private void Update()
    {
        skeletonAnimation.timeScale = spineSpeed;
        //SpineChange();
    }

    //private void SpineChange()
    //{
    //    if (Input.GetKeyUp(KeyCode.Alpha1))
    //    {
    //        PlayerPoolManager.Instance.playerId = 101;
    //        SpineSetting();
    //    }
    //    if (Input.GetKeyUp(KeyCode.Alpha2))
    //    {
    //        PlayerPoolManager.Instance.playerId = 102;
    //        SpineSetting();
    //    }
    //    if (Input.GetKeyUp(KeyCode.Alpha3))
    //    {
    //        PlayerPoolManager.Instance.playerId = 103;
    //        SpineSetting();
    //    }
    //    if (Input.GetKeyUp(KeyCode.Alpha4))
    //    {
    //        PlayerPoolManager.Instance.playerId = 104;
    //        SpineSetting();
    //    }
    //}

    private void SpineSetting()
    {
        animationState = AnimationConstant.IDLE;

        switch (subject)
        {
            case Subject.PLAYER:
                path = SPINE_HOME + (string)CSVReader.Read("CharacterTable", Convert.ToString(GameManager.Instance.GetPlayerId()), "CharacterSpinePath") + "/";
                break;
            case Subject.MONSTER:
                //path = GetComponent<Monster>().monsterData.monsterImage + "/Animation/Spine/";
                DebugManager.Instance.PrintDebug(">>>>" + path);
                break;
        }

        skeletonAnimation.timeScale = spineSpeed;

        SkeletonAnimation character = transform.Find("Character").GetComponent<SkeletonAnimation>();
        //character.skeletonDataAsset = Resources.Load<SkeletonDataAsset>(path + "SkeletonData");
        character.skeletonDataAsset = ResourcesManager.Load<SkeletonDataAsset>(path + "SkeletonData");
        skeletonAnimation.skeletonDataAsset = character.skeletonDataAsset;
        skeletonAnimation.Initialize(true);

        SpineClipSetting();
    }

    private void SpineClipSetting()
    {
        //AnimationReferenceAsset[] clips = Resources.LoadAll<AnimationReferenceAsset>(path + SPINE_STATE);
        AnimationReferenceAsset[] clips = ResourcesManager.LoadAll<AnimationReferenceAsset>(path + SPINE_STATE);
        animationClips = new AnimationReferenceAsset[clips.Length];
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

    public void SetSpineSpeed(float speed)
    {
        float weight = 0;
        if (speed > 5)
        {
            weight = ((float)Math.Pow(speed - 5, 2.0f / 3.0f) + (float)Math.Sqrt(speed - 5) - 1.0f) / 10.0f;
        }
        else
        {
            weight = (0.5f - speed / 10.0f) * -1.0f;
        }
        spineSpeed = 1 + weight;
        DebugManager.Instance.PrintDebug("SpineSpeed: {0}", spineSpeed);
        DebugManager.Instance.PrintDebug("Weight: {0}", weight);
    }

    public void SetCurrentAnimation()
    {
        PlayAnimation(animationClips[(int)animationState], true, 1f);
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
