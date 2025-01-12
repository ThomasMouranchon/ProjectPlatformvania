using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingChecker : MonoBehaviour
{
    private CharacterManager characterManager;
    // Start is called before the first frame update
    void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        characterManager.canLedgeJump = false;
    }

    private void OnTriggerExit(Collider other)
    {
        characterManager.canLedgeJump = true;
    }
}
