using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDetector : MonoBehaviour
{
    private static ButtonDetector instance = null;
    public static ButtonDetector Instance => instance;


    public Animator animator;
    public CharacterManager characterManager;
    public HealthManager healthManager;
    public GameObject playerObject;
    public InteractAction interactAction;

    private void Awake()
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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Button" && other.GetComponentInChildren<InteractableButton>().interactionType == InteractionType.Confirm
            && characterManager.isGrounded && !characterManager.isJumping && !characterManager.landed
            && !characterManager.isDashing && characterManager.moveForm <= 1 && !healthManager.isHurt)
        {
            InteractableButton interactableButton = other.GetComponentInChildren<InteractableButton>();
            if (interactAction.isInteracting)
            {
                if (interactAction.nextInteraction)
                {
                    interactableButton.CollisionEffect();
                    interactAction.nextInteraction = false;
                }

                animator.SetBool("canTalk", false);

                Vector3 direction = other.transform.position - playerObject.transform.position;
                direction.y = 0;

                characterManager.targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
                characterManager.lastRotationAngle = Quaternion.LookRotation(direction).eulerAngles.y;

                characterManager.rigidbody.velocity = Vector3.zero;

                direction = playerObject.transform.position - other.transform.position;
                direction.y = 0;
                other.transform.rotation = Quaternion.LookRotation(direction);
            }
            /*
            if (!interactAction.isInteracting && interactableButton.interactionType == InteractionType.Confirm)
            {
                animator.SetBool("canTalk", false);
                characterManager.canTalk = false;

                interactAction.EndTalk();
            }
            else if (interactAction.isInteracting && interactableButton.interactionType == InteractionType.Confirm)
            {
                animator.SetBool("canTalk", false);

                Vector3 direction = other.transform.position - playerObject.transform.position;
                direction.y = 0;

                characterManager.targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
                characterManager.lastRotationAngle = Quaternion.LookRotation(direction).eulerAngles.y;

                characterManager.rigidbody.velocity = Vector3.zero;

                direction = playerObject.transform.position - other.transform.position;
                direction.y = 0;
                other.transform.rotation = Quaternion.LookRotation(direction);
            }*/
            else
            {
                animator.SetBool("canTalk", true);
                characterManager.canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Button" && other.GetComponentInChildren<InteractableButton>().interactionType == InteractionType.Confirm)
        {
            if (interactAction.isInteracting)
            {
                if (Vector3.Distance(characterManager.transform.position, other.transform.position) > 3)
                {
                    animator.SetBool("canTalk", false);
                    characterManager.canInteract = false;
                }
                else if (Vector3.Distance(characterManager.transform.position, other.transform.position) > 1)
                {
                    characterManager.transform.position = Vector3.MoveTowards(characterManager.transform.position, other.transform.position, 0.3f);
                }
            }
            else
            {
                animator.SetBool("canTalk", false);
                characterManager.canInteract = false;
            }
        }
    }
}
