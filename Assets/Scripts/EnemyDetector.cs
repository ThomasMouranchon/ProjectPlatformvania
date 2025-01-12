using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public bool isOnEnemy;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isOnEnemy = true;            
        }
        //else isOnEnemy = false;
    }

    private void OnTriggerExit()
    {
        isOnEnemy = false;
    }
}
