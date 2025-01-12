using FIMSpace.FProceduralAnimation;
using FIMSpace.FTail;
using FIMSpace.GroundFitter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimUpdater : MonoBehaviour
{
    private static AnimUpdater instance = null;
    public static AnimUpdater Instance => instance;

    public TailAnimator2[] clothTailAnims, frontClothTailAnims;
    [Space(5)]
    public LegsAnimator soulLegsAnim;
    public FGroundRotator soulGroundRotator;
    public Animator soulAnim, eyeAnim, clothAnim, hatAndScarfAnim;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
}
