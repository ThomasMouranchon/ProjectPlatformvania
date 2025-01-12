using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoCharacterDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (YoyoHandler.Instance)
        {
            if (other.tag == "Player" && (YoyoHandler.Instance.isIdling | YoyoHandler.Instance.isReturningToPlayer))
            {
                YoyoHandler.Instance.CaughtByPlayer();
            }
        }
    }
}
