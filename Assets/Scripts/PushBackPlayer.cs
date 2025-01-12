using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBackPlayer : MonoBehaviour
{
    /*private CharacterManager characterManager;
    private Rigidbody playerRigidbody;
    private bool pushBack = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = characterManager.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (characterManager.isGrounded && characterManager.isTouchingWall) playerRigidbody.velocity -= characterManager.forward * characterManager.moveForm;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Default" | other.tag == "Untagged" | other.tag == "Ground")
        {
            pushBack = true;
            if (characterManager.isGrounded && characterManager.isTouchingWall) playerRigidbody.velocity -= characterManager.forward * characterManager.moveForm * 10f;
            else if (characterManager.isGrounded) playerRigidbody.velocity -= characterManager.forward * characterManager.moveForm / 2f;
        }
    }
    
    /*private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Default" | other.tag == "Untagged" | other.tag == "Ground")
        {
            pushBack = false;
        }
    }*/
}
