using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHurtVfx : MonoBehaviour
{
    public GameObject crack, maskLeft, maskRight;
    private MMFeedbacks crackFeedback, maskLeftFeedback, maskRightFeedback;

    void Awake()
    {
        crackFeedback = crack.gameObject.GetComponent<MMFeedbacks>();
        maskLeftFeedback = maskLeft.gameObject.GetComponent<MMFeedbacks>();
        maskRightFeedback = maskRight.gameObject.GetComponent<MMFeedbacks>();
    }

    void Start()
    {
        DeactivateEffect();
    }

    public void UIHurtEffect()
    {
        StartCoroutine(UIHurtEffectCoroutine());
    }

    public IEnumerator UIHurtEffectCoroutine()
    {
        crack.SetActive(true);
        maskLeft.SetActive(true);
        maskRight.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        DeactivateEffect();
    }

    private void DeactivateEffect()
    {
        crack.SetActive(false);
        maskLeft.SetActive(false);
        maskRight.SetActive(false);
    }
}
