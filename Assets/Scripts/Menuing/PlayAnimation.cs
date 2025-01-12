using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationName;
    public float transitionTime;
    public int targetedLayer;
    [Space(30)]

    public bool canBeRestarted;

    public void CallAnimation()
    {
        if (canBeRestarted | (!canBeRestarted && !animator.GetCurrentAnimatorStateInfo(targetedLayer).IsName(animationName)))
        {
            animator.CrossFade(animationName, transitionTime, targetedLayer);
        }
    }
}