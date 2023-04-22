using Spine.Unity;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpineAnimatorManager : MonoBehaviour
{
    private Animator animator;

    private float animationSpeed = 1f;

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        animator.speed = animationSpeed;
    }

    private void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetDirection(Transform transform, Vector3 direction)
    {
        if (direction.x < 0)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (direction.x > 0)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
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
        animationSpeed = 1 + weight;
        DebugManager.Instance.PrintDebug("SpineSpeed: {0}", animationSpeed);
        DebugManager.Instance.PrintDebug("Weight: {0}", weight);
    }

    #region Animation Control
    public void PlayAnimation(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    public void PlayAnimation(string parameter, int value)
    {
        animator.SetInteger(parameter, value);
    }

    public void PlayAnimation(string parameter, float value)
    {
        animator.SetFloat(parameter, value);
    }
    #endregion
}
