using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PhysicsBasedCharacterController;

public class GameConfiguration
{
    public bool controllerCameraSwitch;
    [Range(0.01f, 0.5f)]
    public float controllerMoveDeadZone = 0.1f;
    [Range(0.01f, 0.2f)]
    public float controllerCameraDeadZone = 0.01f;

    [Space(10)]

    [Header("VSync")]
    public int targetFramerate;
    public int vSyncCount;

    [Header("Camera Specifics")]
    [Range(0.5f, 5)]
    public float cameraSpeedHorizontalMultiplier = 1;
    [Range(0.5f, 5)]
    public float cameraSpeedVerticalMultiplier = 1;
    public bool invertYAxis;
    public bool invertXAxis;
    public bool doubleTapReset;
    public bool cameraShake;
}

public class OptionsValues : MonoBehaviour
{
    public GameConfiguration gameConfiguration;
    [Space(10)]

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
    public float cameraSpeedHorizontalMultiplier;
    [Range(0.5f, 5)]
    public float cameraSpeedVerticalMultiplier;
    public bool invertYAxis;
    public bool invertXAxis;
    public bool doubleTapReset;
    public bool cameraShake;
    public bool visibleAimIcon;
    public bool visibleInterface;
    [Space(10)]
    public bool fullScreen;
    public string resolution;
    [Space(10)]
    public float music;
    public float sfx;
    public float voices;
    [Space(10)]
    public bool automaticSave;

    /*[Range(0, 2)]
    public float cameraShakePower = 1;*/
    //private ThirdPersonCamera.CameraInputSampling_FreeForm cameraInputScript;
    [HideInInspector] public CharacterManager characterManager;
    [HideInInspector] public CameraFreeLookController cameraFreeLookController;
    [HideInInspector] public CinemachineFreeLook cinemachineFreeLook;
    //private CameraManager cameraManagerScript;

    void Awake()
    {
        // refs to other scripts are in ScriptLocations

        LoadPlayerOptions();
        UpdateAllOptions();

        Cursor.lockState = CursorLockMode.Locked;
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);

        //cameraManagerScript.targetFrameRate = targetFramerate;
        QualitySettings.vSyncCount = vSyncCount;
        if (vSyncCount == 1) targetFramerate = 0;
        Application.targetFrameRate = targetFramerate;
    }

    public void OnSceneChange()
    {
        //LoadConfiguration();
        LoadPlayerOptions();
        UpdateAllOptions();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*characterManagerScript.stickInclineDeadZone = controllerMoveDeadZone;
        cameraManagerScript.stickInclineDeadZone = controllerCameraDeadZone;
        cameraManagerScript.cameraSpeedMultiplier = cameraSpeedMultiplier;*/

    }

    public void UpdateAllOptions()
    {
        cameraFreeLookController.stickInclineDeadZone = controllerCameraDeadZone;
        cameraFreeLookController.cameraSpeedHorizontalMultiplier = cameraSpeedHorizontalMultiplier;
        cameraFreeLookController.cameraSpeedVerticalMultiplier = cameraSpeedVerticalMultiplier;

        cinemachineFreeLook.m_YAxis.m_InvertInput = invertYAxis;
        cinemachineFreeLook.m_XAxis.m_InvertInput = invertXAxis;

        characterManager.stickInclineDeadZone = controllerMoveDeadZone;
        characterManager.cameraShake = cameraShake;
    }

    public void SaveConfiguration()
    {
        string jsonConfiguration = JsonUtility.ToJson(gameConfiguration);

        PlayerPrefs.SetString("Configuration", jsonConfiguration);

        PlayerPrefs.Save();
    }

    public void LoadConfiguration()
    {
        string jsonConfiguration = PlayerPrefs.GetString("Configuration", "");

        if (!string.IsNullOrEmpty(jsonConfiguration))
        {
            JsonUtility.FromJsonOverwrite(jsonConfiguration, this);
        }

        controllerCameraSwitch = gameConfiguration.controllerCameraSwitch;
        controllerMoveDeadZone = gameConfiguration.controllerMoveDeadZone;
        controllerCameraDeadZone = gameConfiguration.controllerCameraDeadZone;
        targetFramerate = gameConfiguration.targetFramerate;
        vSyncCount = gameConfiguration.vSyncCount;
        cameraSpeedHorizontalMultiplier = gameConfiguration.cameraSpeedHorizontalMultiplier;
        cameraSpeedVerticalMultiplier = gameConfiguration.cameraSpeedVerticalMultiplier;
        invertYAxis = gameConfiguration.invertYAxis;
        invertXAxis = gameConfiguration.invertXAxis;
        doubleTapReset = gameConfiguration.doubleTapReset;
        cameraShake = gameConfiguration.cameraShake;
    }


    public void SavePlayerOptions()
    {
        PlayerPrefs.SetFloat("controllerMovementDeadzone", controllerMoveDeadZone);
        PlayerPrefs.SetFloat("controllerCameraDeadzone", controllerCameraDeadZone);
        PlayerPrefs.SetFloat("cameraHorizontalSpeed", cameraSpeedHorizontalMultiplier);
        PlayerPrefs.SetFloat("cameraVerticalSpeed", cameraSpeedVerticalMultiplier);
        PlayerPrefs.SetInt("invertXAxis", invertXAxis ? 1 : 0);
        PlayerPrefs.SetInt("invertYAxis", invertYAxis ? 1 : 1);
        PlayerPrefs.SetInt("cameraShake", cameraShake ? 1 : 0);
        PlayerPrefs.SetInt("visibleAimIcon", visibleAimIcon ? 1 : 0);
        PlayerPrefs.SetInt("visibleInterface", visibleInterface ? 1 : 0);
        PlayerPrefs.SetInt("fullScreen", fullScreen ? 1 : 0);
        PlayerPrefs.SetString("resolution", resolution);
        PlayerPrefs.SetInt("vSyncCount", vSyncCount);
        PlayerPrefs.SetInt("targetFramerate", targetFramerate);

        /*
        for (int i = 0, i < controllerMove.length, i++)
        {
            PlayerPrefs.SetString("controllerMove" + i, controllerMove[i]);
            PlayerPrefs.SetString("controllerCamera" + i, controllerCamera[i]);
            PlayerPrefs.SetString("controllerReserve" + i, controllerReserve[i]);
        }

        for (int i = 0, i < keyboardMoveUp.length, i++)
        {
            PlayerPrefs.SetString("keyboardMoveUp" + i, keyboardMoveUp[i]);
            PlayerPrefs.SetString("keyboardMoveLeft" + i, keyboardMoveLeft[i]);
            PlayerPrefs.SetString("keyboardMoveRight" + i, keyboardMoveRight[i]);
            PlayerPrefs.SetString("keyboardMoveDown" + i, keyboardMoveDown[i]);

            PlayerPrefs.SetString("keyboardReserveUp" + i, keyboardReserveUp[i]);
            PlayerPrefs.SetString("keyboardReserveLeft" + i, keyboardReserveLeft[i]);
            PlayerPrefs.SetString("keyboardReserveRight" + i, keyboardReserveRight[i]);
            PlayerPrefs.SetString("keyboardReserveDown" + i, keyboardReserveDown[i]);
        }

        for (int i = 0, i < jump.length, i++)
        {

            PlayerPrefs.SetString("jump" + i, jump[i]);
            PlayerPrefs.SetString("grab" + i, grab[i]);
            PlayerPrefs.SetString("kick" + i, kick[i]);
            PlayerPrefs.SetString("maskPower" + i, maskPower[i]);
            PlayerPrefs.SetString("dive" + i, dive[i]);
            PlayerPrefs.SetString("resetCamera" + i, resetCamera[i]);
            PlayerPrefs.SetString("pause" + i, pause[i]);
            PlayerPrefs.SetString("map" + i, map[i]);
            PlayerPrefs.SetString("validate" + i, validate[i]);
            PlayerPrefs.SetString("cancel" + i, cancel[i]);
        }*/

        PlayerPrefs.SetFloat("music", music);
        PlayerPrefs.SetFloat("sfx", sfx);
        PlayerPrefs.SetFloat("voices", voices);

        PlayerPrefs.SetInt("automaticSave", automaticSave ? 1 : 0);

        PlayerPrefs.Save();
    }


    public void LoadPlayerOptions()
    {
        controllerMoveDeadZone = PlayerPrefs.GetFloat("controllerMovementDeadzone", 0.2f);
        controllerCameraDeadZone = PlayerPrefs.GetFloat("controllerCameraDeadzone", 0.01f);
        cameraSpeedHorizontalMultiplier = PlayerPrefs.GetFloat("cameraHorizontalSpeed", 1);
        cameraSpeedVerticalMultiplier = PlayerPrefs.GetFloat("cameraVerticalSpeed", 1);
        invertXAxis = PlayerPrefs.GetInt("invertXAxis", 0) == 1;
        invertYAxis = PlayerPrefs.GetInt("invertYAxis", 1) == 1;
        cameraShake = PlayerPrefs.GetInt("cameraShake", 1) == 1;

        visibleAimIcon = PlayerPrefs.GetInt("visibleAimIcon", 1) == 1;
        visibleInterface = PlayerPrefs.GetInt("visibleInterface", 1) == 1;
        fullScreen = PlayerPrefs.GetInt("fullScreen", 1) == 1;
        resolution = PlayerPrefs.GetString("resolution", "1920 x 1080");
        vSyncCount = PlayerPrefs.GetInt("vSyncCount", 1);
        targetFramerate = PlayerPrefs.GetInt("targetFramerate", 60);

        /*
        for (int i = 0, i < controllerMove.length, i++)
        {
            controllerMove[i] = PlayerPrefs.GetString("controllerMove" + i, "LeftStick");
            controllerCamera[i] = PlayerPrefs.GetString("controllerCamera" + i, "RightStick");
            controllerReserve[i] = PlayerPrefs.GetString("controllerReserve" + i, "ArrowKeys");
        }

        for (int i = 0, i < keyboardMoveUp.length, i++)
        {
            keyboardMoveUp[i] = PlayerPrefs.GetString("keyboardMoveUp" + i, "a");
            keyboardMoveLeft[i] = PlayerPrefs.GetString("keyboardMoveLeft" + i, "w");
            keyboardMoveRight[i] = PlayerPrefs.GetString("keyboardMoveRight" + i, "d");
            keyboardMoveDown[i] = PlayerPrefs.GetString("keyboardMoveDown" + i, "s");

            keyboardReserveUp[i] = PlayerPrefs.GetString("keyboardReserveUp" + i, "upArrow");
            keyboardReserveLeft[i] = PlayerPrefs.GetString("keyboardReserveLeft" + i, "leftArrow");
            keyboardReserveRight[i] = PlayerPrefs.GetString("keyboardReserveRight" + i, "rightArrow");
            keyboardReserveDown[i] = PlayerPrefs.GetString("keyboardReserveDown" + i, "downArrow");
        }

        for (int i = 0, i < jump.length, i++)
        {
            jump[i] = PlayerPrefs.GetString("jump" + i, "A");
            grab[i] = PlayerPrefs.GetString("grab" + i, "B");
            kick[i] = PlayerPrefs.GetString("kick" + i, "L R");
            maskPower[i] = PlayerPrefs.GetString("maskPower" + i, "X");
            dive[i] = PlayerPrefs.GetString("dive" + i, "ZL ZR");
            resetCamera[i] = PlayerPrefs.GetString("resetCamera" + i, "Y");
            pause[i] = PlayerPrefs.GetString("pause" + i, "start");
            map[i] = PlayerPrefs.GetString("map" + i, "select");
            validate[i] = PlayerPrefs.GetString("validate" + i, "A");
            cancel[i] = PlayerPrefs.GetString("cancel" + i, "B");
        }*/

        music = PlayerPrefs.GetFloat("music", 0.7f);
        sfx = PlayerPrefs.GetFloat("sfx", 0.7f);
        voices = PlayerPrefs.GetFloat("voices", 0.7f);

        automaticSave = PlayerPrefs.GetInt("automaticSave", 1) == 1;
    }
}
