using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraResetScript : MonoBehaviour
{
    private Vector3 initialPosition;
    private CharacterManager characterManager;
    private CameraFreeLookController cameraController;
    public CinemachineFreeLook cinemachineFreeLook;
    public CameraRecenter simpleFollowRecenter;
    private int disablingTimer;

    public Camera camera;
    public GameObject freeLook;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        characterManager = CharacterManager.Instance;
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (disablingTimer > 0 && disablingTimer < 60) disablingTimer++;
        else disablingTimer = 0;
    }

    public void ResetCamera()
    {
        simpleFollowRecenter.recenter = true;
    }

    IEnumerator ResetCoroutine()
    {
        //cinemachineFreeLook.m_RecenterToTargetHeading.m_enabled = true;
        simpleFollowRecenter.recenter = true;
        yield return new WaitForSeconds(simpleFollowRecenter.recenterTime);
        //yield return new WaitForSeconds(0);
        simpleFollowRecenter.recenter = false;
        //cinemachineFreeLook.m_RecenterToTargetHeading.m_enabled = false;
    }
}
