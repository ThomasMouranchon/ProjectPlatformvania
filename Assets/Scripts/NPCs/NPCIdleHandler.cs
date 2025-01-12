using FIMSpace.Basics;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class NPCIdleHandler : MonoBehaviour
{
    public Animator mainAnim, textBubbleAnim;
    public ParticleSystem[] hitEffects;
    public bool canTalk;
    //[HideInInspector]
    public bool enableTalk;
    public bool isInRange;
    [Space(10)]
    public NPCLongTalk longTalk;
    [Space(10)]

    public FixedCamera fixedCamera;
    [HideInInspector] public bool hasFixedCamera;
    public int fixedCameraStartBubble = 0;
    public int fixedCameraEndBubble = 0;
    private bool hasForcedZoomOut;

    private TalkAction talkAction;

    private void Start()
    {
        //enableTalk = canTalk;

        hasFixedCamera = fixedCamera;

        if (hasFixedCamera)
        {
            fixedCamera.enabled = false;
        }

        hasForcedZoomOut = false;

        talkAction = TalkAction.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MaskPowerProjectile" | other.tag == "MaskPowerChargedProj" | other.tag == "LingeringHitbox" | other.tag == "Counter")
        {
            if (!mainAnim.GetCurrentAnimatorStateInfo(0).IsName("NPC_Hurt"))
            {
                if (longTalk == null || !longTalk.startedTalk)
                {
                    foreach (ParticleSystem hitEffect in hitEffects)
                    {
                        Instantiate(hitEffect, transform.position, transform.rotation);
                    }
                }

            }

            if (other.tag == "MaskPowerProjectile")
            {
                if (other.GetComponent<WaterBall>()) other.GetComponent<WaterBall>().DestroyWithDelay();
            }
            if (longTalk == null || !longTalk.startedTalk) StartCoroutine(HurtCoroutine());
        }
    }

    public void HurtFunction()
    {
        StartCoroutine(HurtCoroutine());
    }

    private IEnumerator HurtCoroutine()
    {
        mainAnim.CrossFade("NPC_Hurt", 0);

        MakeCanTalkFalse();

        yield return new WaitForSeconds(0.1f);
        
        while (mainAnim.GetCurrentAnimatorStateInfo(0).IsName("NPC_Hurt"))
        {
            MakeCanTalkFalse();
            yield return null;
        }

        canTalk = true;

        if (textBubbleAnim != null && isInRange && !talkAction.isTalking)
        {
            textBubbleAnim.SetBool("canTalk", true);
        }
    }

    private void MakeCanTalkFalse()
    {
        canTalk = false;
        if (textBubbleAnim != null && isInRange)
        {
            textBubbleAnim.SetBool("canTalk", false);
        }
    }

    public void TalkFunction()
    {
        mainAnim.CrossFade("NPC_Talk", 0);
        if (textBubbleAnim != null)
        {
            textBubbleAnim.SetBool("canTalk", false);
        }
        if (longTalk != null)
        {
            longTalk.LongTalkFunction();
        }
    }

    public void NextTalkFunction()
    {
        mainAnim.CrossFade("NPC_Talk", 0);
        if (textBubbleAnim != null)
        {
            textBubbleAnim.SetBool("canTalk", false);
        }
        if (longTalk != null)
        {
            if (hasFixedCamera)
            {
                if (fixedCameraStartBubble <= longTalk.currentActiveText && fixedCameraEndBubble >= longTalk.currentActiveText)
                {
                    fixedCamera.enabled = true;
                    if (!hasForcedZoomOut)
                    {
                        talkAction.zoomHandler.Zoom(false, true);
                        hasForcedZoomOut = true;
                    }
                }
                else if (fixedCameraEndBubble < longTalk.currentActiveText && longTalk.currentActiveText < longTalk.originalTexts.Length)
                {
                    talkAction.zoomHandler.Zoom(true, true);
                    fixedCamera.enabled = false;
                }
            }
            longTalk.NextLongTalk();
        }
    }

    public void EndTalkFunction(bool idle)
    {
        if (idle)
        {
            mainAnim.CrossFade("NPC_Idle", 0);
        }
        if (textBubbleAnim)
        {
            textBubbleAnim.SetBool("canTalk", true);
        }
        if (longTalk)
        {
            if (hasFixedCamera) fixedCamera.enabled = false;
            longTalk.EndTalk();
        }

        hasForcedZoomOut = false;
    }
}
