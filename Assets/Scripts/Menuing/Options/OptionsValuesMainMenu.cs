using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PhysicsBasedCharacterController;

public class OptionsValuesMainMenu : MonoBehaviour
{
    [Header("Keyboard & Mouse")]
    [Space(5)]
    public bool keyboardJump;
    public bool keyboardKick;
    public bool keyboardMaskPower;
    public bool keyboardCameraSwitch;

    [Space(10)]

    [Header("Controller")]
    [Space(5)]
    public bool controllerJump;
    public bool controllerKick;
    public bool controllerMaskPower;
    public bool controllerCameraSwitch;
    [Range(0.01f, 0.5f)]
    public float controllerMoveDeadZone = 0.1f;
    [Range(0.01f, 0.2f)]
    public float controllerCameraDeadZone = 0.01f;

    [Space(10)]

    [Header("VSync")]
    [Space(5)]
    public int targetFramerate;
    public int vSyncCount;

    [Header("Camera Specifics")]
    [Space(5)]
    [Range(0.5f, 5)]
    public float cameraSpeedHorizontalMultiplier = 1;
    [Range(0.5f, 5)]
    public float cameraSpeedVerticalMultiplier = 1;
    public bool invertYAxis;
    public bool invertXAxis;
    public bool doubleTapReset;
    public bool use3DEffects;
    public bool cameraShake;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);

        QualitySettings.vSyncCount = vSyncCount;
        if (vSyncCount == 1) targetFramerate = 0;
        Application.targetFrameRate = targetFramerate;
    }
}
