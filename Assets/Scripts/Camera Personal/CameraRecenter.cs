using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
public class CameraRecenter : MonoBehaviour
{
    private static CameraRecenter instance = null;
    public static CameraRecenter Instance => instance;

    [HideInInspector]
    public bool recenter, scaledTime;
    public float recenterTime = 0.5f;
    public Transform target;
    CinemachineFreeLook vcam;
    CinemachineOrbitalTransposer[] orbital = new CinemachineOrbitalTransposer[3];
    CinemachineVirtualCamera[] rigs = new CinemachineVirtualCamera[3];
    public CharacterManager characterManager;
    public OptionsValues optionValues;
    
    //public Animator speedLinesRecenterAnim;

    void Awake()
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

        vcam = GetComponent<CinemachineFreeLook>();
        for (int i = 0; vcam != null && i < 3; ++i)
        {
            rigs[i] = vcam.GetRig(i);
            orbital[i] = rigs[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
        recenter = true;
    }

    void Update()
    {
        if (!scaledTime)
        {
            if (target == null)
            {
                return;
            }

            for (int i = 0; i < 3; ++i)
            {
                orbital[i].enabled = !recenter;
            }

            if (recenter)
            {
                Vector3 up = vcam.State.ReferenceUp;
                Vector3 back = vcam.transform.position - target.position;
                float angle = UnityVectorExtensions.SignedAngle(back.ProjectOntoPlane(up), -target.forward.ProjectOntoPlane(up), up);
                if (Mathf.Abs(angle) < UnityVectorExtensions.Epsilon)
                {
                    recenter = false;
                    //speedLinesRecenterAnim.SetBool("move", false);
                }
                else
                {
                    StartCoroutine(UnscaledResetCoroutine());
                }

                characterManager.GetComponent<Rigidbody>().velocity += characterManager.forward * 1;
                recenter = false;
            }
        }
    }

    void FixedUpdate()
    {        
        if (scaledTime)
        {
            if (target == null)
            {
                return;
            }

            for (int i = 0; i < 3; ++i)
            {
                orbital[i].enabled = !recenter;
            }

            if (recenter)
            {
                Vector3 up = vcam.State.ReferenceUp;
                Vector3 back = vcam.transform.position - target.position;
                float angle = UnityVectorExtensions.SignedAngle(back.ProjectOntoPlane(up), -target.forward.ProjectOntoPlane(up), up);
                //vcam.m_YAxis.Value = 0.5f;
                if (Mathf.Abs(angle) < UnityVectorExtensions.Epsilon)
                {
                    recenter = false;
                    //speedLinesRecenterAnim.SetBool("move", false);
                }
                else
                {
                    StartCoroutine(ScaledResetCoroutine());
                }

                characterManager.GetComponent<Rigidbody>().velocity += characterManager.forward * 1;
                recenter = false;
            }
        }
    }

    IEnumerator UnscaledResetCoroutine()
    {
        vcam.m_RecenterToTargetHeading.m_enabled = true;
        vcam.m_YAxisRecentering.m_enabled = true;

        //speedLinesRecenterAnim.SetBool("move", true);
        
        yield return new WaitForSecondsRealtime(recenterTime / 2);

        //speedLinesRecenterAnim.SetBool("move", false);

        yield return new WaitForSecondsRealtime(recenterTime / 2);

        vcam.m_RecenterToTargetHeading.m_enabled = false;
        vcam.m_YAxisRecentering.m_enabled = false;
    }

    IEnumerator ScaledResetCoroutine()
    {
        vcam.m_RecenterToTargetHeading.m_enabled = true;
        vcam.m_YAxisRecentering.m_enabled = true;
        yield return new WaitForSeconds(recenterTime);
        vcam.m_RecenterToTargetHeading.m_enabled = false;
        vcam.m_YAxisRecentering.m_enabled = false;
    }

    IEnumerator ResetCoroutine2()
    {
        yield return new WaitForSeconds(1);
        recenter = false; // done!
        //vcam.m_RecenterToTargetHeading.m_enabled = false;
    }
}