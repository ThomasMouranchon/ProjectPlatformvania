using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class ZoomHandler : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    CinemachineComposer[] composers = new CinemachineComposer[3];
    CinemachineVirtualCamera[] rigs = new CinemachineVirtualCamera[3];
    private CinemachineFollowZoom cinemachineFollowZoom;
    private float initialMinZoom, initialMaxZoom;

    private float[] initialComposerHeight = new float[3];
    private float[] initialComposerRadius = new float[3];
    public float[] customComposerHeight;
    public float[] customComposerRadius;
    public int customFov;
    public bool fixedHeight;

    private float velocity;

    private bool isZooming;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; cinemachineFreeLook != null && i < 3; ++i)
        {
            rigs[i] = cinemachineFreeLook.GetRig(i);
            composers[i] = rigs[i].GetCinemachineComponent<CinemachineComposer>();
        }

        for (int i = 0; i < composers.Length; i++)
        {
            initialComposerHeight[i] = cinemachineFreeLook.m_Orbits[i].m_Height;
        }

        for (int i = 0; i < composers.Length; i++)
        {
            initialComposerRadius[i] = cinemachineFreeLook.m_Orbits[i].m_Radius;
        }

        cinemachineFollowZoom = cinemachineFreeLook.GetComponent<CinemachineFollowZoom>();

        initialMinZoom = cinemachineFollowZoom.m_MinFOV;
        initialMaxZoom = cinemachineFollowZoom.m_MaxFOV;
        isZooming = false;
    }

    public void Zoom(bool state, bool force)
    {
        if (InputReader.Instance.enableCameraInput | force)
        {
            if (state)
            {
                StartCoroutine(ZoomCoroutine());
            }
            else
            {
                StartCoroutine(UnzoomCoroutine());
            }
        }
    }

    IEnumerator ZoomCoroutine()
    {
        isZooming = true;
        CameraSwitcher.Instance.fixedZoom = true;

        for (int i = 0; i < composers.Length; i++)
        {
            cinemachineFreeLook.m_Orbits[i].m_Height = customComposerHeight[i];
        }

        for (int i = 0; i < composers.Length; i++)
        {
            cinemachineFreeLook.m_Orbits[i].m_Radius = customComposerRadius[i];
        }

        if (fixedHeight)
        {
            cinemachineFreeLook.m_YAxis.Value = customComposerHeight[1];
        }

        while (cinemachineFollowZoom.m_MaxFOV > customFov + 5)
        {
            cinemachineFollowZoom.m_MaxFOV-= 2;
            cinemachineFollowZoom.m_MinFOV -= 2;
            yield return new WaitForFixedUpdate();
        }

        while (cinemachineFollowZoom.m_MinFOV > customFov - 5)
        {
            cinemachineFollowZoom.m_MinFOV -= 2;
            yield return new WaitForFixedUpdate();
        }

        isZooming = false;
        yield return null;
    }


    IEnumerator UnzoomCoroutine()
    {
        for (int i = 0; i < composers.Length; i++)
        {
            cinemachineFreeLook.m_Orbits[i].m_Height = initialComposerHeight[i];
        }

        for (int i = 0; i < composers.Length; i++)
        {
            cinemachineFreeLook.m_Orbits[i].m_Radius = initialComposerRadius[i];
        }

        while (cinemachineFollowZoom.m_MaxFOV < initialMaxZoom && !isZooming)
        {
            cinemachineFollowZoom.m_MinFOV += 2;
            cinemachineFollowZoom.m_MaxFOV += 2;
            yield return new WaitForFixedUpdate();
        }

        if (!isZooming)
        {
            cinemachineFollowZoom.m_MinFOV = initialMinZoom;
            cinemachineFollowZoom.m_MaxFOV = initialMaxZoom;
        }

        CameraSwitcher.Instance.fixedZoom = false;
        yield return null;
    }
}
