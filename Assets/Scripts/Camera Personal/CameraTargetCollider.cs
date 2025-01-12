using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetCollider : MonoBehaviour
{
    public CameraTarget cameraTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") cameraTarget.SwitchTarget(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") cameraTarget.SwitchTarget(false);
    }
}
