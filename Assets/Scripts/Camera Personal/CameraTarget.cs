using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private CinemachineFreeLook cinemachineFreeLook;
    CinemachineVirtualCamera[] rigs = new CinemachineVirtualCamera[3];
    private CinemachineFollowZoom cinemachineFollowZoom;

    private Transform[] originalCameraTargets = new Transform[3];
    public Transform[] newCameraTargets;

    private bool hasSwitchedTarget;

    void Start()
    {
        cinemachineFreeLook = CameraFreeLookController.Instance.cinemachineFreeLook;

        for (int i = 0; cinemachineFreeLook != null && i < 3; ++i)
        {
            rigs[i] = cinemachineFreeLook.GetRig(i);
        }

        for (int i = 0; i < rigs.Length; i++)
        {
            originalCameraTargets[i] = rigs[i].LookAt;
        }
        hasSwitchedTarget = false;
    }

    public void SwitchTarget(bool state)
    {
        if (state)
        {
            if (newCameraTargets.Length == 1)
            {
                for (int i = 0; i < rigs.Length; i++)
                {
                    cinemachineFreeLook.LookAt = newCameraTargets[0];
                }
            }
            else
            {
                for (int i = 0; i < rigs.Length; i++)
                {
                    cinemachineFreeLook.LookAt = newCameraTargets[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < rigs.Length; i++)
            {
                cinemachineFreeLook.LookAt = originalCameraTargets[i];
            }
        }
        hasSwitchedTarget = state;
    }
}
