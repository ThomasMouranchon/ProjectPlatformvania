using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDetector : MonoBehaviour
{
    private static NPCDetector instance = null;
    public static NPCDetector Instance => instance;


    public Animator animator;
    public CharacterManager characterManager;
    public HealthManager healthManager;
    public GameObject playerObject;
    public TalkAction talkAction;
    private ZoomHandler zoomHandler;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC" && other.GetComponent<NPCIdleHandler>().canTalk)
        {
            animator.SetBool("canTalk", true);
        }
    }*/

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

    private void Start()
    {
        zoomHandler = characterManager.zoomHandlerTalk;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "NPC" && other.GetComponentInChildren<NPCIdleHandler>().enableTalk
            && characterManager.isGrounded && !characterManager.isJumping && !characterManager.landed
            && !characterManager.isDashing && characterManager.moveForm <= 1 && !healthManager.isHurt)
        {
            NPCIdleHandler nPCIdleHandler = other.GetComponentInChildren<NPCIdleHandler>();
            if (nPCIdleHandler.longTalk.endedTalk)
            {
                animator.SetBool("canTalk", false);
                characterManager.canTalk = false;

                talkAction.EndTalk();
                nPCIdleHandler.EndTalkFunction(true);
            }
            else if (talkAction.isTalking && nPCIdleHandler.canTalk)
            {
                if (talkAction.nextText)
                {
                    if (nPCIdleHandler.longTalk.endedCurrentTalk)
                    {
                        nPCIdleHandler.NextTalkFunction();
                    }
                    talkAction.nextText = false;
                }

                if (!nPCIdleHandler.hasFixedCamera) zoomHandler.Zoom(true, false);
                
                animator.SetBool("canTalk", false);

                Vector3 direction = other.transform.position - playerObject.transform.position;
                direction.y = 0;

                //playerObject.transform.rotation = Quaternion.LookRotation(direction);
                
                characterManager.targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
                characterManager.lastRotationAngle = Quaternion.LookRotation(direction).eulerAngles.y;

                characterManager.rigidbody.velocity = Vector3.zero;

                direction = playerObject.transform.position - other.transform.position;
                direction.y = 0;
                other.transform.rotation = Quaternion.LookRotation(direction);

                //characterManager.targetAngle = playerObject.transform.rotation.y;
                //characterManager.lastRotationAngle = playerObject.transform.rotation.y;
            }
            else
            {
                animator.SetBool("canTalk", true);
                characterManager.canTalk = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC" && !talkAction.isTalking)
        {
            animator.SetBool("canTalk", false);
            characterManager.canTalk = false;
            talkAction.nextText = false;

            talkAction.EndTalk();
            NPCIdleHandler nPCIdleHandler = other.GetComponentInChildren<NPCIdleHandler>();
            nPCIdleHandler.EndTalkFunction(false);
        }
        else if (other.tag == "NPC")
        {
            if (Vector3.Distance(characterManager.transform.position, other.transform.position) > 3)
            {
                animator.SetBool("canTalk", false);
                characterManager.canTalk = false;
                talkAction.nextText = false;

                talkAction.EndTalk();
            }
            else if (Vector3.Distance(characterManager.transform.position, other.transform.position) > 1)
            {
                characterManager.transform.position = Vector3.MoveTowards(characterManager.transform.position, other.transform.position, 0.3f);
            }
        }
    }
}
