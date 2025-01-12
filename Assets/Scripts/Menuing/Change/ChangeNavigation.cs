using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeNavigation : MonoBehaviour
{
    public Toggle vSyncCheckbox;
    public TMP_Dropdown vSyncDropdown;
    //public Slider screenShakeSlider;
    public Toggle underDropdownToggle;

    //public GameObject vSyncCheckbox, vSyncDropdown, closeButton;

    //public OptionsValues optionValues;

    // Start is called before the first frame update
    void Start()
    {
        ChangeVSyncNavigation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVSyncNavigation()
    {
        Navigation navigationUp = vSyncCheckbox.navigation;
        Navigation navigationDown = underDropdownToggle.navigation;

        if (QualitySettings.vSyncCount == 1)
        {
            navigationUp.selectOnDown = underDropdownToggle;
            navigationDown.selectOnUp = vSyncCheckbox;
        }
        else
        {
            navigationUp.selectOnDown = vSyncDropdown;
            navigationDown.selectOnUp = vSyncDropdown;
        }
        vSyncCheckbox.navigation = navigationUp;
        underDropdownToggle.navigation = navigationDown;
    }

    /*public void ChangeScreenShakeNavigation()
    {
        Navigation navigationUp = screenShakeCheckbox.navigation;
        Navigation navigationDown = vSyncCheckbox.navigation;

        if (optionValues.cameraShakePower == 0)
        {
            navigationUp.selectOnDown = vSyncCheckbox;
            navigationDown.selectOnUp = screenShakeCheckbox;
        }
        else
        {
            navigationUp.selectOnDown = screenShakeSlider;
            navigationDown.selectOnUp = screenShakeSlider;
        }

        screenShakeCheckbox.navigation = navigationUp;
        vSyncCheckbox.navigation = navigationDown;

    }*/
}
