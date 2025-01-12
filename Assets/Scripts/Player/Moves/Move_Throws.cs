using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class Move_Throws : MonoBehaviour
{
    private static Move_Throws instance = null;
    public static Move_Throws Instance => instance;

    private CharacterManager characterManager;
    private HealthManager healthManager;
    private InputReader inputReader;
    private ItemsHandler itemsHandler;

    private float afterThrowTimer;
    [Space(10)]

    [Tooltip("Electroball when having the yellow/happy form")]
    public GameObject electroBall;

    public VisualEffect yellowElectroBallThrowVFX, yellowCrouchElectroBallThrowVFX;
    public VisualEffect altYellowElectroBallThrowVFX, altYellowCrouchElectroBallThrowVFX;
    public VisualEffect chargedElectroBallThrowVFX;
    [Space(10)]

    public ParticleSystem chargedElectroBallIsReadyEffect, chargedElectroBallIsReadyEffectCrouch;
    [Space(10)]

    public bool yoyoIsActive, isGettingDragged, isHoldingToRope, isGroundedForSlide;
    private int skipMaskPowerInputTimer;
    public GameObject dragHitbox;
    [Space(10)]

    public GameObject ropePointLeft, ropePointRight;
    public int dragVerticalMultiplier, jumpFromDragTimer;
    public bool endedDragCoroutineIsPlaying, usedDashForGHCancel;
    public float dragReturnSpeedXZ;
    [HideInInspector] public bool isTouchingCloud;
    [Space(30)]

    public GameObject bomb;
    public Transform bombSpawnPoint;

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
        characterManager = CharacterManager.Instance;
        healthManager = HealthManager.Instance;
        inputReader = InputReader.Instance;
        itemsHandler = ItemsHandler.Instance;

        dragHitbox.SetActive(false);
        yoyoIsActive = false;

        afterThrowTimer = 21;
        skipMaskPowerInputTimer = 0;
        endedDragCoroutineIsPlaying = false;
        usedDashForGHCancel = false;
        dragReturnSpeedXZ = 0;
    }

    private void FixedUpdate()
    {
        if (afterThrowTimer == 0 | afterThrowTimer > 14)
        {
            characterManager.isThrowing = false;
            characterManager.altThrowing = false;
        }

        if (afterThrowTimer >= 1 && afterThrowTimer <= 18) afterThrowTimer++;

        ropePointLeft.SetActive(yoyoIsActive);
                
        if (!YoyoHandler.Instance)
        {
            yoyoIsActive = false;
            characterManager.soulAnim.SetBool("yoyoIsActive", false);
        }

        if (skipMaskPowerInputTimer > 0 && skipMaskPowerInputTimer < 3)
        {
            skipMaskPowerInputTimer++;
        }
        else skipMaskPowerInputTimer = 0;

        if (jumpFromDragTimer > 0 && jumpFromDragTimer < 20)
        {
            jumpFromDragTimer++;
        }
        else jumpFromDragTimer = 0;

        characterManager.soulAnim.SetBool("isGettingDragged", isGettingDragged);
    }

    public void Throws()
    {
        if (inputReader.yoyo && !characterManager.isDashing && !characterManager.isCloseToCeiling &&
            !characterManager.isDiving && !characterManager.isWallSliding && itemsHandler.enableYoyo &&
            ((afterThrowTimer > 14 && !yoyoIsActive) | (afterThrowTimer > 5 && yoyoIsActive)))
        {
            afterThrowTimer = 1;

            if (yoyoIsActive)
            {
                if (!YoyoHandler.Instance.isReturningToPlayer)
                {
                    YoyoHandler.Instance.StartMoveTowardsPlayer();
                }
            }
            else
            {
                if (characterManager.runBoostTimer >= characterManager.runBoostTimerLimit) Instantiate<GameObject>(electroBall, this.transform.position, characterManager.characterModel.transform.rotation).GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, 110));
                else Instantiate<GameObject>(electroBall, this.transform.position, characterManager.characterModel.transform.rotation).GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, 60));

                characterManager.soulAnim.CrossFade("Player_FireBallThrowLeft", 0, 3);
                characterManager.soulAnim.CrossFade("Player_FireBallThrowLeft", 0, 8);

                characterManager.altThrowing = true;
                characterManager.isThrowing = false;
                altYellowElectroBallThrowVFX.Play();
            }
        }

        if (inputReader.bomb && itemsHandler.enableBomb && afterThrowTimer > 14)
        {
            if (BombHandler.Instance)
            {
                afterThrowTimer = 1;

                Destroy(BombHandler.Instance.gameObject);
            }
            else if (!characterManager.isDashing && !characterManager.isDiving && !characterManager.isWallSliding)
            {
                afterThrowTimer = 1;

                Instantiate<GameObject>(bomb, bombSpawnPoint.position, characterManager.characterModel.transform.rotation).GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, 0));

                characterManager.soulAnim.CrossFade("Player_FireBallThrowRight", 0, 3);
                characterManager.soulAnim.CrossFade("Player_FireBallThrowRight", 0, 8);

                characterManager.isThrowing = true;

                yellowElectroBallThrowVFX.Play();
            }
        }
    }

    public void UpdateLine()
    {
        if (YoyoHandler.Instance)
        {
            YoyoHandler electroBall = YoyoHandler.Instance;
            electroBall.lineRenderer.SetPosition(0, electroBall.transform.position);
            electroBall.lineRenderer.SetPosition(electroBall.lineRenderer.positionCount - 1, ropePointLeft.transform.position);
        }
    }
}
