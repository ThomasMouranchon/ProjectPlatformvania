using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ChangeItemNavigation : MonoBehaviour
{
    public List<Button> chooseCategoryButtons;
    [Space(10)]

    public List<Button> mainButton;
    public Button keyButton;

    [Space(10)]

    public List<Button> mainButtonEnd;
    public Button keyButtonEnd;

    private void Start()
    {
        ChangeItemNav(true);
    }

    public void ChangeItemNav(bool main)
    {
        for (int i = 0; i < chooseCategoryButtons.Count; i++)
        {
            Navigation listNav = chooseCategoryButtons[i].navigation;

            if (main)
            {
                listNav.selectOnDown = mainButton[i];
                listNav.selectOnUp = mainButtonEnd[i];
            }
            else
            {
                listNav.selectOnDown = keyButton;
                listNav.selectOnUp = keyButtonEnd;
            }
            chooseCategoryButtons[i].navigation = listNav;
        }
    }
}