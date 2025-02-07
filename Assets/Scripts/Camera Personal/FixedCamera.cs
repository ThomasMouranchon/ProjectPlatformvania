using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    private bool finishInit = false;

    private bool triggerBased;

    public bool cancelable = true, resetOnDisable = true;
    [Space(30)]

    public float xAxisValue;
    public float transitionTimeX = 0.2f;
    private float velocityX;
    [Space(30)]

    public float yAxisValue;
    public float transitionTimeY = 0.2f;
    private float velocityY;
    [Space(30)]

    public bool changeRadius;
    private float[] initialComposerRadius = new float[3];
    public float[] customComposerRadius = new float[3];
    private float[] radiusVelocities = new float[3];
    [Space(30)]

    public bool changeHeight;
    private float[] initialComposerHeight = new float[3];
    public float[] customComposerHeight = new float[3];
    private float[] heightVelocities = new float[3];

    private bool cameraReset;
    public float transitionSpeedX2;
    public float transitionSpeedY2;

    [HideInInspector] public CameraFreeLookController cameraFreeLookController;
    private CinemachineFreeLook cinemachineFreeLook;
    private InputReader inputReader;

    private CameraTarget cameraTarget;
    private bool hasCameraTarget;

    private void Awake()
    {
        finishInit = false;
    }

    void Start()
    {
        if (!finishInit)
        {
            cameraFreeLookController = CameraFreeLookController.Instance;
            inputReader = InputReader.Instance;

            if (gameObject.GetComponent<Collider>() == null || !gameObject.GetComponent<Collider>().isTrigger)
            {
                triggerBased = false;

                cameraTarget = GetComponent<CameraTarget>();
            }
            else triggerBased = true;

            finishInit = true;
        }
    }

    void StartAgain()
    {
        if (finishInit)
        {
            cameraFreeLookController = CameraFreeLookController.Instance;
            inputReader = InputReader.Instance;

            if (gameObject.GetComponent<Collider>() == null || !gameObject.GetComponent<Collider>().isTrigger)
            {
                triggerBased = false;

                cameraTarget = GetComponent<CameraTarget>();
                hasCameraTarget = cameraTarget;
                if (hasCameraTarget)
                {
                    changeRadius = true;
                    changeHeight = true;
                    cameraTarget.SwitchTarget(true);
                }
            }
            else
            {
                triggerBased = true;
                if (!cancelable) inputReader.enableCameraSwitch = false;
                cameraFreeLookController.fixedCamera = true;
            }

            cameraReset = false;
        }
    }

    private void StartEffects()
    {
        cameraFreeLookController = CameraFreeLookController.Instance;
        inputReader = InputReader.Instance;

        if (gameObject.GetComponent<Collider>() == null || !gameObject.GetComponent<Collider>().isTrigger)
        {
            triggerBased = false;

            cameraTarget = GetComponent<CameraTarget>();
            hasCameraTarget = cameraTarget;
            if (hasCameraTarget)
            {
                changeRadius = true;
                changeHeight = true;
                cameraTarget.SwitchTarget(true);
            }
        }
        else
        {
            triggerBased = true;
            if (!cancelable) inputReader.enableCameraSwitch = false;
            cameraFreeLookController.fixedCamera = true;
        }

        cameraReset = false;
    }

    void Update()
    {
        if (cancelable && !cameraReset) cameraReset = (inputReader.cameraSwitch | CameraSwitcher.Instance.fixedZoom);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggerBased)
        {
            if (other.tag.Equals("Player"))
            {
                cameraReset = false;
                if (!cancelable) inputReader.enableCameraSwitch = false;
                /*if (fixedXPosition | fixedYPosition)*/ cameraFreeLookController.SwitchFixedCamera(true, cancelable);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (triggerBased)
        {
            if (other.tag.Equals("Player")) StartCoroutine(FixedUpdateCoroutine());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerBased)
        {
            if (other.tag.Equals("Player"))
            {
                inputReader.enableCameraInput = true;
                if (!cancelable) inputReader.enableCameraSwitch = true;
                cameraFreeLookController.SwitchFixedCamera(false, cancelable);
            }
        }
    }

    IEnumerator FixedUpdateCoroutine()
    {
        yield return new WaitForFixedUpdate();
        FixCamera();
    }

    void FixedUpdate()
    {
        if (!triggerBased) FixCamera();
    }

    public void FixCamera()
    {
        if (!cinemachineFreeLook)
        {
            cinemachineFreeLook = cameraFreeLookController.cinemachineFreeLook;

            for (int i = 0; i < 3; i++)
            {
                initialComposerHeight[i] = cinemachineFreeLook.m_Orbits[i].m_Height;
            }

            for (int i = 0; i < 3; i++)
            {
                initialComposerRadius[i] = cinemachineFreeLook.m_Orbits[i].m_Radius;
            }
        }

        if (cameraReset)
        {/*
            if (changeRadius)
            {
                for (int i = 0; i < 3; i++)
                {
                    cinemachineFreeLook.m_Orbits[i].m_Radius = Mathf.SmoothDamp(
                        cinemachineFreeLook.m_Orbits[i].m_Radius,
                        initialComposerRadius[i],
                        ref radiusVelocities[i],
                        transitionSpeedX2
                    );
                }
            }
            if (changeHeight)
            {
                for (int i = 0; i < 3; i++)
                {
                    cinemachineFreeLook.m_Orbits[i].m_Height = Mathf.SmoothDamp(
                        cinemachineFreeLook.m_Orbits[i].m_Height,
                        initialComposerHeight[i],
                        ref heightVelocities[i],
                        transitionSpeedY2
                    );
                }
            }*/

            if (!cancelable) inputReader.enableCameraSwitch = true;
            if (triggerBased) cameraFreeLookController.SwitchFixedCamera(false, cancelable);
            else cameraFreeLookController.fixedCamera = false;
            //CameraSwitcher.Instance.ChangeCamera(false);
        }
        else
        {
            float NormalizeAngle(float angle)
            {
                angle = (angle + 180) % 360 - 180;
                return angle;
            }
            float currentAngle = NormalizeAngle(cinemachineFreeLook.m_XAxis.Value);
            float targetAngle = NormalizeAngle(xAxisValue);

            if (Mathf.Abs(cinemachineFreeLook.m_XAxis.Value - xAxisValue) > 0.01f)
            {
                currentAngle = Mathf.SmoothDampAngle(
                    currentAngle,
                    targetAngle,
                    ref velocityX,
                    transitionTimeX
                );
                cinemachineFreeLook.m_XAxis.Value = currentAngle;
            }

            if (Mathf.Abs(cinemachineFreeLook.m_YAxis.Value - yAxisValue) > 0.01f)
            {
                cinemachineFreeLook.m_YAxis.Value = Mathf.SmoothDamp(
                    cinemachineFreeLook.m_YAxis.Value,
                    yAxisValue,
                    ref velocityY,
                    transitionTimeY
                );
            }

            if (changeRadius)
            {
                for (int i = 0; i < 3; i++)
                {
                    cinemachineFreeLook.m_Orbits[i].m_Radius = Mathf.SmoothDamp(
                        cinemachineFreeLook.m_Orbits[i].m_Radius,
                        customComposerRadius[i],
                        ref radiusVelocities[i],
                        transitionSpeedX2
                    );
                }
            }
            if (changeHeight)
            {
                for (int i = 0; i < 3; i++)
                {
                    cinemachineFreeLook.m_Orbits[i].m_Height = Mathf.SmoothDamp(
                        cinemachineFreeLook.m_Orbits[i].m_Height,
                        customComposerHeight[i],
                        ref heightVelocities[i],
                        transitionSpeedY2
                    );
                }
            }
        }
    }
    private void OnEnable()
    {
        StartAgain();
    }

    private void OnDisable()
    {
        if (finishInit)
        {
            if (!cancelable) inputReader.enableCameraSwitch = true;
            cameraFreeLookController.fixedCamera = false;
            cameraFreeLookController.SwitchUIFixedCamera(false);
            if (hasCameraTarget) cameraTarget.SwitchTarget(false);
            //CameraSwitcher.Instance.ChangeCamera(false);

            velocityX = 0;
            velocityY = 0;
            for (int i = 0; i < 3; i++)
            {
                radiusVelocities[i] = 0;
                heightVelocities[i] = 0;
            }
        }
    }
}