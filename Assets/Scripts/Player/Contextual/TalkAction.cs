using Cinemachine;
using FIMSpace.FProceduralAnimation;
using FIMSpace.FTail;
using FIMSpace.GroundFitter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkAction : MonoBehaviour
{
    private static TalkAction instance = null;
    public static TalkAction Instance => instance;

    //[HideInInspector]
    public bool isTalking;
    public bool nextText;
    public Animator animator;

    private AnimUpdater animUpdater;
    private TailAnimator2 soulLHandAnim, soulRHandAnim;
    [Space(5)]
    private LegsAnimator soulLegsAnim;
    private FGroundRotator soulGroundRotator;
    [HideInInspector]
    public Animator soulAnim, eyeAnim;
    public UISoulEyesHandler soulEyesHandler;
    private CameraRecenter cameraRecenter;
    private CharacterManager characterManager;
    [HideInInspector] public ZoomHandler zoomHandler;
    private HealthManager healthManager;
    private InputReader inputReader;

    private float[] initialComposerHeight = new float[3];
    public float[] talkComposerHeight;
    [Space(10)]
    [Tooltip("Number of steps when switching camera height")]
    public int numberOfSteps;

    private bool validate, validateHold, back, backHold, jump;
    public Animator fastForwardAnim;
    public bool isFastForward;
    public int afterTalkTimer;

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

    void Start()
    {
        animUpdater = AnimUpdater.Instance;

        soulLegsAnim = animUpdater.soulLegsAnim;
        soulGroundRotator = animUpdater.soulGroundRotator;
        soulAnim = animUpdater.soulAnim;
        eyeAnim = animUpdater.eyeAnim;

        cameraRecenter = CameraRecenter.Instance;
        characterManager = CharacterManager.Instance;
        healthManager = HealthManager.Instance;
        inputReader = InputReader.Instance;
        zoomHandler = characterManager.zoomHandlerTalk;
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
            && characterManager.canTalk)
        {
            if (isTalking)
            {
                if (afterTalkTimer == 0)
                {
                    afterTalkTimer++;
                    NextTalk();
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
                isFastForward = false;

                afterTalkTimer++;
                StartTalk();
            }
        }
        else
        {
            isFastForward = false;
        }

        fastForwardAnim.SetBool("active", isFastForward);

        if (afterTalkTimer > 0 && afterTalkTimer < 15) afterTalkTimer++;
        else afterTalkTimer = 0;
    }

    public void StartTalk()
    {
        if (!isTalking)
        {
            isTalking = true;
            animator.CrossFade("Player_Idle", 0);

            nextText = true;
            healthManager.HideUI(true);

            zoomHandler.Zoom(!CameraFreeLookController.Instance.fixedCamera, false);
            characterManager.rigidbody.isKinematic = true;
        }
    }

    public void NextTalk()
    {
        nextText = true;
    }

    public void EndTalk()
    {
        isTalking = false;
        characterManager.EnableMovement();
        healthManager.HideUI(false);

        zoomHandler.Zoom(false, false);

        characterManager.rigidbody.isKinematic = false;
    }

    private IEnumerator ProgressiveHeightModification(bool talkVersion)
    {/*
        if (talkVersion)
        {
            while (composers[0].m_ScreenY > talkComposerHeight[0])
            {
                for (int i = 0; i < composers.Length - 1; i++)
                {
                    composers[i].m_ScreenY -= (initialComposerHeight[i] - talkComposerHeight[i]) / numberOfSteps;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (composers[0].m_ScreenY < initialComposerHeight[0])
            {
                for (int i = 0; i < composers.Length - 1; i++)
                {
                    composers[i].m_ScreenY += (initialComposerHeight[i] - talkComposerHeight[i]) / numberOfSteps;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }*/
        yield return null;
    }
}
