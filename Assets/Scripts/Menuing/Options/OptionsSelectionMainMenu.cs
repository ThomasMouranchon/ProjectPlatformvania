using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsSelectionMainMenu : MonoBehaviour
{
    [Range(0.01f, 0.5f)]
    public float characterDeadZoneSliderValue;
    public Slider characterDeadZoneSlider;
    public TMP_Text characterDeadZoneText;
    [Space(10)]
    [Range(0.01f, 0.2f)]
    public float cameraDeadZoneSliderValue;
    public Slider cameraDeadZoneSlider;
    public TMP_Text cameraDeadZoneText;
    [Space(10)]
    [Range(0.5f, 5)]
    public float cameraSpeedHorizontalMultiplierSliderValue;
    public Slider cameraSpeedHorizontalMultiplierSlider;
    public TMP_Text cameraSpeedHorizontalMultiplierText;
    [Space(10)]
    [Range(0.5f, 5)]
    public float cameraSpeedVerticalMultiplierSliderValue;
    public Slider cameraSpeedVerticalMultiplierSlider;
    public TMP_Text cameraSpeedVerticalMultiplierText;
    [Space(10)]
    public Toggle invertXAxis;
    public Toggle invertYAxis;
    [Space(10)]
    public Toggle cameraShakeToggle;
    [Space(10)]
    public Toggle use3DEffects;
    [Space(10)]
    public Toggle vSyncCountToggle;
    public TMPro.TMP_Dropdown framerateSelection;
    public GameObject framerateSelectionGameobject;
    public int desiredFramerate = 30;

    /*[Space(10)]
    public bool doubleTapReset = true;*/
    //public OptionsSelection otherOptionSelection;

    [Space(10)]
    public OptionsValuesMainMenu optionsValuesMainMenu;

    // Start is called before the first frame update
    void Awake()
    {
        //OnStartOptionsSelection(this);
        //OnStartOptionsSelection(otherOptionSelection);
        
        characterDeadZoneSliderValue = optionsValuesMainMenu.controllerMoveDeadZone;
        switch (characterDeadZoneSliderValue)
        {
            case 0.01f:
                characterDeadZoneSlider.value = 0;
                break;
            case 0.05f:
                characterDeadZoneSlider.value = 1;
                break;
            case 0.1f:
                characterDeadZoneSlider.value = 2;
                break;
            case 0.15f:
                characterDeadZoneSlider.value = 3;
                break;
            case 0.2f:
                characterDeadZoneSlider.value = 4;
                break;
            case 0.25f:
                characterDeadZoneSlider.value = 5;
                break;
            case 0.3f:
                characterDeadZoneSlider.value = 6;
                break;
            case 0.35f:
                characterDeadZoneSlider.value = 7;
                break;
            case 0.4f:
                characterDeadZoneSlider.value = 8;
                break;
            case 0.45f:
                characterDeadZoneSlider.value = 9;
                break;
            case 0.5f:
                characterDeadZoneSlider.value = 10;
                break;
            default:
                characterDeadZoneSlider.value = 2;
                characterDeadZoneSliderValue = 0.2f;
                break;
        }
        characterDeadZoneText.text = "" + characterDeadZoneSliderValue;

        cameraDeadZoneSliderValue = optionsValuesMainMenu.controllerCameraDeadZone;
        switch (cameraDeadZoneSliderValue)
        {
            case 0.01f:
                cameraDeadZoneSlider.value = 0;
                break;
            case 0.025f:
                cameraDeadZoneSlider.value = 1;
                break;
            case 0.05f:
                cameraDeadZoneSlider.value = 2;
                break;
            case 0.075f:
                cameraDeadZoneSlider.value = 3;
                break;
            case 0.1f:
                cameraDeadZoneSlider.value = 4;
                break;
            case 0.125f:
                cameraDeadZoneSlider.value = 5;
                break;
            case 0.15f:
                cameraDeadZoneSlider.value = 6;
                break;
            case 0.175f:
                cameraDeadZoneSlider.value = 7;
                break;
            case 0.2f:
                cameraDeadZoneSlider.value = 8;
                break;
            default:
                cameraDeadZoneSlider.value = 0;
                cameraDeadZoneSliderValue = 0.01f;
                break;
        }
        cameraDeadZoneText.text = "" + cameraDeadZoneSliderValue;

        // Camera Horizontal speed
        cameraSpeedHorizontalMultiplierSliderValue = optionsValuesMainMenu.cameraSpeedHorizontalMultiplier;
        
        if (cameraSpeedHorizontalMultiplierSliderValue == 0.5f) cameraSpeedHorizontalMultiplierSlider.value = 0;
        else if (cameraSpeedHorizontalMultiplierSliderValue < 1) cameraSpeedHorizontalMultiplierSlider.value = cameraSpeedHorizontalMultiplierSliderValue * 10 - 5;
        else cameraSpeedHorizontalMultiplierSlider.value = (cameraSpeedHorizontalMultiplierSliderValue - 1) * 4 + 5;

        cameraSpeedHorizontalMultiplierText.text = "" + cameraSpeedHorizontalMultiplierSliderValue;


        // Camera Vertical speed
        cameraSpeedVerticalMultiplierSliderValue = optionsValuesMainMenu.cameraSpeedVerticalMultiplier;

        if (cameraSpeedVerticalMultiplierSliderValue == 0.5f) cameraSpeedVerticalMultiplierSlider.value = 0;
        else if (cameraSpeedVerticalMultiplierSliderValue < 1) cameraSpeedVerticalMultiplierSlider.value = cameraSpeedVerticalMultiplierSliderValue * 10 - 5;
        else cameraSpeedVerticalMultiplierSlider.value = (cameraSpeedVerticalMultiplierSliderValue - 1) * 4 + 5;

        cameraSpeedVerticalMultiplierText.text = "" + cameraSpeedVerticalMultiplierSliderValue;


        // Invert axis
        invertYAxis.SetIsOnWithoutNotify(optionsValuesMainMenu.invertYAxis);

        invertXAxis.SetIsOnWithoutNotify(optionsValuesMainMenu.invertXAxis);

        //doubleTapReset.SetIsOnWithoutNotify(optionValues.doubleTapReset);
        //optionValues.doubleTapReset = !optionValues.doubleTapReset;


        // use3DEffects
        if (optionsValuesMainMenu.use3DEffects) use3DEffects.SetIsOnWithoutNotify(true);
        else use3DEffects.SetIsOnWithoutNotify(false);

        // Camera shakes
        if (optionsValuesMainMenu.cameraShake) cameraShakeToggle.SetIsOnWithoutNotify(true);
        else cameraShakeToggle.SetIsOnWithoutNotify(false);

        /*cameraShakePowerSliderValue = optionValuesMainMenu.cameraShakePower;
        cameraShakePowerText.text = "" + cameraShakePowerSliderValue;

        if (cameraShakePowerSlider.value == 0) cameraShakePowerSliderValue = 0.5f;
        else cameraShakePowerSliderValue = cameraShakePowerSlider.value * 0.1f + 0.5f;*/

        desiredFramerate = optionsValuesMainMenu.targetFramerate;
        //QualitySettings.vSyncCount = optionValues.vSyncCount;
        if (optionsValuesMainMenu.vSyncCount == 1) vSyncCountToggle.SetIsOnWithoutNotify(true);
        else vSyncCountToggle.SetIsOnWithoutNotify(false);

        ChooseFramerateAtStart();
        /*if (QualitySettings.vSyncCount == 1) framerateSelectionGameobject.SetActive(false);
        else framerateSelectionGameobject.SetActive(true);*/
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ChooseCharacterDeadZoneSliderValue()
    {
        switch (characterDeadZoneSlider.value)
        {
            case 0:
                characterDeadZoneSliderValue = 0.01f;
                break;
            case 1:
                characterDeadZoneSliderValue = 0.05f;
                break;
            case 2:
                characterDeadZoneSliderValue = 0.1f;
                break;
            case 3:
                characterDeadZoneSliderValue = 0.15f;
                break;
            case 4:
                characterDeadZoneSliderValue = 0.2f;
                break;
            case 5:
                characterDeadZoneSliderValue = 0.25f;
                break;
            case 6:
                characterDeadZoneSliderValue = 0.3f;
                break;
            case 7:
                characterDeadZoneSliderValue = 0.35f;
                break;
            case 8:
                characterDeadZoneSliderValue = 0.4f;
                break;
            case 9:
                characterDeadZoneSliderValue = 0.45f;
                break;
            case 10:
                characterDeadZoneSliderValue = 0.5f;
                break;
            default:
                characterDeadZoneSlider.value = 2;
                characterDeadZoneSliderValue = 0.2f;
                break;
        }
        optionsValuesMainMenu.controllerMoveDeadZone = characterDeadZoneSliderValue;
        characterDeadZoneText.text = "" + characterDeadZoneSliderValue;
    }

    public void ChooseCameraDeadZoneSliderValue()
    {
        switch (cameraDeadZoneSlider.value)
        {
            case 0:
                cameraDeadZoneSliderValue = 0.01f;
                break;
            case 1:
                cameraDeadZoneSliderValue = 0.025f;
                break;
            case 2:
                cameraDeadZoneSliderValue = 0.05f;
                break;
            case 3:
                cameraDeadZoneSliderValue = 0.075f;
                break;
            case 4:
                cameraDeadZoneSliderValue = 0.1f;
                break;
            case 5:
                cameraDeadZoneSliderValue = 0.125f;
                break;
            case 6:
                cameraDeadZoneSliderValue = 0.15f;
                break;
            case 7:
                cameraDeadZoneSliderValue = 0.175f;
                break;
            case 8:
                cameraDeadZoneSliderValue = 0.2f;
                break;
            default:
                cameraDeadZoneSlider.value = 0;
                cameraDeadZoneSliderValue = 0.01f;
                break;
        }
        optionsValuesMainMenu.controllerCameraDeadZone = cameraDeadZoneSliderValue;
        cameraDeadZoneText.text = "" + cameraDeadZoneSliderValue;
    }

    public void ChooseCameraSpeedHorizontalMultiplierSliderValue()
    {
        /*switch (cameraSpeedHorizontalMultiplierSlider.value)
        {
            case 0:
                cameraSpeedHorizontalMultiplierSliderValue = 0.5f;
                break;
            case 1:
                cameraSpeedHorizontalMultiplierSliderValue = 0.6f;
                break;
            case 2:
                cameraSpeedHorizontalMultiplierSliderValue = 0.7f;
                break;
            case 3:
                cameraSpeedHorizontalMultiplierSliderValue = 0.8f;
                break;
            case 4:
                cameraSpeedHorizontalMultiplierSliderValue = 0.9f;
                break;
            case 5:
                cameraSpeedHorizontalMultiplierSliderValue = 1;
                break;
            case 6:
                cameraSpeedHorizontalMultiplierSliderValue = 1.25f;
                break;
            case 7:
                cameraSpeedHorizontalMultiplierSliderValue = 1.5f;
                break;
            case 8:
                cameraSpeedHorizontalMultiplierSliderValue = 1.75f;
                break;
            case 9:
                cameraSpeedHorizontalMultiplierSliderValue = 2;
                break;
            case 10:
                cameraSpeedHorizontalMultiplierSliderValue = 2.25f;
                break;
            case 11:
                cameraSpeedHorizontalMultiplierSliderValue = 2.5f;
                break;
            case 12:
                cameraSpeedHorizontalMultiplierSliderValue = 2.75f;
                break;
            case 13:
                cameraSpeedHorizontalMultiplierSliderValue = 3;
                break;
            case 14:
                cameraSpeedHorizontalMultiplierSliderValue = 3.25f;
                break;
            case 15:
                cameraSpeedHorizontalMultiplierSliderValue = 3.5f;
                break;
            case 16:
                cameraSpeedHorizontalMultiplierSliderValue = 3.75f;
                break;
            case 17:
                cameraSpeedHorizontalMultiplierSliderValue = 4;
                break;
            case 18:
                cameraSpeedHorizontalMultiplierSliderValue = 4.25f;
                break;
            case 19:
                cameraSpeedHorizontalMultiplierSliderValue = 4.5f;
                break;
            case 20:
                cameraSpeedHorizontalMultiplierSliderValue = 4.75f;
                break;
            case 21:
                cameraSpeedHorizontalMultiplierSliderValue = 5;
                break;
            default:
                cameraSpeedHorizontalMultiplierSlider.value = 5;
                cameraSpeedHorizontalMultiplierSliderValue = 1;
                break;
        }*/
        //cameraSpeedHorizontalMultiplierSliderValue = cameraSpeedHorizontalMultiplierSlider.value;
        if (cameraSpeedHorizontalMultiplierSlider.value == 0) cameraSpeedHorizontalMultiplierSliderValue = 0.5f;
        else if (cameraSpeedHorizontalMultiplierSlider.value < 5) cameraSpeedHorizontalMultiplierSliderValue = cameraSpeedHorizontalMultiplierSlider.value * 0.1f + 0.5f;
        //else
        //if (cameraSpeedVerticalMultiplierSlider.value < 13) cameraSpeedHorizontalMultiplierSliderValue = 1 + (cameraSpeedHorizontalMultiplierSlider.value - 5) * 0.25f;
        else cameraSpeedHorizontalMultiplierSliderValue = 1 + (cameraSpeedHorizontalMultiplierSlider.value - 5) * 0.25f;

        //cameraSpeedHorizontalMultiplierSliderValue = Mathf.Round(cameraSpeedHorizontalMultiplierSlider.value * 10) / 10;

        optionsValuesMainMenu.cameraSpeedHorizontalMultiplier = cameraSpeedHorizontalMultiplierSliderValue;
        cameraSpeedHorizontalMultiplierText.text = "" + cameraSpeedHorizontalMultiplierSliderValue;
    }

    public void ChooseCameraSpeedVerticalMultiplierSliderValue()
    {
        /*switch (cameraSpeedVerticalMultiplierSlider.value)
        {
            case 0:
                cameraSpeedVerticalMultiplierSliderValue = 0.5f;
                break;
            case 1:
                cameraSpeedVerticalMultiplierSliderValue = 0.6f;
                break;
            case 2:
                cameraSpeedVerticalMultiplierSliderValue = 0.7f;
                break;
            case 3:
                cameraSpeedVerticalMultiplierSliderValue = 0.8f;
                break;
            case 4:
                cameraSpeedVerticalMultiplierSliderValue = 0.9f;
                break;
            case 5:
                cameraSpeedVerticalMultiplierSliderValue = 1;
                break;
            case 6:
                cameraSpeedVerticalMultiplierSliderValue = 1.25f;
                break;
            case 7:
                cameraSpeedVerticalMultiplierSliderValue = 1.5f;
                break;
            case 8:
                cameraSpeedVerticalMultiplierSliderValue = 1.75f;
                break;
            case 9:
                cameraSpeedVerticalMultiplierSliderValue = 2;
                break;
            case 10:
                cameraSpeedVerticalMultiplierSliderValue = 2.25f;
                break;
            case 11:
                cameraSpeedVerticalMultiplierSliderValue = 2.5f;
                break;
            case 12:
                cameraSpeedVerticalMultiplierSliderValue = 2.75f;
                break;
            case 13:
                cameraSpeedVerticalMultiplierSliderValue = 3;
                break;
            case 14:
                cameraSpeedVerticalMultiplierSliderValue = 3.5f;
                break;
            case 15:
                cameraSpeedVerticalMultiplierSliderValue = 4;
                break;
            case 16:
                cameraSpeedVerticalMultiplierSliderValue = 4.5f;
                break;
            case 17:
                cameraSpeedVerticalMultiplierSliderValue = 5;
                break;
            default:
                cameraSpeedVerticalMultiplierSlider.value = 5;
                cameraSpeedVerticalMultiplierSliderValue = 1;
                break;
        }*/
        //cameraSpeedVerticalMultiplierSliderValue = cameraSpeedVerticalMultiplierSlider.value;

        if (cameraSpeedVerticalMultiplierSlider.value == 0) cameraSpeedVerticalMultiplierSliderValue = 0.5f;
        else if (cameraSpeedVerticalMultiplierSlider.value < 5) cameraSpeedVerticalMultiplierSliderValue = cameraSpeedVerticalMultiplierSlider.value * 0.1f + 0.5f;
        else cameraSpeedVerticalMultiplierSliderValue = 1 + (cameraSpeedVerticalMultiplierSlider.value - 5) * 0.25f;

        optionsValuesMainMenu.cameraSpeedVerticalMultiplier = cameraSpeedVerticalMultiplierSliderValue;
        cameraSpeedVerticalMultiplierText.text = "" + cameraSpeedVerticalMultiplierSliderValue;
    }

    public void ToggleScreenShaker()
    {
        if (cameraShakeToggle.isOn) optionsValuesMainMenu.cameraShake = true;
        else optionsValuesMainMenu.cameraShake = false;
    }
    /*
    public void ChooseCharacterDeadZoneSliderValue()
    {
        optionValues.controllerMoveDeadZone = characterDeadZoneSlider;
    }*/


    public void ChooseFramerate()
    {
        switch (framerateSelection.value)
        {
            case 0:
                desiredFramerate = 30;
                break;
            case 1:
                desiredFramerate = 45;
                break;
            case 2:
                desiredFramerate = 60;
                break;
            case 3:
                desiredFramerate = 90;
                break;
            case 4:
                desiredFramerate = 120;
                break;
            case 5:
                desiredFramerate = 150;
                break;
            case 6:
                desiredFramerate = 0;
                break;
            default:
                desiredFramerate = -1;
                break;

        }

        if (optionsValuesMainMenu.vSyncCount == 1)
        {
            optionsValuesMainMenu.vSyncCount = 1;
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 0;
            optionsValuesMainMenu.targetFramerate = 0;
        }
        else
        {
            optionsValuesMainMenu.vSyncCount = 0;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = desiredFramerate;
            optionsValuesMainMenu.targetFramerate = desiredFramerate;
        }

    }

    public void ChooseFramerateAtStart()
    {
        switch (desiredFramerate)
        {
            case 30:
                framerateSelection.value = 0;
                break;
            case 45:
                framerateSelection.value = 1;
                break;
            case 60:
                framerateSelection.value = 2;
                break;
            case 90:
                framerateSelection.value = 3;
                break;
            case 120:
                framerateSelection.value = 4;
                break;
            case 150:
                framerateSelection.value = 5;
                break;
            case 0:
                framerateSelection.value = 6;
                break;
            default:
                framerateSelection.value = 6;
                break;
        }
        
        if (optionsValuesMainMenu.vSyncCount == 1)
        {
            //QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 0;
            optionsValuesMainMenu.targetFramerate = 0;
            //vSyncCountToggle.isOn = true;
            framerateSelectionGameobject.SetActive(false);
        }
        else
        {
            optionsValuesMainMenu.vSyncCount = 0;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = desiredFramerate;
            optionsValuesMainMenu.targetFramerate = desiredFramerate;
            //vSyncCountToggle.isOn = false;
            framerateSelectionGameobject.SetActive(true);
        }

    }
        
    public void ToggleVSync()
    {
        if (QualitySettings.vSyncCount == 0)
        {
            optionsValuesMainMenu.vSyncCount = 1;
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 0;
            optionsValuesMainMenu.targetFramerate = 0;
        }
        else
        {
            optionsValuesMainMenu.vSyncCount = 0;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = desiredFramerate;
            optionsValuesMainMenu.targetFramerate = desiredFramerate;
        }
    }
    
    public void InvertYAxis()
    {
        if (invertYAxis.isOn) optionsValuesMainMenu.invertYAxis = true;
        else optionsValuesMainMenu.invertYAxis = false;
        //optionValues.invertYAxis = !optionValues.invertYAxis;
    }

    public void InvertXAxis()
    {
        if (invertXAxis.isOn) optionsValuesMainMenu.invertXAxis = true;
        else optionsValuesMainMenu.invertXAxis = false;
        //optionValues.invertXAxis = !optionValues.invertXAxis;
    }

    /*public void ToggleDoubleTapCamera()
    {
        if (doubleTapReset.isOn) optionValues.doubleTapReset = true;
        else optionValues.doubleTapReset = false;
        //optionValues.doubleTapReset = !optionValues.doubleTapReset;
    }*/

    public void Toggle3DEffects()
    {
        if (use3DEffects.isOn) optionsValuesMainMenu.use3DEffects = true;
        else optionsValuesMainMenu.use3DEffects = false;
        //optionValues.doubleTapReset = !optionValues.doubleTapReset;
    }
}
