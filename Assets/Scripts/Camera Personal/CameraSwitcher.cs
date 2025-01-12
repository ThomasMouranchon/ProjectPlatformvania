using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    private static CameraSwitcher instance = null;
    public static CameraSwitcher Instance => instance;

    public CinemachineFreeLook cinemachineFreeLook;
    CinemachineComposer comp0;
    CinemachineComposer comp1;
    CinemachineComposer comp2;
    //public float lerpTime = 1;

    private float[] initialComposerRadius = new float[3];
    private float[] initialComposerHeight = new float[3];

    private float[] radiusVelocities = new float[3];
    private float[] heightVelocities = new float[3];

    public float resetSpeedRadius, resetSpeedHeight;
    public bool fixedZoom;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();//.GetCinemachineComponent<CinemachineFramingTransposer>()
        comp0 = cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
        comp1 = cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
        comp2 = cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineComposer>();

        for (int i = 0; i < 3; i++)
        {
            initialComposerHeight[i] = cinemachineFreeLook.m_Orbits[i].m_Height;
        }

        for (int i = 0; i < 3; i++)
        {
            initialComposerRadius[i] = cinemachineFreeLook.m_Orbits[i].m_Radius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(cinemachineFreeLook.m_Orbits[1].m_Radius - initialComposerRadius[1]) > 0.1f && !fixedZoom)
        {
            InputReader.Instance.enableCameraInput = false;
        }
        else
        {
            InputReader.Instance.enableCameraInput = true;
        }
    }

    public void ChangeCamera(bool cameraTPSisActive)
    {
        if (!CameraFreeLookController.Instance.fixedCamera)
        {
            if (cameraTPSisActive)
            {
                comp0.m_TrackedObjectOffset.x = 2;
                comp0.m_TrackedObjectOffset.y = 0;
                cinemachineFreeLook.m_Orbits[0].m_Radius = 4;
                comp1.m_TrackedObjectOffset.x = 2;

                cinemachineFreeLook.m_Orbits[1].m_Radius = 8;
                comp2.m_TrackedObjectOffset.x = 1.5f;
                comp2.m_TrackedObjectOffset.y = 3;

                cinemachineFreeLook.m_Orbits[2].m_Radius = 2;
            }
            else
            {
                comp0.m_TrackedObjectOffset.x = 0;
                comp0.m_TrackedObjectOffset.y = 3;
                //cinemachineFreeLook.m_Orbits[0].m_Radius = 1;

                comp1.m_TrackedObjectOffset.x = 0;
                //cinemachineFreeLook.m_Orbits[1].m_Radius = 15;

                comp2.m_TrackedObjectOffset.x = 0;
                comp2.m_TrackedObjectOffset.y = 3;
                //cinemachineFreeLook.m_Orbits[2].m_Radius = 1;

                if (Mathf.Abs(cinemachineFreeLook.m_Orbits[1].m_Radius - initialComposerRadius[1]) > 0.01f)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cinemachineFreeLook.m_Orbits[i].m_Radius = Mathf.SmoothDamp(
                            cinemachineFreeLook.m_Orbits[i].m_Radius,
                            initialComposerRadius[i],
                            ref radiusVelocities[i],
                            resetSpeedRadius
                        );
                    }
                    InputReader.Instance.enableCameraInput = false;
                }
                else
                {
                    InputReader.Instance.enableCameraInput = true;
                }

                if (Mathf.Abs(cinemachineFreeLook.m_Orbits[1].m_Height - initialComposerHeight[1]) > 0.01f)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cinemachineFreeLook.m_Orbits[i].m_Height = Mathf.SmoothDamp(
                            cinemachineFreeLook.m_Orbits[i].m_Height,
                            initialComposerHeight[i],
                            ref heightVelocities[i],
                            resetSpeedHeight
                        );
                    }
                }
            }
        }
    }
}
