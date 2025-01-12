using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class FollowCameraRotation : MonoBehaviour
{
    private void Awake()
    {
        Application.onBeforeRender += UpdateRotation;
    }

    private void OnDestroy()
    {
        Application.onBeforeRender -= UpdateRotation;
    }

    void UpdateRotation()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }
}
