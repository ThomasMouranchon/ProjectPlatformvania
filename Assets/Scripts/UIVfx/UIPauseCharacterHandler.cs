using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

public class UIPauseCharacterHandler : MonoBehaviour
{
    public Animator soulAnim;
    public SkinnedMeshRenderer soulMeshRenderer;
    public GameObject soulPosition;
    public Vector3 additionalPosition;
    private Quaternion neutralRotation;
    public bool hasLowHealth;
    [Space(10)]
    //public GameObject soulObjectForRotation;
    public float blueMenuRotationSpeed = 5f;
    public float blueMenuRotationSmoothTime = 0.2f;

    private float currentRotationVelocity;
    private float currentInputX;
    private AnimatorStateInfo currentAnimation;

    public void UpdatePauseMenu(int subMenuValue)
    {
        soulAnim.SetInteger("subMenuValue", subMenuValue);
        soulMeshRenderer.enabled = true;

        switch (subMenuValue)
        {
            default:
                StartCoroutine(whiteCoroutine());
                break;
            case 1:
                StartCoroutine(yellowCoroutine());
                break;
            case 2:
                StartCoroutine(blueCoroutine());
                break;
            case 3:
                StartCoroutine(darkCoroutine());
                break;
            case 4:
                StartCoroutine(redCoroutine());
                break;
        }
    }

    public void CallContextualAnimation()
    {
        switch (soulAnim.GetInteger("subMenuValue"))
        {
            case 1:
                StartCoroutine(yellowContextualActionCoroutine());
                break;
            case 4:
                StartCoroutine(redContextualActionCoroutine());
                break;
        }
    }

    public void StopPauseMenu()
    {
        soulMeshRenderer.enabled = false;
    }

    IEnumerator whiteCoroutine()
    {
        if (hasLowHealth) soulAnim.CrossFade("Pause_WhiteStartLowHealth", 0);
        else soulAnim.CrossFade("Pause_WhiteStart", 0);
        yield return null;
    }

    IEnumerator yellowCoroutine()
    {
        if (hasLowHealth) soulAnim.CrossFade("Pause_YellowStartLowHealth", 0);
        else soulAnim.CrossFade("Pause_YellowStart", 0);

        while (soulAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return null;
    }

    IEnumerator blueCoroutine()
    {
        if (hasLowHealth) soulAnim.CrossFade("Pause_BlueStartLowHealth", 0);
        else soulAnim.CrossFade("Pause_BlueStart", 0);

        while (soulAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return null;
    }

    IEnumerator darkCoroutine()
    {
        if (hasLowHealth) soulAnim.CrossFade("Pause_DarkStartLowHealth", 0);
        else soulAnim.CrossFade("Pause_DarkStart", 0);

        while (soulAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return null;
    }

    IEnumerator redCoroutine()
    {
        if (hasLowHealth) soulAnim.CrossFade("Pause_RedStartLowHealth", 0);
        else soulAnim.CrossFade("Pause_RedStart", 0);

        while (soulAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return null;
    }



    IEnumerator yellowContextualActionCoroutine()
    {
        if (!(currentAnimation.normalizedTime <= 0.8f &&
         (currentAnimation.IsName("Pause_YellowStartLowHealth") |
          currentAnimation.IsName("Pause_YellowStart"))))
        {
            if (hasLowHealth) soulAnim.CrossFade("Pause_YellowActionLowHealth", 0);
            else soulAnim.CrossFade("Pause_YellowAction", 0);
        }

        yield return null;
    }

    IEnumerator redContextualActionCoroutine()
    {
        if (!(currentAnimation.normalizedTime <= 0.8f &&
         (currentAnimation.IsName("Pause_RedStartLowHealth") |
          currentAnimation.IsName("Pause_RedStart"))))
        {
            if (hasLowHealth) soulAnim.CrossFade("Pause_RedActionLowHealth", 0);
            else soulAnim.CrossFade("Pause_RedAction", 0);
        }

        yield return null;
    }
}