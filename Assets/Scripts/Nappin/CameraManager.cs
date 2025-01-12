using UnityEngine;

//DISABLE if using old input system
using UnityEngine.InputSystem;


public class CameraManager : MonoBehaviour
{
    [Header("Camera properties")]
    public GameObject thirdPersonCamera;
    public GameObject chargeCamera;
    public GameObject aerialCamera;
    public GameObject aerialChargeCamera;
    public Camera mainCamera;
    public Camera gotPhotoCamera;
    public CharacterManager characterManager;
    [Space(10)]

    public LayerMask thirdPersonMask;
    public LayerMask chargeMask;
    public LayerMask aerialMask;
    public LayerMask aerialChargeMask;
    [Space(10)]

    public bool activeThirdPerson = true;
    public bool activeDebug = true;

    public int targetFrameRate;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);

        Application.targetFrameRate = targetFrameRate;
        characterManager = CharacterManager.Instance;
    }

    /**/


    private void Awake()
    {
        SetCamera();
        SetDebug();
    }


    private void FixedUpdate()
    {
        //DISABLE if using old input system
        /*
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            activeThirdPerson = !activeThirdPerson;
            SetCamera();
        }

        //DISABLE if using old input system
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            SetDebug();
        }*/

        //ENABLE if using old input system

        Screen.fullScreen = true;


        if (Input.GetKeyDown(KeyCode.M))
        {
            activeThirdPerson = !activeThirdPerson;
            SetCamera();
        }/*

        if (Input.GetKeyDown(KeyCode.N))
        {
            SetDebug();
        }

        */

        /*if (characterManager.isGrounded)
        {
            /*if (characterManager.isChargingFireBall)
            {
                chargeCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);

                mainCamera.cullingMask = chargeMask;
            }
            else
            {
                thirdPersonCamera.SetActive(true);
                chargeCamera.SetActive(false);

                mainCamera.cullingMask = thirdPersonMask;
            //}
            aerialCamera.SetActive(false);
            aerialChargeCamera.SetActive(false);
        }
        else
        {
            /*if (characterManager.isChargingFireBall)
            {
                aerialChargeCamera.SetActive(true);
                aerialCamera.SetActive(false);

                mainCamera.cullingMask = aerialChargeMask;
            }
            else
            {
                aerialCamera.SetActive(true);
                aerialChargeCamera.SetActive(false);

                mainCamera.cullingMask = aerialMask;
            //}
            thirdPersonCamera.SetActive(false);
            chargeCamera.SetActive(false);
        }*/
    }

    public void SetCamera()
    {
        if (activeThirdPerson)
        {
            chargeCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
            mainCamera.cullingMask = thirdPersonMask;
            gotPhotoCamera.cullingMask = thirdPersonMask;
        }
        else
        {
            chargeCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);

            mainCamera.cullingMask = chargeMask;
            gotPhotoCamera.cullingMask = chargeMask;
        }
        activeThirdPerson = !activeThirdPerson;
    }


    public void SetDebug()
    {
        characterManager.debug = !characterManager.debug;
    }
}