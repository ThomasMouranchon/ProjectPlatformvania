using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorTarget : MonoBehaviour
{
    public enum TargetedPlayerAnimator
    {
        MainCharacter,
        Eyes,
        Mouth
    }

    public TargetedPlayerAnimator targetedAnimator;
    public PlayAnimation playAnimation;

    void Start()
    {
        switch (targetedAnimator)
        {
            default:
                playAnimation.animator = AnimUpdater.Instance.soulAnim;
                break;
            case TargetedPlayerAnimator.Eyes:
                playAnimation.animator = AnimUpdater.Instance.eyeAnim;
                break;
        }
    }
}
