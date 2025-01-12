using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCCharacterDetector : MonoBehaviour
{
    public Animator animator;
    public NPCIdleHandler npcIdleHandler;

    private CinemachineFreeLook cinemachineFreeLook;

    public Transform contextualActionParent;

    private float posX;
    private float posZ;

    private float distance;

    void Start()
    {
        cinemachineFreeLook = ScriptLocations.Instance.cinemachineFreeLook;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            npcIdleHandler.isInRange = true;
        }
    }

    private void FixedUpdate()
    {
        if (npcIdleHandler.isInRange && npcIdleHandler.mainAnim.GetCurrentAnimatorStateInfo(0).IsName("NPC_Idle"))
        {
            posX = Mathf.Abs(cinemachineFreeLook.transform.position.x - contextualActionParent.transform.position.x);
            posZ = Mathf.Abs(cinemachineFreeLook.transform.position.z - contextualActionParent.transform.position.z);

            distance = Vector3.Distance(contextualActionParent.transform.position, cinemachineFreeLook.transform.position);

            if ((posX < 8 && posZ < 8) | distance > 30) animator.SetBool("canTalk", false);
            else animator.SetBool("canTalk", true);
            //Debug.Log(distance.ToString());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("canTalk", false);
            npcIdleHandler.isInRange = false;
            //animator.CrossFade("ContextualAction_Disappear", 0);
        }
    }
}