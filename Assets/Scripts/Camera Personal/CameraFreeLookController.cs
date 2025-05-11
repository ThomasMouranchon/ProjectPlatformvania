using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFreeLookController : MonoBehaviour
{
    private static CameraFreeLookController instance = null;
    public static CameraFreeLookController Instance => instance;

    public CinemachineFreeLook cinemachineFreeLook;
    public CinemachineCollider cinemachineCollider;
    public CinemachineInputProvider cinemachineInputProvider;
    [Space(10)]

    public Transform playerObject;
    public Transform cameraTarget;
    [Space(10)]

    public float verticalStandardSpeed;
    public float horizontalStandardSpeed;
    private float verticalControllerSpeed;
    private float horizontalControllerSpeed;
    private float verticalMouseSpeed;
    private float horizontalMouseSpeed;
    [Space(10)]

    [Range(0.5f, 3)]
    public float cameraSpeedHorizontalMultiplier = 1;
    [Range(0.5f, 3)]
    public float cameraSpeedVerticalMultiplier = 1;
    private float stickIncline;
    [Range(0.01f, 0.2f)]
    public float stickInclineDeadZone;

    public Animator fixedCameraIconAnim;
    public bool fixedCamera;

    private InputReader inputReader;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = horizontalStandardSpeed * cameraSpeedHorizontalMultiplier;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = verticalStandardSpeed * cameraSpeedVerticalMultiplier;

        inputReader = InputReader.Instance;
        //stickInclineDeadZone = optionsValuesScript.controllerCameraDeadZone;

        /*horizontalCurrentSpeed = horizontalStandardSpeed;
        verticalCurrentSpeed = verticalStandardSpeed;*/

        //cinemachineFreeLook.ForceCameraPosition(playerObject.position, new Quaternion(0,0,0,0));

        //cinemachineFreeLook.m_XAxis.Value = playerObject.rotation.y;
        cinemachineFreeLook.transform.rotation = Quaternion.identity;
        //cinemachineFreeLookGameObject.transform.rotation = Quaternion.EulerAngles(0, 0, 0);
        cinemachineFreeLook.m_YAxis.Value = 0.5f;
        //ResetCoroutine();

        horizontalControllerSpeed = horizontalStandardSpeed;
        verticalControllerSpeed = verticalStandardSpeed;
        horizontalMouseSpeed = horizontalStandardSpeed;
        verticalMouseSpeed = verticalStandardSpeed;

        SwitchFixedCamera(false, true);
    }
    private void FixedUpdate()
    {
        if (Application.targetFrameRate < 60 && Application.targetFrameRate > 0)
        {
            CameraInput();
        }
    }

    private void LateUpdate()
    {
        if (Application.targetFrameRate <= 0 | Application.targetFrameRate >= 60)
        {
            CameraInput();
        }
    }

    public void CameraInput()
    {
        if (inputReader.enableCameraInput && !fixedCamera)
        {
            stickIncline = Mathf.Abs(inputReader.cameraInput.y) + Mathf.Abs(inputReader.cameraInput.x);

            if (inputReader.enableCameraInput)
            {
                cinemachineInputProvider.enabled = true;
                if (inputReader.isMouseAndKeyboard)
                {
                    horizontalStandardSpeed = horizontalMouseSpeed;
                    verticalStandardSpeed = verticalMouseSpeed;

                    if (QualitySettings.vSyncCount == 0)
                    {
                        cinemachineFreeLook.m_XAxis.m_AccelTime = 0.2f;
                        cinemachineFreeLook.m_YAxis.m_AccelTime = 0.1f;
                    }
                    else
                    {
                        cinemachineFreeLook.m_XAxis.m_AccelTime = 0.3f;
                        cinemachineFreeLook.m_YAxis.m_AccelTime = 0.15f;
                    }
                }
                else
                {
                    cinemachineFreeLook.m_XAxis.m_AccelTime = 0.3f;
                    cinemachineFreeLook.m_YAxis.m_AccelTime = 0.15f;

                    if (stickIncline <= stickInclineDeadZone)
                    {
                        horizontalStandardSpeed = 0;
                        horizontalStandardSpeed = 0;
                    }
                    else if (stickIncline > stickInclineDeadZone && stickIncline < 0.35f)
                    {
                        horizontalStandardSpeed = horizontalControllerSpeed / 4;
                        verticalStandardSpeed = verticalControllerSpeed / 4;
                    }
                    else
                    {
                        horizontalStandardSpeed = horizontalControllerSpeed;
                        verticalStandardSpeed = verticalControllerSpeed;
                    }
                }
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = horizontalStandardSpeed * cameraSpeedHorizontalMultiplier;
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = verticalStandardSpeed * cameraSpeedVerticalMultiplier;
            }
            else
            {
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
                cinemachineInputProvider.enabled = false;
            }
        }
        else
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            cinemachineInputProvider.enabled = false;
        }
    }

    public void SwitchFixedCamera(bool show, bool cancelable)
    {
        fixedCamera = show;
        AnimatorStateInfo currentAnimation = fixedCameraIconAnim.GetCurrentAnimatorStateInfo(0);
        if (show && !currentAnimation.IsName("FixedCameraIcon_ShowIdle"))
        {
            fixedCameraIconAnim.CrossFade("FixedCameraIcon_Appear", 0);
        }
        else if (!currentAnimation.IsName("FixedCameraIcon_HideIdle"))
        {
            fixedCameraIconAnim.CrossFade("FixedCameraIcon_Disappear", 0);
        }

        fixedCameraIconAnim.SetBool("cancelable", cancelable);
    }

    public void SwitchUIFixedCamera(bool show)
    {
        AnimatorStateInfo currentAnimation = fixedCameraIconAnim.GetCurrentAnimatorStateInfo(0);
        if (show && !currentAnimation.IsName("FixedCameraIcon_ShowIdle"))
        {
            fixedCameraIconAnim.CrossFade("FixedCameraIcon_Appear", 0);
        }
        else if (!show && !currentAnimation.IsName("FixedCameraIcon_HideIdle"))
        {
            fixedCameraIconAnim.CrossFade("FixedCameraIcon_Disappear", 0);
        }
    }
}
