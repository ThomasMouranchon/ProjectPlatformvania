using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ParentTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CharacterManager.Instance.ResetParent();
        }
    }

    private void OnDestroy()
    {
        if (CharacterManager.Instance == null) return;
        else
        {
            if (CharacterManager.Instance.transform.parent = gameObject.transform)
            {
                CharacterManager.Instance.ResetParent();
            }
        }
    }
}
