using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Event_Cameras : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera[] eventCameras;
    private int currentActiveCamera;

    private void Start()
    {
        cinemachineBrain = ScriptLocations.Instance.cinemachineBrain;
        currentActiveCamera = eventCameras.Length;
    }

    private void ShowMainCamera()
    {
        CameraFreeLookController.Instance.cinemachineFreeLook.Priority = 20;
        foreach (CinemachineVirtualCamera camera in eventCameras)
        {
            camera.Priority = 10;
        }
    }

    public void SwitchToNextCamera()
    {
        if (currentActiveCamera == eventCameras.Length)
        {
            currentActiveCamera = 0;
        }
        else currentActiveCamera++;

        if (currentActiveCamera == eventCameras.Length)
        {
            ShowMainCamera();
        }
        else
        {
            cinemachineBrain.ActiveVirtualCamera.Priority = 10;
            for (int i = eventCameras.Length - 1; i >= 0; i--)
            {
                if (i == currentActiveCamera) eventCameras[i].Priority = 20;
                else eventCameras[i].Priority = 10;
            }
        }
    }
}