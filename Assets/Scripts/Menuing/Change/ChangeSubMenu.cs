using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSubMenu : MonoBehaviour
{
    public GameObject goToshow;
    public List<GameObject> goListToHide;
    private List<Animator> goListAnim;

    [Space(10)]
    public RectTransform validationIcon;
    private Animator validationIconAnim;

    private RectTransform targetRectTransform;

    private void Start()
    {

    }

    private void AssignComponents()
    {
        goListAnim = new List<Animator>();
        goListAnim.Add(goToshow.GetComponent<Animator>());
        for (int i = 0; i < goListToHide.Count; i++)
        {
            goListAnim.Add(goListToHide[i].GetComponent<Animator>());
        }

        targetRectTransform = this.GetComponent<RectTransform>();

        validationIconAnim = validationIcon.GetComponentInChildren<Animator>();
    }

    public void ShowGameObject()
    {
        goToshow.SetActive(true);
        if (!validationIconAnim)
        {
            AssignComponents();
        }
        validationIconAnim.CrossFade("ValidateIcon_IdleOff", 0);
        validationIconAnim.SetBool("play", true);
        validationIcon.anchoredPosition = targetRectTransform.anchoredPosition;

        if (!goListAnim[0].GetCurrentAnimatorStateInfo(0).IsName("OptionsSubButtons_idleOn"))
        {
            goListAnim[0].CrossFade("OptionsSubButtons_Open", 0);
        }
        
        for (int i = 0; i < goListToHide.Count; i++)
        {
            goListToHide[i].SetActive(false);
        }
    }
}
