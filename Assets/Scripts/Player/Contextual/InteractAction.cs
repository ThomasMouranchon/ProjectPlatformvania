using Cinemachine;
using FIMSpace.Basics;
using FIMSpace.FProceduralAnimation;
using FIMSpace.FTail;
using FIMSpace.GroundFitter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : MonoBehaviour
{
    private static InteractAction instance = null;
    public static InteractAction Instance => instance;

    public bool isInteracting, nextInteraction, isFastForward;
    public int afterTalkTimer;
    private bool validate, validateHold, back, backHold, jump;

    private CharacterManager characterManager;
    private HealthManager healthManager;
    private InputReader inputReader;

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
    }

    private void Start()
    {
        characterManager = CharacterManager.Instance;
        healthManager = HealthManager.Instance;
        inputReader = InputReader.Instance;
    }

    void Update()
    {
        validate = inputReader.validate;
        validateHold = inputReader.validateHold;
        back = inputReader.back;
        backHold = inputReader.backHold;
        jump = inputReader.jump;
    }

    private void FixedUpdate()
    {
        if ((validate | jump | backHold) && characterManager.isGrounded
            && !characterManager.isJumping && !characterManager.isDashing
            && !characterManager.landed && characterManager.moveForm <= 1
            && characterManager.canInteract)
        {
            if (isInteracting)
            {
                if (afterTalkTimer == 0)
                {
                    afterTalkTimer++;
                    NextInteraction();
                }

                if (backHold) isFastForward = true;
                else isFastForward = false;
            }
            else if (!backHold)
            {
                characterManager.canMove = false;
                //inputReader.enableJump = false;
                inputReader.enableDash = false;
                inputReader.enableBomb = false;
                inputReader.enableYoyo = false;
                inputReader.enablePause = false;

                afterTalkTimer++;
                StartInteraction();
            }
        }
        if (afterTalkTimer > 0 && afterTalkTimer < 15) afterTalkTimer++;
        else afterTalkTimer = 0;
    }

    public void StartInteraction()
    {
        if (!isInteracting)
        {
            isInteracting = true;

            nextInteraction = true;
            healthManager.HideUI(true);

            characterManager.rigidbody.isKinematic = true;
        }
    }

    public void NextInteraction()
    {
        nextInteraction = true;
    }
    public void EndTalk()
    {
        isInteracting = false;
        characterManager.EnableMovement();
        healthManager.HideUI(false);

        characterManager.rigidbody.isKinematic = false;
    }
}