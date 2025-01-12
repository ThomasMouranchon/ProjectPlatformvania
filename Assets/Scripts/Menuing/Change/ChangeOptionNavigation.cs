using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ChangeOptionNavigation : MonoBehaviour
{
    public List<Button> chooseCategoryButtons;
    [Space(10)]

    public Button gameplayButton;
    public Button graphicsButton;
    public Button controlsButton;
    public Button audioButton;
    public Toggle datasToggle;

    [Space(10)]

    public Toggle gameplayEndToggle;
    public TMP_Dropdown graphicsEndDropdown;
    public Toggle controlsEndToggle;
    public Slider audioEndSlider;
    public Button datasEndButton;

    private void Start()
    {
        ChangeOptionNav(0);
    }

    public void ChangeOptionNav(int value)
    {
        for (int i = 0; i < chooseCategoryButtons.Count; i++)
        {
            Navigation listNav = chooseCategoryButtons[i].navigation;// = firstsButtons[value].selectable.navigation;

            switch (value)
            {
                case 0:
                    listNav.selectOnDown = gameplayButton;
                    listNav.selectOnUp = gameplayEndToggle;
                    break;
                case 1:
                    listNav.selectOnDown = graphicsButton;
                    listNav.selectOnUp = graphicsEndDropdown;
                    break;
                case 2:
                    listNav.selectOnDown = controlsButton;
                    listNav.selectOnUp = controlsEndToggle;
                    break;
                case 3:
                    listNav.selectOnDown = audioButton;
                    listNav.selectOnUp = audioEndSlider;
                    break;
                case 4:
                    listNav.selectOnDown = datasToggle;
                    listNav.selectOnUp = datasEndButton;
                    break;
            }
            chooseCategoryButtons[i].navigation = listNav;
        }

        //closeOptionsButton.navigation = listEndNav;
    }
}
