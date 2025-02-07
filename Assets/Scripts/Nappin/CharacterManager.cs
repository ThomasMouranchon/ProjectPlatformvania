using Cinemachine;
using FIMSpace;
using FIMSpace.Basics;
using FIMSpace.FProceduralAnimation;
using FIMSpace.FTail;
using FIMSpace.GroundFitter;
using System.Collections;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.VFX;
//using FIMSpace.GroundFitter;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance = null;
    public static CharacterManager Instance => instance;

    private ScriptLocations scriptLocations;
    [Space(10)]

    [Header("Movement specifics")]
    [Tooltip("Layers where the player can stand on")]
    [SerializeField] public LayerMask groundMask;
    [Tooltip("Base player speed")]
    public float movementSpeed = 14f;
    [Tooltip("After jumping from a ledge player speed")]
    public float afterLedgeMovementSpeed = 14f;
    private float initialMovementSpeed;
    [Range(0f, 1f)]
    [Tooltip("Minimum input value to trigger movement")]
    public float movementThrashold = 0.01f;
    [Space(10)]

    [Tooltip("Speed up multiplier")]
    public float dampSpeedUp = 0.2f;
    private float initialDampSpeedUp;
    [Tooltip("Speed up multiplier when gliding")]
    public float dampGlideSpeedUp = 0.2f;
    [Tooltip("Speed down multiplier")]
    public float dampSpeedDown = 0.1f;
    [Tooltip("Speed down multiplier for keyboard")]
    public float dampSpeedDownKeyboard = 0.2f;
    [Tooltip("Speed down multiplier when close to ledge")]
    public float dampSpeedDownLedge = 0.2f;
    [Tooltip("Speed down multiplier when close to ledge for keyboard")]
    public float dampSpeedDownLedgeKeyboard = 0.2f;
    [Space(10)]

    [Header("Jump and gravity specifics")]
    [Tooltip("Jump velocity")]
    public float jumpVelocity = 20f;
    [Tooltip("Small Jump velocity")]
    public float smallJumpVelocity = 20f;
    [Tooltip("Multiplier applied to gravity when the player is falling")]
    public float fallMultiplier = 1.7f;
    [Tooltip("Multiplier applied to gravity when the player is holding jump")]
    public float holdJumpMultiplier = 5f;
    [Range(0f, 1f)]
    [Tooltip("Player friction against floor")]
    public float frictionAgainstFloor = 0.3f;
    [Range(0.01f, 0.99f)]
    [Tooltip("Player friction against wall")]
    public float frictionAgainstWall = 0.839f;
    [Space(5f)]

    [Header("Slope and step specifics")]
    [Tooltip("Distance from the player feet used to check if the player is touching the ground")]
    public float groundCheckerThrashold = 0.1f;
    [Tooltip("Distance from the player feet used to check if the player is touching a slope")]
    public float slopeCheckerThrashold = 0.51f;
    [Tooltip("Distance from the player center used to check if the player is touching a step")]
    public float stepCheckerThrashold = 0.6f;
    private Vector3 slopeJumpDirection;
    [Space(10)]

    [Range(1f, 89f)]
    [Tooltip("Max climbable slope angle")]
    public float maxClimbableSlopeAngle = 53.6f;
    [Tooltip("Max climbable step height")]
    public float maxStepHeight = 0.74f;
    [Space(5)]

    public float distToGroundForSlopes;
    public Transform frontSlopePoint;
    public Transform backSlopePoint;
    public Transform leftSlopePoint;
    public Transform rightSlopePoint;
    [Space(5)]

    [Tooltip("Vertical multiplier used when the player is jumping from a slope")]
    public float verticalJumpFromSlope = 30f;
    [Tooltip("Horizontal multiplier used when the player is jumping from a slope")]
    public float horizontalJumpFromSlope = 30f;
    public float verticalSmallJumpFromSlope = 30f;
    public float horizontalSmallJumpFromSlope = 30f;
    [Space(10)]

    [Tooltip("Speed multiplier based on slope angle")]
    public AnimationCurve speedMultiplierOnAngle = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
    [Range(0.01f, 1f)]
    [Tooltip("Multipler factor on climbable slope")]
    public float canSlideMultiplierCurve = 0.061f;
    [Range(0.01f, 1f)]
    [Tooltip("Multipler factor on non climbable slope")]
    public float cantSlideMultiplierCurve = 0.039f;
    [Range(0.01f, 1f)]
    [Tooltip("Multipler factor on step")]
    public float climbingStairsMultiplierCurve = 0.637f;

    public float slideMovementSpeed = 14f;
    [Space(10)]

    [Tooltip("Multipler factor for gravity")]
    public float gravityMultiplier = 6f;
    [Tooltip("Multipler factor for gravity used on change of normal")]
    public float gravityMultiplayerOnSlideChange = 3f;
    [Tooltip("Multipler factor for gravity used on non climbable slope")]
    public float gravityMultiplierIfUnclimbableSlope = 30f;
    [Space(10)]

    public bool lockOnSlope = true;
    private float slopeDot;

    [Header("Wall slide specifics")]
    [Tooltip("Layers where the player can wall slide on")]
    [SerializeField] public LayerMask wallMask;
    [Tooltip("Speed when the player is touching a wall")]
    public float wallMovementSpeed = 10f;
    [Tooltip("Distance from the player head used to check if the player is touching a wall")]
    public float wallCheckerThrashold = 0.8f;
    [Tooltip("Distance from the player head used to check if the player is touching a wall")]
    public float wallCheckerThrasholdForGround = 0.8f;
    [Tooltip("Distance from the player head used to check if the player is touching a wall")]
    public float wallCheckerThrasholdForAnim = 0.8f;
    [Tooltip("Distance from the player head used to check if the player is touching a wall")]
    public float wallCheckerThrasholdForDash = 0.8f;
    [Tooltip("Wall checker distance from the player center upward")]
    public float heightWallCheckerTop = 0.5f;
    [Tooltip("Wall checker distance from the player center downard")]
    public float heightWallCheckerBottom = 0.5f;
    [Space(5)]

    [Tooltip("Vertical multiplier used when the player is jumping from a wall")]
    public float verticalJumpFromWallMultiplier = 30f;
    [Tooltip("Horizontal multiplier used when the player is jumping from a wall")]
    public float horizontalJumpFromWallMultiplier = 30f;
    [Tooltip("Horizontal multiplier used when the player is jumping from a wall")]
    public float smallHorizontalJumpFromWallMultiplier = 30f;
    [Tooltip("Factor used to determine the height of the jump")]
    public float multiplierVerticalLeap = 1f;

    [Header("References")]
    [Tooltip("Character camera")]
    public Camera characterCamera;
    private Vector3 cameraInitialPosition;
    [Tooltip("Character model")]
    public GameObject characterModel;
    public GameObject characterHead;
    public Transform characterArmature;
    [Tooltip("Character rotation speed when the forward direction is changed")]
    public float characterModelRotationSmooth = 0.1f;
    public float glideRotationSmooth = 0.1f;
    public float slideRotationSmooth = 0.1f;
    private float initialcharacterModelRotationSmooth;
    //[HideInInspector]
    public float lastRotationAngle;
    [Space(10)]

    [Tooltip("Head reference")]
    public Transform farFrontPoint;
    [Tooltip("Head reference")]
    public Transform frontPoint;
    [Tooltip("Head reference")]
    public Transform farBackPoint;
    [Tooltip("Head reference")]
    public Transform backPoint;
    [Tooltip("Left foot reference")]
    public Transform leftPoint;
    [Tooltip("Right foot reference")]
    public Transform rightPoint;
    [Space(10)]

    [Tooltip("Input reference")]
    private InputReader inputReader;
    private MovementActions movementActions;
    [Space(10)]

    public bool debug = true;
    [Space(50)]

    public Vector3 forward;
    [HideInInspector] public Vector3 globalForward;
    private Vector3 reactionForward;
    private Vector3 down;
    private Vector3 globalDown;
    private Vector3 reactionGlobalDown;

    private float currentSurfaceAngle;
    private bool currentLockOnSlope;

    private Vector3 wallNormal;
    private Vector3 groundNormal;
    private Vector3 prevGroundNormal;
    private bool prevGrounded;

    private float coyoteJumpMultiplier = 1f;

    public bool isGrounded;
    public bool isGroundJustBelow;
    public bool isGroundBelow;
    public bool farFrontIsGround;
    public bool frontIsGround;
    [HideInInspector] public bool backIsGround;
    [HideInInspector] public bool leftIsGround;
    [HideInInspector] public bool rightIsGround;

    [HideInInspector] public bool isTouchingSlope;
    private bool isTouchingStep;
    [HideInInspector] public bool isTouchingWall;
    private bool isCloseToWall;
    [HideInInspector] public bool isCloseToWallForAir;
    private bool isCloseToWallForDash;
    public bool isCloseToCeiling;
    private bool isCloseToCeilingForJump;
    private bool isCloseToCeilingForStretch;
    [HideInInspector] public bool isCloseToCeilingForDrag;

    public bool isJumping;
    [HideInInspector] public bool finishedJumpCoroutine;
    [HideInInspector] public bool isAirJumping;
    [HideInInspector] public bool alternateJumping;
    [HideInInspector]
    public bool isLongJumping;
    private bool isSliding;
    public bool isSlidingDown;
    private bool slideInFront;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isGroundDashing;
    private bool isAirDashing;
    private bool canDash;
    [HideInInspector] public bool isThrowing;
    [HideInInspector] public bool altThrowing;
    private bool isCameraSwitching;
    [HideInInspector] public bool isGliding;
    public bool isDiving;
    [HideInInspector] public bool canTalk;
    [HideInInspector] public bool canInteract;

    private Vector2 axisInput;
    private bool jump;
    private bool jumpHold;
    private bool dash;
    private bool maskPower;
    private bool maskPowerHold;
    [HideInInspector] public bool lastFrameMaskPowerHold;
    private bool cameraSwitch;
    public int cameraSwitchTimer;
    private bool cameraSwitchHold;
    public int cameraSpeedValue;

    [HideInInspector]
    public float targetAngle;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector] public CapsuleCollider collider;
    public MeshCollider groundCollider;
    public CinemachineImpulseSource impulseSource;
    //public CapsuleCollider headCollider;
    private float originalColliderHeight;
    public GameObject groundDetecter;
    public GameObject groundDetecter2;

    // Add on

    [Tooltip("Layers where the player can grab ledgge on")]
    [SerializeField] LayerMask ledgeMask;
    [Tooltip("Ledge reference")]
    //public Transform ledgeStartPoint, ledgeEndPoint, ledgeCurrentRayPoint, ledgeTopCurrentRayPoint;
    public float ledgeJumpDistance;
    //public int ledgeJumpPowerMultiplier;
    private float[] ledgeJumpPowerMultiplierArray;
    public Transform[] ledgePointArray;
    private bool[] isCloseToLedgeArray;
    private bool ledgeJump;
    [HideInInspector]
    public bool canLedgeJump;

    [Space(10)]

    private Vector3 currVelocity = Vector3.zero;
    private float turnSmoothVelocity;
    [HideInInspector] public bool isWallSliding;
    private int isWallSlidingTimer;
    private bool isBouncing;
    private bool leftGroundDuringDash;

    public int idleTimer;
    public int idleTimerLimit;
    public int idleValue;

    public int afterJumpTimer;
    private int endedJumpTimer;
    public int jumpHoldTimer;
    private int wallJumpHoldTimer;
    [HideInInspector] public int afterWallJumpTimer;
    public int afterWallJumpTimerLimit;
    private int afterWallSlideTimer;
    private int afterGlideTimer;
    public int afterDashTimer;
    private int dashRecoilTimer;
    private int afterSlideTimer;
    private int afterSlopeJumpTimer;
    private int afterUnclimbableSlopeJumpTimer;
    private int afterNormalSlopeLeavingTimer;
    public int gettingPhotoTimer;

    private int afterGroundedDashTimer;
    [HideInInspector] public int tryingToClimbTimer; // Apply more gravity if you try to climb
    public int tryingToClimbTimerLimit; // Apply more gravity if you try to climb

    public int airbornedTimer;

    [HideInInspector] public bool wasAirborned;
    private int particleMovementEffectTimer;
    public float stickIncline;
    [Tooltip("Stick incline used when using keyboard to avoid slippery controls")]
    public float stickInclineRealValue;
    public bool canMove;

    public float stickInclineDeadZone;

    public float additionalGravity;
    [HideInInspector] public float distToGround;
    public float customDistToGround;
    public float customDistToGroundForDash;
    public float customDistToGroundForDrag;
    public float customDistToTrampoline;
    private float distToLedge;
    public float distToCeiling;
    public float currentDistToCeiling;
    public float distToCeilingForJump;
    public float distToCeilingForStretch;
    public float distToCeilingForDrag;

    public int afterDiveLandingTimer;
    public int afterLandingTimer;
    public bool landed;
    [Space(10)]

    [Header("Camera specifics")]

    public GameObject cameraTarget;
    public float normalHorizontalLerpSpeed;
    public float slowVerticalLerpSpeed;
    public float groundVerticalLerpSpeed;
    public float downwardSlopeVerticalLerpSpeed;
    public float diveVerticalLerpSpeed;
    public float airVerticalLerpSpeed;
    private float currentLerpSpeed;
    private float lerpSpeedTimer;
    public float cameraMovementSpeed = 14f;
    [Space(10)]

    public int forwardJumpVerticalVelocity;
    public int forwardJumpHorizontalVelocity;
    [Space(10)]

    public int runBoostTimerLimit;
    [HideInInspector] public int runBoostTimer;
    public int boostedMovementSpeed;

    public bool cameraShake;
    public bool isRespawning;
    [Space(10)]

    private bool fixedDirectionDash;

    [Header("Dash Specifics")]
    [Tooltip("Vertical multiplier used when the player is dashing in the air")]
    public float verticalAirDashMultiplier = 10f;
    [Tooltip("Horizontal multiplier used when the player is dashing in the air")]
    public float horizontalAirDashMultiplier = 10f;
    [Tooltip("Horizontal multiplier used when the player is dashing on the ground")]
    public float horizontalDashMultiplier = 10f;
    [Tooltip("Force used to push back player when dashing against a wall")]
    public float pushBackDashForce = 10f;
    [Space(10)]

    [Header("Kite Specifics")]
    public GameObject kite;
    public SkinnedMeshRenderer[] kiteSkinnedMeshes;
    public MeshRenderer[] kiteMeshes;
    public Animator kiteAnim;
    public TailAnimator2 kiteTailAnim;
    //private Vector3 kiteGlidePosition = new Vector3(0, 0.3f, 0);
    //public Vector3 kiteAfterRespawnPosition = new Vector3(0, 0.056f, -0.0055f);
    private Quaternion kiteInitialRotation;
    private Vector3 kiteRotationChange = new Vector3(0, 0, 15);
    private Vector3 kiteInitialScale;
    private Vector3 kiteScaleChange = new Vector3(0.2f, 0.2f, 0.2f);
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    [Header("Jump on Enemy Specifics")]
    public Vector3 jumpOnEnemyVisualPosition = new Vector3(0, -0.2f, 0);

    [Header("Mask Powers Specifics")]
    [Space(10)]

    [Header("Red/Angry Specifics")]
    [Space(5)]
    [Tooltip("Fireball speed")]
    public float fireBallSpeed = 20f;

    [Header("FPS Mode Specifics")]
    [Space(10)]

    public int moveForm;
    public int TPSWalkForm;
    [Tooltip("Horizontal multiplier used when hopping in FPS Mode")]
    public float horizontalHopMultiplier = 10f;
    [Tooltip("Vertical multiplier used when hopping in FPS Mode")]
    public float verticalHopMultiplier = 10f;
    private float runTimer;
    [Space(10)]

    [Tooltip("Layers corresponding to enemy")]
    [SerializeField] LayerMask enemyMask;
    //public ChangeBoolValue enemyBelowDetecter;
    public bool isOnTrampoline;
    public EnemyDetector enemyDetectorScript;
    [Space(10)]

    /*[Header("Trampoline Specifics")]
	public float bounceStrength = 2.5f;
	[Space(10)]
	*/
    public GameObject soulCharacter;
    public SkinnedMeshRenderer soulRenderer;
    private AnimUpdater animUpdater;
    [Space(5)]
    private LegsAnimator soulLegsAnim;
    private FGroundRotator soulGroundRotator;
    [HideInInspector]
    public Animator soulAnim, clothAnim, hatAndScarfAnim;
    [Space(10)]

    [HideInInspector] public Move_Throws move_Throws;
    private Move_PhantomDash move_PhantomDash;
    [HideInInspector] public MaskPowerHit maskPowerHit;
    private Move_CameraAim move_CameraAim;

    [HideInInspector] public AnimatorStateInfo currentAnimation;
    [Space(10)]

    [HideInInspector] public HealthManager healthManager;
    private GameManager gameManager;
    private PauseManager pauseManager;
    public CameraSwitcher cameraSwitcher;
    public CameraFreeLookController cameraFreeLookController;
    public CameraRecenter cameraRecenter;
    public CinemachineBrain cinemachineBrain;
    private RandomGenerator randomGeneratorScript;
    public StretchAnimation stretchAnimationScript;
    private AlphaChanger alphaChanger;
    private ItemsHandler itemsHandler;
    public UISoulEyesHandler uiSoulEyesHandler;
    public CinemachineInputProvider inputProvider;
    [HideInInspector] public EventTextAction eventTextAction;
    [Space(10)]

    public ZoomHandler zoomHandlerTalk;
    public ZoomHandler zoomHandlerWall;
    [Space(10)]

    public GameObject screenCenter;
    [Space(10)]

    public GameObject vfxFolder;
    public GameObject vfxThrowFolder;
    public GameObject afterHitEffect;
    [Space(10)]

    private int randomGeneratedSmoke;
    [Space(10)]

    private Quaternion neutralRotation;

    public ParticleSystem movementEffect3D;
    [Space(10)]

    public ParticleSystem slideEffect3D;
    [Space(10)]

    public ParticleSystem jumpEffect3D;
    public TrailRenderer jumpingTrail;
    [Space(10)]

    public ParticleSystem afterJumpEffect3D;
    [Space(10)]

    public ParticleSystem hopEffect3D;
    [Space(10)]

    public ParticleSystem landingEffect3D;
    [Space(10)]

    public ParticleSystem diveLandingEffect3D;
    [Space(10)]

    public ParticleSystem wallJumpEffect3D;
    [Space(10)]

    public ParticleSystem wallSlideEffect3D;
    [Space(10)]

    public ParticleSystem alternateWallSlideEffect3D;
    [Space(10)]

    public ParticleSystem AerialNeutralTrailDashEffect;
    public ParticleSystem AerialNeutralTrailDashEffect2;
    public ParticleSystem GroundedDashTrailEffect;
    public ParticleSystem GroundedDashTrailInitiateEffect;
    [Space(5)]

    public GameObject parentVfxsGameobject;
    public ParticleSystem diveTrailEffect1;
    public ParticleSystem diveTrailEffect2;
    public ParticleSystem diveTrailEffect3;
    [Space(10)]

    public ParticleSystem AerialDashSmokeEffect3D;
    [Space(10)]

    public ParticleSystem AerialNeutralDashSmokeEffect3D;
    [Space(10)]

    public ParticleSystem GroundedDashSmokeEffect3D;
    [Space(10)]

    [Header("Dash Specifics")]
    public GameObject speedBoostHitbox;
    public float dashKnockBackHorizontal;
    public float dashKnockBackVertical;
    [Space(5)]

    public ParticleSystem dashParticles;
    private int dashTimer;
    [Space(10)]

    public ParticleSystem aerialDashParticles;
    private int aerialDashTimer;
    [Space(10)]

    public ParticleSystem hitEffect1;
    public ParticleSystem hitEffect2;
    public ParticleSystem hitEffect3;
    public ParticleSystem hitEffect4;
    public ParticleSystem hitEffect5;
    public ParticleSystem hitEffect6;
    public ParticleSystem hemisphereHitEffect1;
    public ParticleSystem hemisphereHitEffect2;
    [Space(5)]

    public ParticleSystem jumpHitEffect1;
    public ParticleSystem jumpHitEffect2;
    public ParticleSystem jumpHitEffect3;
    [Space(5)]

    public ParticleSystem hurtEffect1;
    public ParticleSystem hurtEffect2;
    public ParticleSystem hurtEffect3;
    public ParticleSystem deathEffect;
    [Space(10)]

    public ParticleSystem[] respawnEffects;
    [Space(5)]

    public ParticleSystem isHurtEffect;
    public ParticleSystem isHurtEffect2;
    [Space(5)]
    public ParticleSystem runBoostEffect;

    [Space(10)]

    public TrailRenderer[] glidingTrails;

    private int afterGlidingTrailTimer;

    private Vector2 screenCenterPoint;
    private Ray ray;

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

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animUpdater = GetComponent<AnimUpdater>();

        soulLegsAnim = animUpdater.soulLegsAnim;
        soulGroundRotator = animUpdater.soulGroundRotator;
        soulAnim = animUpdater.soulAnim;
        clothAnim = animUpdater.clothAnim;
        hatAndScarfAnim = animUpdater.hatAndScarfAnim;

        //headCollider = headPoint.GetComponent<CapsuleCollider>();
        originalColliderHeight = collider.height;

        SetFriction(frictionAgainstFloor, true);
        currentLockOnSlope = lockOnSlope;

        initialMovementSpeed = movementSpeed;
        initialDampSpeedUp = dampSpeedUp;
        initialcharacterModelRotationSmooth = characterModelRotationSmooth;

        cameraSpeedValue = 2;

        distToGround = this.collider.bounds.extents.y;
        distToLedge = this.collider.bounds.extents.z;

        afterGroundedDashTimer = 30;
        tryingToClimbTimer = 0;

        cameraSwitchTimer = 21;

        speedBoostHitbox.SetActive(false);

        neutralRotation = new Quaternion(0, 0, 0, 0);

        jumpingTrail.enabled = false;

        foreach (TrailRenderer trailRenderer in glidingTrails)
        {
            trailRenderer.enabled = false;
        }

        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        currentAnimation = soulAnim.GetCurrentAnimatorStateInfo(0);

        kiteInitialRotation = kite.transform.localRotation;
        //kiteRotationChange = new Vector3(kiteRotationChange.x * kiteInitialRotation.x, kiteRotationChange.y * kiteInitialRotation.y, kiteRotationChange.z * kiteInitialRotation.z);
        kiteInitialScale = kite.transform.localScale;
        kiteScaleChange = new Vector3(kiteScaleChange.x * kiteInitialScale.x, kiteScaleChange.y * kiteInitialScale.y, kiteScaleChange.z * kiteInitialScale.z);

        altThrowing = false;
        alternateJumping = false;

        isCloseToLedgeArray = new bool[ledgePointArray.Length];
        ledgeJumpPowerMultiplierArray = new float[ledgePointArray.Length];

        for (int i = 0; i < isCloseToLedgeArray.Length; i++)
        {
            isCloseToLedgeArray[i] = false;
            ledgeJumpPowerMultiplierArray[i] = 26 + i * 2;
        }

        soulAnim.SetBool("isRespawning", true);
        StartCoroutine(WaitToResetCamera());
    }

    private void Start()
    {
        alphaChanger = AlphaChanger.Instance;
        eventTextAction = EventTextAction.Instance;
        inputReader = InputReader.Instance;
        movementActions = inputReader.movementActions;

        gameManager = GameManager.Instance;
        healthManager = HealthManager.Instance;

        move_Throws = Move_Throws.Instance;
        move_PhantomDash = Move_PhantomDash.Instance;
        maskPowerHit = GetComponentInChildren<MaskPowerHit>();
        move_CameraAim = Move_CameraAim.Instance;

        pauseManager = PauseManager.Instance;

        randomGeneratorScript = RandomGenerator.Instance;

        scriptLocations = ScriptLocations.Instance;

        itemsHandler = ItemsHandler.Instance;

        characterCamera = scriptLocations.mainCamera;
        cameraInitialPosition = characterCamera.transform.position;

        canLedgeJump = true;
        StartCoroutine(StartGameCoroutine(1));
    }

    private void Update()
    {
        axisInput = inputReader.axisInput;
        jump = inputReader.jump;
        jumpHold = inputReader.jumpHold;
        dash = inputReader.dash;
        maskPower = inputReader.yoyo;
        maskPowerHold = inputReader.yoyoHold;
        cameraSwitch = inputReader.cameraSwitch;
        cameraSwitchHold = inputReader.cameraSwitchHold;

        if (canMove)
        {
            stickIncline = Mathf.Abs(axisInput.y) + Mathf.Abs(axisInput.x);
            stickInclineRealValue = Mathf.Abs(axisInput.y) + Mathf.Abs(axisInput.x);
        }
        else
        {
            stickIncline = 0;
            stickInclineRealValue = 0;
        }


        if (stickIncline <= stickInclineDeadZone)
        {
            stickIncline = 0;
            moveForm = 0;
        }
        else if (stickIncline > 1)
        {
            stickIncline = 1;
        }

        if (stickInclineRealValue <= stickInclineDeadZone)
        {
            stickInclineRealValue = 0;
        }
        else if (stickInclineRealValue > 1)
        {
            stickInclineRealValue = 1;
        }

        if (inputReader.isMouseAndKeyboard && stickIncline > stickInclineDeadZone && !isCloseToWall)
        {
            moveForm = 6;
            stickIncline = 1f;
        }
        else if (stickIncline > stickInclineDeadZone && stickIncline <= 0.25)
        {
            moveForm = 1;
            stickIncline = 0.25f;
        }
        else if (stickIncline > 0.25 && stickIncline <= 0.4)
        {
            moveForm = 2;
            stickIncline = 0.4f;
        }
        else if (stickIncline > 0.4 && stickIncline <= 0.55)
        {
            moveForm = 3;
            stickIncline = 0.55f;
        }
        else if (stickIncline > 0.55 && stickIncline <= 0.7)
        {
            moveForm = 4;
            stickIncline = 0.7f;
        }
        else if (stickIncline > 0.7 && stickIncline <= 0.85)
        {
            moveForm = 5;
            stickIncline = 0.85f;
        }
        else if (stickIncline > 0.85)
        {
            moveForm = 6;
            stickIncline = 1;
        }
        else if (stickIncline > 0.55 && afterSlideTimer > 0 && isGrounded)
        {
            moveForm = 3;
            stickIncline = 0.55f;
        }


    }

    private void FixedUpdate()
    {
        CheckGrounded();
        CheckGroundBelow();
        CheckFarFrontGround();
        CheckFrontGround();
        CheckBackGround();
        CheckLeftGround();
        CheckRightGround();
        CheckJumpOnTrampoline();
        CheckLedgeGrab();
        CheckCeiling();
        CheckStep();

        if (itemsHandler.enableWallJump) CheckWall();
        else isTouchingWall = false;
        
        CheckSlopeAndDirections();

        // Movement

        MoveWalk();
        if (!isRespawning) MoveRotation();
        else if (isRespawning)
        {
            characterModel.transform.rotation = Quaternion.Euler(characterModel.transform.rotation.eulerAngles.x,
                lastRotationAngle,
                characterModel.transform.rotation.eulerAngles.z);
        }
        MoveJump();
        MoveDash();
        MoveMaskPower();
        MoveCameraSwitch();

        ApplyGravity();

        if (isJumping)
        {
            afterJumpTimer++;
        }
        else
        {
            afterJumpTimer = 0;
        }

        if ((afterJumpTimer > 0 && afterJumpTimer <= 8) | (afterWallJumpTimer > 0 && afterWallJumpTimer <= 6))
        {
            Instantiate<ParticleSystem>(afterJumpEffect3D, characterModel.transform.position, neutralRotation);
            jumpingTrail.enabled = true;
        }
        else if ((afterJumpTimer > 0 && afterJumpTimer <= 20) | (afterWallJumpTimer > 0 && afterWallJumpTimer <= afterWallJumpTimerLimit))
        {
            jumpingTrail.enabled = true;
        }
        else
        {
            jumpingTrail.enabled = false;
        }

        if (afterJumpTimer > 15 | isGliding | isDashing)
        {
            isBouncing = false;
            isLongJumping = false;
        }

        if (afterWallJumpTimer >= 1 && afterWallJumpTimer <= afterWallJumpTimerLimit) afterWallJumpTimer++;
        else afterWallJumpTimer = 0;

        if (afterWallJumpTimer >= 1 && afterWallJumpTimer <= 2) afterWallSlideTimer = 0;


        if (afterSlopeJumpTimer >= 1 && afterSlopeJumpTimer <= 40) afterSlopeJumpTimer++;
        if (afterUnclimbableSlopeJumpTimer >= 1 && afterUnclimbableSlopeJumpTimer <= 40) afterUnclimbableSlopeJumpTimer++;


        if (afterGroundedDashTimer >= 1 && afterGroundedDashTimer <= 30) afterGroundedDashTimer++;

        if (((tryingToClimbTimer <= tryingToClimbTimerLimit && afterWallJumpTimer > 10 && afterWallJumpTimer <= afterWallJumpTimerLimit)) && (isDashing | isGliding)) tryingToClimbTimer++;

        randomGeneratedSmoke = Random.Range(1, 3);

        parentVfxsGameobject.transform.position = characterModel.transform.position;

        currentAnimation = soulAnim.GetCurrentAnimatorStateInfo(0);

        if (!isGrounded && !move_Throws.isGroundedForSlide)
        {
            wasAirborned = false;
            landed = false;
            afterLandingTimer = 0;
            afterDiveLandingTimer = 0;
            if (!isTouchingWall) airbornedTimer++;
            else airbornedTimer = 10;
            if (afterGlideTimer < 10) afterGlideTimer++;

            if (isDiving) collider.radius = 1;
            else collider.radius = 0.25f;

            if (isGliding)
            {
                foreach (TailAnimator2 anim in animUpdater.frontClothTailAnims)
                {
                    anim.TailAnimatorAmount = 0;
                }
            }
            else
            {
                foreach (TailAnimator2 anim in animUpdater.frontClothTailAnims)
                {
                    anim.TailAnimatorAmount = 0.5f;
                }
            }

            if (airbornedTimer > 25 && (afterWallJumpTimer == 0 | afterWallJumpTimer > 15))
            {
                ledgeJump = false;
            }

            if (collider.height < 2.17f)
            {
                collider.height += 0.2f;
            }
            if (collider.center.y < 0)
            {
                collider.center += new Vector3(0, 0.1f, 0);
            }

            if (Physics.Raycast(groundDetecter.transform.position, Vector3.up, distToGround + 12, groundMask) &&
                Physics.Raycast(frontPoint.transform.position, Vector3.up, distToGround + 12, groundMask) &&
                Physics.Raycast(backPoint.transform.position, Vector3.up, distToGround + 12, groundMask) &&
                Physics.Raycast(leftPoint.transform.position, Vector3.up, distToGround + 12, groundMask) &&
                Physics.Raycast(rightPoint.transform.position, Vector3.up, distToGround + 12, groundMask) && isJumping)
            {
                soulAnim.CrossFade("Player_Scale_StretchSmallJump", 0.05f, 9);
            }

            idleTimer = 0;
            if (frontIsGround | backIsGround | leftIsGround | rightIsGround | isSliding)
            {
                isGrounded = true;
            }

            soulLegsAnim.enabled = false;
            endedJumpTimer = 0;
        }
        else if (isGrounded | move_Throws.isGroundedForSlide)
        {
            isGliding = false;

            if ((endedJumpTimer > 0 && endedJumpTimer < 20) | (endedJumpTimer == 0 && isJumping)) endedJumpTimer++;
            else endedJumpTimer = 0;

            if (finishedJumpCoroutine && afterDiveLandingTimer > 0 && afterDiveLandingTimer < 10)
            {
                rigidbody.velocity -= Vector3.up * 10;

                isJumping = false;
                isAirJumping = false;
                isLongJumping = false;
                if (!jump && !jumpHold && afterJumpTimer > 5)
                {
                    switch (moveForm)
                    {
                        case 1:
                        case 2:
                        case 3:
                            soulAnim.CrossFade("Player_LandingWalk", 0.05f);
                            break;
                        case 4:
                        case 5:
                        case 6:
                            soulAnim.CrossFade("Player_LandingRun", 0.05f);
                            break;
                        default:
                            soulAnim.CrossFade("Player_LandingIdle", 0.05f);
                            break;
                    }
                }
            }
            afterGlideTimer = 10;
            airbornedTimer = 0;
            tryingToClimbTimer = 0;
            afterWallSlideTimer = 0;

            if (!isJumping)
            {
                afterSlopeJumpTimer = 0;
                afterUnclimbableSlopeJumpTimer = 0;
            }
            if (isDashing | (afterDiveLandingTimer > 0 && afterDiveLandingTimer < 12) | landed)
            {
                soulLegsAnim.enabled = false;
            }
            else
            {
                collider.radius = 0.35f;

                soulLegsAnim.enabled = true;
            }

            if (collider.height < 2.17f)
            {
                collider.height += 0.2f;
            }
            if (collider.center.y < 0)
            {
                collider.center += new Vector3(0, 0.1f, 0);
            }

            groundCollider.enabled = false;

            foreach (TailAnimator2 anim in animUpdater.frontClothTailAnims)
            {
                anim.TailAnimatorAmount = 0.5f;
            }

            if (!isDashing && !landed && !isJumping && moveForm == 0 && (idleTimer <= idleTimerLimit)) idleTimer++;

            if (isDashing | isJumping | moveForm > 0 | (currentAnimation.normalizedTime >= 1.5f && idleValue > 1)
                | (currentAnimation.normalizedTime >= 1.55f && idleValue == 11)
                | (idleValue >= 1 && (isThrowing | altThrowing)) | (eventTextAction.isActive && idleValue > 1))
            {
                idleTimer = 0;
                idleValue = 0;
            }
            else if (idleTimer == idleTimerLimit)
            {
                idleValue = Random.Range(1, 4);
            }
        }

        if (lerpSpeedTimer >= 15 && currentLerpSpeed < normalHorizontalLerpSpeed)
        {
            currentLerpSpeed++;
            lerpSpeedTimer = 0;
        }

        LerpCamera();

        if (isGliding)
        {
            dampSpeedUp = dampGlideSpeedUp;

            if (airbornedTimer < 60)
            {
                foreach (TrailRenderer trailRenderer in glidingTrails)
                {
                    trailRenderer.Clear();
                    trailRenderer.enabled = false;
                }
            }
            else
            {
                foreach (TrailRenderer trailRenderer in glidingTrails)
                {
                    trailRenderer.enabled = true;
                }
            }

            if (afterGlidingTrailTimer <= 30)
            {
                afterGlidingTrailTimer++;

                if (afterGlidingTrailTimer > 22)
                {
                    foreach (TrailRenderer trailRenderer in glidingTrails)
                    {
                        trailRenderer.time += 0.05f;
                    }
                }
            }
        }
        else
        {
            dampSpeedUp = initialDampSpeedUp;

            if (afterGlidingTrailTimer <= 7)
            {
                afterGlidingTrailTimer++;
                foreach (TrailRenderer trailRenderer in glidingTrails)
                {
                    trailRenderer.time -= 0.05f;
                }
            }
            else
            {
                foreach (TrailRenderer trailRenderer in glidingTrails)
                {
                    trailRenderer.Clear();
                    trailRenderer.enabled = false;
                }
            }
        }

        if ((isGrounded | move_Throws.isGroundedForSlide) && !isAirDashing && (!wasAirborned | (isJumping && isCloseToCeilingForJump) | (isJumping && afterUnclimbableSlopeJumpTimer == 0 && isSlidingDown)
            /*((currentAnimation.IsName("Player_Jump") | currentAnimation.IsName("Player_JumpAlternate")) && currentAnimation.normalizedTime > 0.7f)*/ ))
        {
            if (!isCloseToCeilingForJump)
            {
                if (isDiving | additionalGravity > 120)
                {
                    afterDiveLandingTimer++;

                    Instantiate<ParticleSystem>(diveLandingEffect3D, characterModel.transform.position, neutralRotation);

                    if (cameraShake && !isRespawning) impulseSource.GenerateImpulse(1.5f);
                }
                else
                {
                    Instantiate<ParticleSystem>(landingEffect3D, characterModel.transform.position, neutralRotation);
                }
            }
            if (!((currentAnimation.IsName("Player_Jump") | currentAnimation.IsName("Player_JumpAlternate") |
                currentAnimation.IsName("Player_SlideJump") | currentAnimation.IsName("Player_FrontSlideJump")) &&
                currentAnimation.normalizedTime > 0.7f)) wasAirborned = true;
            isJumping = false;
            isAirJumping = false;
            isLongJumping = false;
            if (afterGroundedDashTimer > 30)
            {
                isDashing = false;
            }
            fixedDirectionDash = false;
            afterDashTimer = 0;
            dashRecoilTimer = 0;
            afterLandingTimer++;
        }


        if (((currentAnimation.IsName("Player_Jump") | currentAnimation.IsName("Player_JumpAlternate")) &&
            currentAnimation.normalizedTime > 0.7f) |
            ((currentAnimation.IsName("Player_SlideJump") | currentAnimation.IsName("Player_FrontSlideJump")) &&
            currentAnimation.normalizedTime > 0.5f) | (isCloseToWall && moveForm > 0))
        {
            isJumping = false;
            isAirJumping = false;
            isLongJumping = false;
            if (isGrounded && !(isCloseToWall && moveForm > 0))
            {
                afterLandingTimer = 1;
                switch (moveForm)
                {
                    case 1:
                    case 2:
                    case 3:
                        soulAnim.CrossFade("Player_LandingWalk", 0.05f);
                        break;
                    case 4:
                    case 5:
                    case 6:
                        soulAnim.CrossFade("Player_LandingRun", 0.05f);
                        break;
                    default:
                        soulAnim.CrossFade("Player_LandingIdle", 0.05f);
                        break;

                }
            }
        }
        /*
    	if ((isGrounded | move_MaskPower.isGroundedForSlide) && !wasAirborned && finishedJumpCoroutine && !isAirDashing)
    	{
        	isJumping = false;
    	}*/

        if (afterLandingTimer > 0 && afterLandingTimer <= 12)
        {
            afterLandingTimer++;
            landed = true;

            jumpingTrail.enabled = false;

            if (afterLandingTimer == 12)
            {
                isBouncing = false;
                ledgeJump = false;
            }

            afterGroundedDashTimer = 30;
            if (afterDiveLandingTimer == 0 && afterLandingTimer < 7)
            {
                stretchAnimationScript.DoStretch("StretchLandAnimation");
                soulAnim.CrossFade("Player_Scale_StretchLand", 0, 9);
            }
            else if (isCloseToCeilingForJump && !isDashing)
            {
                switch (moveForm)
                {
                    case 1:
                    case 2:
                    case 3:
                        soulAnim.CrossFade("Player_LandingWalk", 0.05f);
                        break;
                    case 4:
                    case 5:
                    case 6:
                        soulAnim.CrossFade("Player_LandingRun", 0.05f);
                        break;
                    default:
                        soulAnim.CrossFade("Player_LandingIdle", 0.05f);
                        break;

                }
            }
        }
        else
        {
            afterLandingTimer = 0;
            landed = false;
            leftGroundDuringDash = false;
        }

        if (afterDiveLandingTimer == 1)
        {
            stretchAnimationScript.DoStretch("StretchDiveLandAnimation");
            soulAnim.CrossFade("Player_Scale_StretchDiveLand", 0, 9);
        }

        if (afterDiveLandingTimer > 0 && afterDiveLandingTimer <= 12)
        {
            afterDiveLandingTimer++;
            afterGroundedDashTimer = 30;
            //rigidbody.isKinematic = true;
        }
        else
        {
            afterDiveLandingTimer = 0;
            rigidbody.isKinematic = false;
        }

        if (isDashing)
        {
            afterDashTimer++;
        }

        speedBoostHitbox.SetActive(runBoostTimer >= runBoostTimerLimit);

        if (moveForm == 6 && itemsHandler.enableSpeedBoost && !move_Throws.isGettingDragged &&
            !isSlidingDown && !ledgeJump && !isWallSliding && !isGroundDashing && !isCloseToWall)
        {
            if (isGrounded && runBoostTimer < runBoostTimerLimit)
            {
                runBoostTimer++;
            }
        }
        else
        {
            runBoostTimer = 0;
        }

        if (isAirDashing) isGrounded = false;

        if (afterDashTimer >= 18 | (afterDashTimer >= 8 && isAirDashing && CheckGroundedWithDash()))
        {
            isDashing = false;
            isAirDashing = false;
            isGroundDashing = false;
            fixedDirectionDash = false;
            dashRecoilTimer++;
            afterDashTimer = 0;
        }

        if (dashRecoilTimer > 0 && dashRecoilTimer < 8)
        {
            dashRecoilTimer++;
        }
        else
        {
            dashRecoilTimer = 0;
        }

        if (isGroundDashing && !isGrounded) leftGroundDuringDash = true;

        //characterModelRotationSmooth = isWallSliding || isDashing ? 100 : (!isGliding ? initialcharacterModelRotationSmooth : 0.1f);

        if (isWallSliding || isDashing)
        {
            characterModelRotationSmooth = 100;
            ledgeJump = false;
        }
        else if (isGliding)
        {
            characterModelRotationSmooth = glideRotationSmooth;
            ledgeJump = false;
        }
        else if (isGrounded && isSliding)
        {
            characterModelRotationSmooth = slideRotationSmooth;
        }
        else characterModelRotationSmooth = initialcharacterModelRotationSmooth;

        if (additionalGravity <= -210)
        {
            isDiving = true;
            ledgeJump = false;

            if (additionalGravity > -270) diveTrailEffect1.Play();
            else if (additionalGravity > -330) diveTrailEffect2.Play();
            else diveTrailEffect3.Play();
        }
        else
        {
            isDiving = false;
            diveTrailEffect1.Stop();
            diveTrailEffect2.Stop();
            diveTrailEffect3.Stop();
        }

        if (cameraSwitchTimer >= 0 && cameraSwitchTimer <= 20) cameraSwitchTimer++;

        if (afterDashTimer >= 6 && isGroundDashing && leftGroundDuringDash && landed)
        {
            if (stickIncline == 0)
            {
                soulAnim.CrossFade("Player_LandingIdleDash", 0.05f, 0, 0.3f);
            }
            else if (stickIncline >= 0.55f)
            {
                soulAnim.CrossFade("Player_LandingRunDash", 0.05f, 0, 0.3f);
            }
            else
            {
                soulAnim.CrossFade("Player_LandingWalkDash", 0.05f, 0, 0.3f);
            }
            isGroundDashing = false;
        }
        else if (afterDashTimer >= 16 && isAirDashing && landed)
        {
            if (stickIncline == 0)
            {
                soulAnim.CrossFade("Player_LandingIdleDash", 0.05f, 0, 0.15f);
            }
            else if (stickIncline >= 0.55f)
            {
                soulAnim.CrossFade("Player_LandingRunDash", 0.05f, 0, 0.15f);
            }
            else
            {
                soulAnim.CrossFade("Player_LandingWalkDash", 0.05f, 0, 0.15f);
            }
            isAirDashing = false;
        }

        AnimUpdate();

        /*
    	IEnumerator LerpToPosition(Vector3 newPosition)
    	{
        	/*if (useRelativeSpeed)
        	{
            	float totalDistance = farRight.position.x - farLeft.position.x;
            	float diff = transform.position.x - farLeft.position.x;
            	float multiplier = diff / totalDistance;
            	lerpSpeed *= multiplier;
        	}*/
        /*
        	float time = 0.0f;
        	Vector3 startingPos = cameraTarget.transform.position;
        	while (time < 1.0f)
        	{
            	time += Time.deltaTime * (Time.timeScale / normalLerpSpeed);
            	//time += lerpSpeed / 100;

            	cameraTarget.transform.position = Vector3.Lerp(startingPos, newPosition, time);
        	}

        	yield return 0;
    	}*/
    }

    #region Checks

    private void CheckGrounded()
    {
        prevGrounded = isGrounded;

        if (!(frontIsGround | backIsGround | leftIsGround | rightIsGround) && !move_Throws.isGettingDragged)
        {
            isGrounded = Physics.Raycast(groundDetecter.transform.position, -Vector3.up, customDistToGround, groundMask);
            move_Throws.isGroundedForSlide = false;
        }
        else if (move_Throws.isGettingDragged && !move_Throws.isHoldingToRope)
        {
            isGrounded = false;
            move_Throws.isGroundedForSlide = Physics.Raycast(groundDetecter.transform.position, -Vector3.up, distToGround + customDistToGroundForDrag, groundMask);
            if (move_Throws.isGroundedForSlide)
            {
                if (finishedJumpCoroutine)
                {
                    isJumping = false;
                    isAirJumping = false;
                    isLongJumping = false;
                }
            }
        }
    }

    private void CheckGroundBelow()
    {
        if (!isGrounded) isGroundJustBelow = Physics.Raycast(groundDetecter.transform.position, -Vector3.up, distToGround + 4.2f, groundMask);
        else isGroundJustBelow = false;

        if (!isGrounded) isGroundBelow = Physics.Raycast(groundDetecter.transform.position, -Vector3.up, distToGround + 11.2f, groundMask);
        else isGroundBelow = false;
    }

    private bool CheckGroundedWithDash()
    {
        return Physics.Raycast(groundDetecter.transform.position, -Vector3.up, customDistToGround, groundMask);
    }

    private void CheckFarFrontGround()
    {
        farFrontIsGround = Physics.Raycast(farFrontPoint.transform.position, -Vector3.up, distToGround + customDistToGround + 2, groundMask);
    }

    private void CheckFrontGround()
    {
        if (move_Throws.isGettingDragged && !move_Throws.isHoldingToRope)
        {
            frontIsGround = false;
        }
        else
        {
            frontIsGround = Physics.Raycast(frontPoint.transform.position, -Vector3.up, customDistToGround, groundMask);
        }
    }

    private void CheckBackGround()
    {
        if (move_Throws.isGettingDragged && !move_Throws.isHoldingToRope)
        {
            backIsGround = false;
        }
        else
        {
            backIsGround = Physics.Raycast(backPoint.transform.position, -Vector3.up, customDistToGround, groundMask);
        }
    }

    private void CheckLeftGround()
    {
        if (move_Throws.isGettingDragged && !move_Throws.isHoldingToRope)
        {
            leftIsGround = false;
        }
        else
        {
            leftIsGround = Physics.Raycast(leftPoint.transform.position, -Vector3.up, customDistToGround, groundMask);
        }
    }

    private void CheckRightGround()
    {
        if (move_Throws.isGettingDragged && !move_Throws.isHoldingToRope)
        {
            rightIsGround = false;
        }
        else
        {
            rightIsGround = Physics.Raycast(rightPoint.transform.position, -Vector3.up, customDistToGround, groundMask);
        }
    }

    private void CheckJumpOnTrampoline()
    {
        isOnTrampoline = Physics.Raycast(groundDetecter2.transform.position, -Vector3.up, distToGround + customDistToTrampoline, enemyMask);
    }

    private void CheckLedgeGrab()
    {
        bool tmpWall = false;
        Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + heightWallCheckerTop, transform.position.z);
        Vector3 middleWallPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 bottomWallPos = new Vector3(transform.position.x, transform.position.y - heightWallCheckerBottom, transform.position.z);
        RaycastHit ledgeHit;

        float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
        Vector3[] wallPositions = { topWallPos, middleWallPos, bottomWallPos };

        Quaternion wallHitQuaternion;
        foreach (Vector3 wallPos in wallPositions)
        {
            foreach (float angle in angles)
            {
                Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * globalForward;
                if (Physics.Raycast(wallPos, direction, out ledgeHit, wallCheckerThrashold, ledgeMask))
                {
                    wallHitQuaternion = Quaternion.LookRotation(ledgeHit.normal);
                    wallNormal = ledgeHit.normal;
                    tmpWall = true;

                    if (tmpWall && !isGrounded && stickIncline > 0 && canLedgeJump && !move_Throws.isGettingDragged && (afterJumpTimer > 10 | afterJumpTimer == 0))
                    {
                        if (Vector3.Angle(wallNormal, -globalForward) < 44f)
                        {
                            for (int i = 0; i < ledgePointArray.Length; i++)
                            {
                                isCloseToLedgeArray[i] = Physics.Raycast(ledgePointArray[i].position, forward, distToLedge + wallCheckerThrashold + 1, ledgeMask);
                            }
                            if (isWallSlidingTimer < 10) isWallSlidingTimer++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ledgePointArray.Length; i++)
                        {
                            isCloseToLedgeArray[i] = false;
                        }
                    }

                    if (tmpWall && !isGrounded && !isDashing && stickIncline > 0 && (afterJumpTimer > 10 | afterJumpTimer == 0))
                    {
                        Quaternion forwardQuaternion = Quaternion.Euler(Vector3.forward);

                        for (int i = 0; i < ledgePointArray.Length - 1; i++)
                        {
                            if (isCloseToLedgeArray[i] && !isCloseToLedgeArray[i + 1])
                            {
                                afterJumpTimer = 1;
                                isWallSlidingTimer = 1;
                                isGliding = false;
                                ledgeJump = true;
                                isJumping = true;
                                airbornedTimer = 1;
                                additionalGravity = 0;
                                tryingToClimbTimer = 0;
                                //characterModel.transform.rotation = Quaternion.Euler(-ledgeHit.normal);

                                if (afterWallJumpTimer < 20)
                                {
                                    soulAnim.CrossFade("Player_LedgeJump", 0.05f, 0);
                                }
                                else
                                {
                                    //soulAnim.CrossFade("Player_LedgeJumpRotate", 0.05f, 0);
                                    soulAnim.CrossFade("Player_LedgeJump", 0.05f, 0);
                                }

                                stretchAnimationScript.DoStretch("StretchLedgeJumpAnimation");
                                soulAnim.CrossFade("Player_Scale_StretchLedgeJump", 0, 9);

                                rigidbody.velocity = forward * 0.05f + Vector3.up * ledgeJumpPowerMultiplierArray[i];
                            }
                        }
                    }
                }
            }
        }
    }

    private void CheckCeiling()
    {
        if (Physics.Raycast(groundDetecter.transform.position, Vector3.up, distToGround + distToCeiling, groundMask) &&
            (Physics.Raycast(frontPoint.transform.position, Vector3.up, distToGround + distToCeiling, groundMask) |
            Physics.Raycast(backPoint.transform.position, Vector3.up, distToGround + distToCeiling, groundMask) |
            Physics.Raycast(leftPoint.transform.position, Vector3.up, distToGround + distToCeiling, groundMask) |
            Physics.Raycast(rightPoint.transform.position, Vector3.up, distToGround + distToCeiling, groundMask)))
        {
            isCloseToCeiling = true;
        }
        else isCloseToCeiling = false;

        isCloseToCeilingForJump = Physics.Raycast(cameraTarget.transform.position, Vector3.up, distToGround + distToCeilingForJump, groundMask);
        isCloseToCeilingForStretch = Physics.Raycast(cameraTarget.transform.position, Vector3.up, distToGround + distToCeilingForStretch, groundMask);
        isCloseToCeilingForDrag = Physics.Raycast(cameraTarget.transform.position, Vector3.up, distToGround + distToCeilingForDrag, groundMask);
    }
    public bool CheckCeilingForGHEnd()
    {
        if (Physics.Raycast(groundDetecter.transform.position, Vector3.up, distToGround + distToCeiling + 1.5f, groundMask) &&
            Physics.Raycast(frontPoint.transform.position, Vector3.up, distToGround + distToCeiling + 1.5f, groundMask) &&
            Physics.Raycast(backPoint.transform.position, Vector3.up, distToGround + distToCeiling + 1.5f, groundMask) &&
            Physics.Raycast(leftPoint.transform.position, Vector3.up, distToGround + distToCeiling + 1.5f, groundMask) &&
            Physics.Raycast(rightPoint.transform.position, Vector3.up, distToGround + distToCeiling + 1.5f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckStep()
    {
        bool tmpStep = false;
        Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);

        RaycastHit stepLowerHit;
        if (Physics.Raycast(bottomStepPos, globalForward, out stepLowerHit, stepCheckerThrashold, groundMask))
        {
            RaycastHit stepUpperHit;
            if (RoundValue(stepLowerHit.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), globalForward, out stepUpperHit, stepCheckerThrashold + 0.05f, groundMask))
            {
                //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                tmpStep = true;
            }
        }

        RaycastHit stepLowerHit45;
        if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out stepLowerHit45, stepCheckerThrashold, groundMask))
        {
            RaycastHit stepUpperHit45;
            if (RoundValue(stepLowerHit45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(45, Vector3.up) * globalForward, out stepUpperHit45, stepCheckerThrashold + 0.05f, groundMask))
            {
                //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                tmpStep = true;
            }
        }

        RaycastHit stepLowerHitMinus45;
        if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(-45, transform.up) * globalForward, out stepLowerHitMinus45, stepCheckerThrashold, groundMask))
        {
            RaycastHit stepUpperHitMinus45;
            if (RoundValue(stepLowerHitMinus45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(-45, Vector3.up) * globalForward, out stepUpperHitMinus45, stepCheckerThrashold + 0.05f, groundMask))
            {
                //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                tmpStep = true;
            }
        }

        isTouchingStep = tmpStep;
    }

    private void CheckWall()
    {
        bool tmpWall = false;
        //Vector3 tmpWallNormal = Vector3.zero;
        Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + heightWallCheckerTop, transform.position.z);
        Vector3 middleWallPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 bottomWallPos = new Vector3(transform.position.x, transform.position.y - heightWallCheckerBottom, transform.position.z);
        RaycastHit wallHit;

        if (Physics.Raycast(topWallPos, globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(90, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(135, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(180, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(225, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(270, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(315, transform.up) * globalForward, out wallHit, wallCheckerThrashold, wallMask))
        {
            wallNormal = wallHit.normal;
            tmpWall = true;
        }

        Vector3 wallPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (isGrounded && stickIncline >= 0.25)
        {
            if (Physics.Raycast(bottomWallPos, globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask) |
                Physics.Raycast(middleWallPos, globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask) |
                Physics.Raycast(topWallPos, globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask))
            {
                stickIncline = 0.25f;
                moveForm = 1;
                isCloseToWall = true;
            }
            else if (Physics.Raycast(bottomWallPos, Quaternion.AngleAxis(10, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask) |
                Physics.Raycast(middleWallPos, Quaternion.AngleAxis(10, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask) |
                Physics.Raycast(topWallPos, Quaternion.AngleAxis(10, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask))
            {
                stickIncline = 0.25f;
                moveForm = 1;
                isCloseToWall = true;
            }
            else if (Physics.Raycast(bottomWallPos, Quaternion.AngleAxis(350, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask) |
                Physics.Raycast(middleWallPos, Quaternion.AngleAxis(350, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask) |
                Physics.Raycast(topWallPos, Quaternion.AngleAxis(350, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForGround, wallMask))
            {
                stickIncline = 0.25f;
                moveForm = 1;
                isCloseToWall = true;
            }
            else
            {
                isCloseToWall = false;
            }
        }
        else
        {
            isCloseToWall = false;
        }

        if (stickIncline >= 0.25 && !isDashing && !isGliding && !isDiving && !isJumping)
        {
            if (Physics.Raycast(topWallPos, globalForward, out wallHit, wallCheckerThrasholdForAnim, wallMask))
            {
                isCloseToWallForAir = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(10, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForAnim, wallMask))
            {
                isCloseToWallForAir = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(350, transform.up) * globalForward, out wallHit, wallCheckerThrasholdForAnim, wallMask))
            {
                isCloseToWallForAir = true;
            }
            else
            {
                isCloseToWallForAir = false;
            }
        }
        else
        {
            isCloseToWallForAir = false;
        }

        if (isWallSliding && !ledgeJump)
        {
            characterModel.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -wallHit.normal);
            targetAngle = characterModel.transform.rotation.y;
            lastRotationAngle = characterModel.transform.rotation.y;

            isJumping = true;
            afterJumpTimer = 100;
            isThrowing = false;
            altThrowing = false;

            soulAnim.CrossFade("Player_WallSlide", 0, 0);
            soulAnim.CrossFade("Player_WallSlide", 0.05f, 3);
        }

        if (isWallSliding)
        {
            Vector3 pushDirection = wallHit.normal * -1;
            rigidbody.AddForce(pushDirection * 0.5f, ForceMode.Impulse);
        }

        isTouchingWall = tmpWall;
    }

    private void CheckSlopeAndDirections()
    {
        prevGroundNormal = groundNormal;

        RaycastHit slopeHit;
        if (Physics.SphereCast(transform.position, slopeCheckerThrashold, Vector3.down, out slopeHit, originalColliderHeight / 2f + 0.5f, groundMask))
        {
            groundNormal = slopeHit.normal;

            if (slopeHit.normal.y == 1 | slopeHit.normal.y == 0)
            {

                forward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                globalForward = forward;
                reactionForward = forward;

                SetFriction(frictionAgainstFloor, true);
                currentLockOnSlope = lockOnSlope;

                currentSurfaceAngle = 0f;
                isTouchingSlope = false;

                isSlidingDown = false;
                afterNormalSlopeLeavingTimer = 0;
            }
            else
            {
                //set forward
                Vector3 tmpGlobalForward = transform.forward.normalized;
                Vector3 tmpForward = new Vector3(tmpGlobalForward.x, Vector3.ProjectOnPlane(transform.forward.normalized, slopeHit.normal).normalized.y, tmpGlobalForward.z);
                Vector3 tmpReactionForward = new Vector3(tmpForward.x, tmpGlobalForward.y - tmpForward.y, tmpForward.z);

                if (currentSurfaceAngle <= maxClimbableSlopeAngle && !isTouchingStep)
                {
                    //set forward
                    forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                    globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                    reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);

                    soulGroundRotator.MaxForwardRotation = 20;
                    SetFriction(frictionAgainstFloor, true);
                    currentLockOnSlope = lockOnSlope;
                    isSlidingDown = false;
                    afterNormalSlopeLeavingTimer = 0;
                }
                else if (isTouchingStep)
                {
                    //set forward
                    forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                    globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                    reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);

                    soulGroundRotator.MaxForwardRotation = 20;
                    SetFriction(frictionAgainstFloor, true);
                    currentLockOnSlope = true;
                    isSlidingDown = false;
                    afterNormalSlopeLeavingTimer = 0;
                }
                else if (!ledgeJump && !isAirDashing)
                {
                    //set forward
                    forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                    globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                    reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);

                    targetAngle = tmpGlobalForward.y;
                    SetFriction(0f, true);
                    currentLockOnSlope = lockOnSlope;
                    if (!isGrounded | landed)
                    {
                        isGrounded = true;
                        additionalGravity = 0;
                        isDiving = false;
                    }

                    if (afterNormalSlopeLeavingTimer < 12) afterNormalSlopeLeavingTimer++;

                    if (((Physics.Raycast(frontSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes, groundMask) &&
                        !Physics.Raycast(backSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes, groundMask) &&
                        Physics.Raycast(leftSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes, groundMask) &&
                        Physics.Raycast(rightSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes, groundMask)) |
                        rigidbody.velocity.magnitude > 26) && (afterNormalSlopeLeavingTimer >= 12 | currentAnimation.IsName("Player_LandingIdle") |
                        currentAnimation.IsName("Player_LandingWalk") | currentAnimation.IsName("Player_LandingRun")))
                    {
                        soulGroundRotator.MaxForwardRotation = 90;
                        isSlidingDown = true;
                        additionalGravity = 0;

                        Vector3 slopeDirection = new Vector3(slopeHit.normal.x, 0, slopeHit.normal.z).normalized;
                        //Debug.Log(slopeDot);
                        if (!isDashing && !isJumping && !currentAnimation.IsName("Player_Slide") &&
                        !currentAnimation.IsName("Player_FrontSlide"))
                        {
                            if (slopeDot <= 0)
                            {
                                slideInFront = false;
                                soulAnim.CrossFade("Player_Slide", 0.025f);
                                targetAngle = Mathf.Atan2(slopeDirection.x, slopeDirection.z) * -Mathf.Rad2Deg;
                                lastRotationAngle = Mathf.Atan2(slopeDirection.x, slopeDirection.z) * -Mathf.Rad2Deg;
                            }
                            else
                            {
                                slideInFront = true;
                                soulAnim.CrossFade("Player_FrontSlide", 0.1f);
                                targetAngle = Mathf.Atan2(slopeDirection.x, slopeDirection.z) * Mathf.Rad2Deg;
                                lastRotationAngle = Mathf.Atan2(slopeDirection.x, slopeDirection.z) * Mathf.Rad2Deg;
                            }
                            //soulAnim.CrossFade("Player_Slide", 0, 0);
                        }

                    }
                    if (isSlidingDown)
                    {
                        Instantiate<ParticleSystem>(slideEffect3D, characterModel.transform.position, neutralRotation);
                    }

                    slopeJumpDirection = new Vector3(slopeHit.normal.x, 0, slopeHit.normal.z);
                }

                currentSurfaceAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                isTouchingSlope = true;

                slopeDot = Vector3.Dot(slopeHit.normal, tmpForward);

                Vector3 movement = new Vector3(axisInput.x, 0.0f, axisInput.y).normalized;

                movement = transform.TransformDirection(movement);

                if ((jump | inputReader.currentJumpTimer > 0) && jumpHoldTimer == 0 && !ledgeJump && currentSurfaceAngle > 28 && !isWallSliding && afterJumpTimer < 6 && afterDiveLandingTimer == 0)
                {
                    afterSlopeJumpTimer = 1;
                }
                if ((jump | inputReader.currentJumpTimer > 0) && jumpHoldTimer == 0 && !ledgeJump && currentSurfaceAngle > maxClimbableSlopeAngle && !isWallSliding && afterJumpTimer < 6 && afterDiveLandingTimer == 0)
                {
                    afterUnclimbableSlopeJumpTimer = 1;
                }
            }

            //set down
            down = Vector3.Project(Vector3.down, slopeHit.normal);
            globalDown = Vector3.down.normalized;
            reactionGlobalDown = Vector3.up.normalized;
        }
        else
        {
            groundNormal = Vector3.zero;

            forward = Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
            globalForward = forward;
            reactionForward = forward;

            //set down
            down = Vector3.down.normalized;
            globalDown = Vector3.down.normalized;
            reactionGlobalDown = Vector3.up.normalized;

            SetFriction(frictionAgainstFloor, true);
            currentLockOnSlope = lockOnSlope;

            isSliding = false;
            isSlidingDown = false;
            //afterNormalSlopeLeavingTimer = 0;
            afterSlideTimer = 0;
            soulGroundRotator.MaxForwardRotation = 20;
            isTouchingSlope = false;

            if (!move_Throws.isGettingDragged)
            {
                int n = 0;
                if (Physics.Raycast(frontSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes * 2, groundMask)) n++;
                if (Physics.Raycast(backSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes * 2, groundMask)) n++;
                if (Physics.Raycast(leftSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes * 2, groundMask)) n++;
                if (Physics.Raycast(rightSlopePoint.transform.position, -Vector3.up, distToGround + distToGroundForSlopes * 2, groundMask)) n++;

                if (isGroundJustBelow && additionalGravity < -240 && n > 0 && n < 4)
                {
                    isGrounded = true;
                    additionalGravity = 0;
                }
            }
        }
    }

    #endregion


    #region Move

    private void MoveWalk()
    {
        if (axisInput.magnitude > stickInclineDeadZone && (!move_Throws.isGettingDragged | (move_Throws.isGettingDragged && move_Throws.isHoldingToRope)))
        {
            if (!fixedDirectionDash)
            {
                if (isSliding)
                {
                    float angleCible = Mathf.Atan2(axisInput.x, axisInput.y) * Mathf.Rad2Deg + characterCamera.transform.eulerAngles.y;
                    targetAngle = Mathf.Lerp(targetAngle, angleCible, 0.1f);
                }
                else
                {
                    targetAngle = Mathf.Atan2(axisInput.x, axisInput.y) * Mathf.Rad2Deg + characterCamera.transform.eulerAngles.y;
                }
            }
            if (runBoostTimer >= runBoostTimerLimit)
            {
                Vector3 targetDirection = forward * movementSpeed * stickIncline;
                Vector3 smoothedDirection = Vector3.RotateTowards(rigidbody.velocity.normalized, targetDirection.normalized, 50 * Time.deltaTime, 0.0f);
                rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, smoothedDirection * targetDirection.magnitude, ref currVelocity, dampSpeedUp);
            }
            else rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, forward * movementSpeed * stickIncline, ref currVelocity, dampSpeedUp);

            if (isGrounded)
            {
                if (runBoostTimer >= runBoostTimerLimit)
                {
                    if (movementSpeed != boostedMovementSpeed) Instantiate(runBoostEffect, characterModel.transform);

                    movementSpeed = boostedMovementSpeed;
                }
                else
                {
                    if (currentAnimation.IsName("Player_RunBoost") && isCloseToWall)
                    {
                        soulAnim.CrossFade("Player_Walk 1", 0, 0);
                    }
                    movementSpeed = initialMovementSpeed;
                }
                particleMovementEffectTimer++;

                if (moveForm == 4 && particleMovementEffectTimer > 10)
                {
                    Instantiate<ParticleSystem>(movementEffect3D, characterModel.transform.position, neutralRotation);
                    particleMovementEffectTimer = 0;
                }
                else if (moveForm == 5 && particleMovementEffectTimer > 8)
                {
                    Instantiate<ParticleSystem>(movementEffect3D, characterModel.transform.position, neutralRotation);
                    particleMovementEffectTimer = 0;
                }
                else if (moveForm == 6 && runBoostTimer >= runBoostTimerLimit && particleMovementEffectTimer > 3)
                {
                    Instantiate<ParticleSystem>(movementEffect3D, characterModel.transform.position, neutralRotation);
                    particleMovementEffectTimer = 0;
                }
                else if (moveForm == 6 && particleMovementEffectTimer > 22)
                {
                    Instantiate<ParticleSystem>(movementEffect3D, characterModel.transform.position, neutralRotation);
                    particleMovementEffectTimer = 0;
                }
                else
                {
                    if (moveForm == 0) particleMovementEffectTimer = 0;
                }
            }
            else if (!isGrounded && isTouchingWall)
            {
                movementSpeed = wallMovementSpeed;
            }
            else if (ledgeJump)
            {
                movementSpeed = afterLedgeMovementSpeed;
            }
            else
            {
                if (runBoostTimer >= runBoostTimerLimit)
                {
                    movementSpeed = boostedMovementSpeed;
                }
                else
                {
                    if (currentAnimation.IsName("Player_RunBoost"))
                    {
                        if (isCloseToWall) soulAnim.CrossFade("Player_Walk 1", 0, 0);
                    }
                    runBoostTimer = 0;
                    movementSpeed = initialMovementSpeed;
                }
            }
        }
        else if (isGrounded && !isJumping)
        {
            if (inputReader.isMouseAndKeyboard)
            {
                if (stickIncline == 0 && !farFrontIsGround)
                {
                    rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currVelocity, dampSpeedDownLedgeKeyboard);

                }
                else rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currVelocity, dampSpeedDownKeyboard);
            }
            else
            {
                if (moveForm < 6 && !farFrontIsGround) rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currVelocity, dampSpeedDownLedge);
                else rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currVelocity, dampSpeedDown);
            }
        }
        else rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currVelocity, dampSpeedUp);
    }

    private void MoveRotation()
    {
        float angle;
        if ((currentAnimation.IsName("Player_LandingIdleDive") && stickIncline < stickInclineDeadZone) | !canMove | isSlidingDown) angle = lastRotationAngle;
        else if (isWallSliding | isDashing | isLongJumping) angle = Mathf.SmoothDampAngle(characterModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 1000000000000);
        else if (runBoostTimer >= runBoostTimerLimit) angle = Mathf.SmoothDampAngle(characterModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.3f);
        else angle = Mathf.SmoothDampAngle(characterModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, characterModelRotationSmooth);

        lastRotationAngle = angle;

        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        vfxThrowFolder.transform.rotation = Quaternion.Euler(0, characterModel.transform.rotation.eulerAngles.y, 0);

        /*if ((isThrowing | altThrowing) && !isDashing)
        {
            characterModel.transform.rotation = Quaternion.Euler(characterModel.transform.rotation.eulerAngles.x, characterCamera.transform.rotation.eulerAngles.y, characterModel.transform.rotation.eulerAngles.z);

            vfxFolder.transform.rotation = Quaternion.Euler(vfxFolder.transform.rotation.eulerAngles.x, characterModel.transform.rotation.eulerAngles.y, vfxFolder.transform.rotation.eulerAngles.z);
            afterHitEffect.transform.rotation = Quaternion.Euler(afterHitEffect.transform.rotation.eulerAngles.x, characterModel.transform.rotation.eulerAngles.y, afterHitEffect.transform.rotation.eulerAngles.z);
        }
        else*/ if ((isAirDashing && fixedDirectionDash) | (isDashing && move_Throws.endedDragCoroutineIsPlaying))
        {
            characterModel.transform.rotation = Quaternion.Euler(characterModel.transform.rotation.eulerAngles.x, rigidbody.transform.rotation.eulerAngles.y, characterModel.transform.rotation.eulerAngles.z);

            vfxFolder.transform.rotation = Quaternion.Euler(vfxFolder.transform.rotation.eulerAngles.x, characterModel.transform.rotation.eulerAngles.y, vfxFolder.transform.rotation.eulerAngles.z);
            afterHitEffect.transform.rotation = Quaternion.Euler(afterHitEffect.transform.rotation.eulerAngles.x, characterModel.transform.rotation.eulerAngles.y, afterHitEffect.transform.rotation.eulerAngles.z);
        }
        else
        {
            characterModel.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            vfxFolder.transform.rotation = Quaternion.Euler(vfxFolder.transform.rotation.eulerAngles.x, angle, vfxFolder.transform.rotation.eulerAngles.z);
            afterHitEffect.transform.rotation = Quaternion.Euler(afterHitEffect.transform.rotation.eulerAngles.x, angle, afterHitEffect.transform.rotation.eulerAngles.z);
        }
    }

    private void MoveJump()
    {
        if (inputReader.enableJump)
        {
            // Unclimbable slope jump
            if (jumpHoldTimer == 0 && isGrounded && airbornedTimer == 0 && isSlidingDown
                && (afterLandingTimer == 0 | afterLandingTimer > 1) && !ledgeJump && !isWallSliding && afterJumpTimer < 6 && afterJumpTimer != 1 && afterDiveLandingTimer == 0)
            {
                if (jump)
                {
                    RaycastHit slopeHit;
                    if (Physics.SphereCast(transform.position, slopeCheckerThrashold, Vector3.down, out slopeHit, distToGround + distToGroundForSlopes, groundMask))
                    {
                        slopeJumpDirection = new Vector3(slopeHit.normal.x, 0, slopeHit.normal.z);
                    }
                    rigidbody.velocity += slopeJumpDirection * horizontalJumpFromSlope + Vector3.up * verticalJumpFromSlope;

                    if (isSlidingDown) inputReader.currentJumpTimer = 0;
                    isJumping = true;
                    ledgeJump = false;

                    airbornedTimer = 20;
                    afterUnclimbableSlopeJumpTimer = 1;
                    afterJumpTimer = 1;
                    afterGlideTimer = 0;

                    stretchAnimationScript.DoStretch("StretchJumpAnimation");
                    soulAnim.CrossFade("Player_Scale_StretchSmallJump", 0, 9);

                    targetAngle = Mathf.Atan2(slopeHit.normal.x, slopeHit.normal.z) * Mathf.Rad2Deg;

                    forward = slopeHit.normal;
                    globalForward = forward;
                    reactionForward = forward;

                    Instantiate(jumpEffect3D, characterModel.transform.position, characterModel.transform.rotation);

                    if (slideInFront)
                    {
                        soulAnim.CrossFade("Player_FrontSlideJump", 0, 0);
                    }
                    else
                    {
                        soulAnim.CrossFade("Player_SlideJump", 0, 0);
                    }
                }
            }
            //jumped
            else if ((jump | inputReader.currentJumpTimer > 0) && jumpHoldTimer == 0 && (isGrounded | airbornedTimer < 8) && (afterLandingTimer == 0 | afterLandingTimer > 1) &&
                !isWallSliding && afterJumpTimer == 0 && afterDiveLandingTimer == 0 && !isBouncing && !canTalk && !canInteract && !isCloseToCeiling && !ledgeJump && !isSlidingDown &&
                (currentSurfaceAngle <= maxClimbableSlopeAngle | currentSurfaceAngle == 1 | (!isGrounded && afterNormalSlopeLeavingTimer == 0)))
            {
                if (!isBouncing) rigidbody.velocity += Vector3.up * jumpVelocity;

                inputReader.currentJumpTimer = 0;
                isJumping = true;
                ledgeJump = false;
                if (airbornedTimer > 0) isAirJumping = true;

                stretchAnimationScript.DoStretch("StretchJumpAnimation");
                soulAnim.CrossFade("Player_Scale_StretchSmallJump", 0, 9);

                jumpHoldTimer++;
                airbornedTimer = 0;

                StartCoroutine(WaitForLeavingGroundCoroutine());

                if (!isDashing && !isCloseToCeilingForJump)
                {
                    if (!alternateJumping)
                    {
                        alternateJumping = true;
                        soulAnim.CrossFade("Player_Jump", 0, 0);
                    }
                    else
                    {
                        alternateJumping = false;
                        soulAnim.CrossFade("Player_JumpAlternate", 0, 0);
                    }
                }
                else if (!isDashing && isCloseToCeilingForJump)
                {
                    switch (moveForm)
                    {
                        case 1:
                        case 2:
                        case 3:
                            soulAnim.CrossFade("Player_LandingWalk", 0.05f);
                            break;
                        case 4:
                        case 5:
                        case 6:
                            soulAnim.CrossFade("Player_LandingRun", 0.05f);
                            break;
                        default:
                            soulAnim.CrossFade("Player_LandingIdle", 0.05f);
                            break;
                    }
                }

                if (afterJumpTimer == 0)
                {
                    Instantiate<ParticleSystem>(jumpEffect3D, characterModel.transform.position, neutralRotation);
                }
            }
            else if (jumpHold && jumpHoldTimer > 0 && (isGrounded | airbornedTimer < 8) && !isWallSliding &&
                afterJumpTimer < 6 && afterDiveLandingTimer == 0 && !isBouncing && !isCloseToCeiling)
            {
                jumpHoldTimer++;
                afterJumpTimer = 3;

                if (jumpHoldTimer == 6)
                {
                    if (!isBouncing) rigidbody.velocity += Vector3.up * smallJumpVelocity;
                    isLongJumping = true;

                    stretchAnimationScript.DoStretch("StretchJumpAnimation");
                    soulAnim.CrossFade("Player_Scale_StretchJump", 0, 9);
                }
                else if (jumpHoldTimer > 6)
                {
                    jumpHoldTimer = 0;
                }
            }
            //jumped from wall
            else if ((jump | inputReader.currentJumpTimer > 0) && !isGrounded && isWallSliding && !ledgeJump && afterDashTimer == 0)
            {
                if (tryingToClimbTimer < tryingToClimbTimerLimit) rigidbody.velocity += wallNormal * horizontalJumpFromWallMultiplier + Vector3.up * verticalJumpFromWallMultiplier * multiplierVerticalLeap;
                else rigidbody.velocity += wallNormal * horizontalJumpFromWallMultiplier;

                inputReader.currentJumpTimer = 0;
                isJumping = true;
                wallJumpHoldTimer = 1;
                afterWallJumpTimer = 1;
                afterGlideTimer = 0;

                stretchAnimationScript.DoStretch("StretchJumpAnimation");
                soulAnim.CrossFade("Player_Scale_StretchSmallJump", 0, 9);

                targetAngle = Mathf.Atan2(wallNormal.x, wallNormal.z) * Mathf.Rad2Deg;

                forward = wallNormal;
                globalForward = forward;
                reactionForward = forward;

                Instantiate<ParticleSystem>(wallJumpEffect3D, characterModel.transform.position, characterModel.transform.rotation);

                soulAnim.CrossFade("Player_WallJump", 0, 0);
            }
            else if (jumpHold && wallJumpHoldTimer > 0 && !isGrounded && !isWallSliding && afterWallJumpTimer > 0 && afterWallJumpTimer < 10
                && !ledgeJump && afterDashTimer == 0)
            {
                wallJumpHoldTimer++;
                afterJumpTimer = 3;

                if (wallJumpHoldTimer == 6)
                {
                    rigidbody.velocity += wallNormal * smallHorizontalJumpFromWallMultiplier + Vector3.up * smallJumpVelocity;
                }
                else if (wallJumpHoldTimer > 6)
                {
                    wallJumpHoldTimer = 0;
                }
            }
            // Glide
            else if (jump && itemsHandler.enableGlide && !isGrounded && !landed && !isTouchingSlope && (afterWallJumpTimer == 0 | afterWallJumpTimer >= 15)
                && !isTouchingWall && !isGliding && airbornedTimer > 9 && afterGlideTimer >= 10
                && (afterUnclimbableSlopeJumpTimer == 0 | afterUnclimbableSlopeJumpTimer > 20))
            {
                if (isJumping && afterJumpTimer >= 10 || !isJumping)
                {
                    if (additionalGravity < -330) rigidbody.velocity += Vector3.up * verticalAirDashMultiplier;
                    else if (ledgeJump) rigidbody.velocity += forward * 5;
                    isGliding = true;
                    afterGlideTimer = 0;
                    afterGlidingTrailTimer = 0;

                    foreach (TrailRenderer trailRenderer in glidingTrails)
                    {
                        trailRenderer.Clear();
                        trailRenderer.time = 0;
                        trailRenderer.enabled = true;
                    }

                    if (!isCloseToWallForAir) soulAnim.CrossFade("Player_GlideFall", 0.0f, 0);
                    soulAnim.CrossFade("Player_GlideFall", 0.0f, 3);
                }
            }
            else if (jump && isGliding && !isGrounded && afterGlideTimer >= 10)
            {
                isGliding = false;
                afterGlideTimer = 0;
            }
            else
            {
                jumpHoldTimer = 0;
                wallJumpHoldTimer = 0;
            }
        }
    }

    public void MoveDash()
    {
        if (dash && itemsHandler.enableDash && !isDashing && canDash && !isCloseToCeiling && !isWallSliding && !isSlidingDown
            && (afterUnclimbableSlopeJumpTimer == 0 | afterUnclimbableSlopeJumpTimer > 20))
        {
            isDashing = true;
            canDash = false;
            move_PhantomDash.PhantomDash();

            if (isGrounded)
            {
                isGroundDashing = true;
                isJumping = false;
                isAirJumping = false;
                isLongJumping = false;
                afterGroundedDashTimer = 1;
                afterJumpTimer = 0;

                Instantiate<ParticleSystem>(landingEffect3D, characterModel.transform.position, neutralRotation);

                Instantiate<ParticleSystem>(dashParticles, characterModel.transform.position, characterModel.transform.rotation);

                rigidbody.velocity += forward * horizontalDashMultiplier;
                fixedDirectionDash = true;

                soulAnim.CrossFade("Player_DashNeutral", 0.05f, 0);

                GroundedDashTrailEffect.Play();
                GroundedDashTrailInitiateEffect.Play();

                Instantiate<ParticleSystem>(GroundedDashSmokeEffect3D, characterModel.transform.position, neutralRotation);
            }
            else
            {
                isAirDashing = true;
                isGliding = false;
                wasAirborned = true;

                if (move_Throws.dragReturnSpeedXZ > 0) rigidbody.velocity += forward * 10;
                rigidbody.velocity += forward * horizontalAirDashMultiplier + Vector3.up * verticalAirDashMultiplier;

                fixedDirectionDash = true;

                soulAnim.CrossFade("Player_DashRunAerial", 0.05f, 0);

                GroundedDashTrailEffect.Play();
                GroundedDashTrailInitiateEffect.Play();

                Instantiate<ParticleSystem>(aerialDashParticles, characterModel.transform.position, characterModel.transform.rotation);
            }
        }
        else
        {
            AerialNeutralTrailDashEffect.Stop();
            GroundedDashTrailEffect.Stop();
            GroundedDashTrailInitiateEffect.Stop();

            if (isWallSliding | (!isDashing && isGrounded && frontIsGround && backIsGround && leftIsGround && rightIsGround)) canDash = true;
        }
    }

    private void MoveMaskPower()
    {/*
        if ((isThrowing | altThrowing) && !isDashing)
        {
            characterModel.transform.rotation = Quaternion.Euler(characterModel.transform.rotation.eulerAngles.x, characterCamera.transform.rotation.eulerAngles.y, characterModel.transform.rotation.eulerAngles.z);

            vfxFolder.transform.rotation = Quaternion.Euler(vfxFolder.transform.rotation.eulerAngles.x, characterCamera.transform.rotation.eulerAngles.y, vfxFolder.transform.rotation.eulerAngles.z);
            afterHitEffect.transform.rotation = Quaternion.Euler(afterHitEffect.transform.rotation.eulerAngles.x, characterCamera.transform.rotation.eulerAngles.y, afterHitEffect.transform.rotation.eulerAngles.z);
        }*/

        move_Throws.Throws();
    }

    private void MoveCameraSwitch()
    {
        move_CameraAim.CameraAim();

        if (cameraSwitch && cameraSwitchTimer > 20) cameraSwitchTimer = 0;
        else if (cameraSwitch) cameraRecenter.recenter = true;
        else
        {
            cameraSwitcher.ChangeCamera(false);
        }
    }

    #endregion


    public void StartGame(float time)
    {
        StartCoroutine(StartGameCoroutine(time));
    }


    #region Coroutines

    public IEnumerator StartGameCoroutine(float time)
    {
        isRespawning = true;
        canMove = false;
        rigidbody.velocity = Vector3.zero;
        isDashing = false;
        isGliding = false;
        if (SaveManager.Instance.startedGame) additionalGravity = -210;
        else additionalGravity = 0;

        isDiving = SaveManager.Instance.startedGame;
        isAirDashing = false;
        dashRecoilTimer = 0;
        afterDashTimer = 0;

        inputReader.enableDash = false;
        inputReader.enableJump = false;
        inputReader.enableCameraInput = false;
        inputReader.enableCameraSwitch = false;
        inputReader.enableYoyo = false;
        pauseManager.canBePaused = false;
        inputProvider.enabled = false;

        tryingToClimbTimer = 0;

        characterModelRotationSmooth = initialcharacterModelRotationSmooth;

        zoomHandlerTalk.Zoom(false, false);

        cameraFreeLookController.cinemachineFreeLook.m_XAxis.Value = healthManager.respawnRotation.eulerAngles.y;
        cameraFreeLookController.cinemachineFreeLook.m_YAxis.Value = 0.5f;

        targetAngle = healthManager.respawnRotation.eulerAngles.y;
        lastRotationAngle = healthManager.respawnRotation.eulerAngles.y;

        if (SaveManager.Instance.startedGame) soulAnim.CrossFade("Player_Dive", 0);

        soulAnim.SetBool("isCloseToDamage", false);
        soulAnim.SetBool("isCloseToBoss", false);
        soulAnim.SetBool("isRespawning", SaveManager.Instance.startedGame);

        yield return new WaitForEndOfFrame();

        targetAngle = healthManager.respawnRotation.eulerAngles.y;
        lastRotationAngle = healthManager.respawnRotation.eulerAngles.y;
        pauseManager.navigationIcon.SetActive(true);
        pauseManager.navigationIconAnim.CrossFade("NavigationIcon_IdleOff", 0);
        kiteAnim.updateMode = AnimatorUpdateMode.Normal;

        yield return new WaitForSeconds(0.1f);

        targetAngle = healthManager.respawnRotation.eulerAngles.y;
        lastRotationAngle = healthManager.respawnRotation.eulerAngles.y;
        rigidbody.velocity += Vector3.up * 10;
        rigidbody.isKinematic = false;

        yield return new WaitForSeconds(time / 4);

        if (SaveManager.Instance.startedGame)
        {
            foreach (ParticleSystem effect in respawnEffects)
            {
                effect.Play();
            }
        }

        yield return new WaitForSeconds(time / 4 * 3);

        if (SaveManager.Instance.startedGame)
        {
            canMove = true;
            inputReader.enableDash = true;
            inputReader.enableJump = true;
            inputReader.enableCameraInput = true;
            inputReader.enableCameraSwitch = true;
            inputReader.enableYoyo = true;
            pauseManager.canBePaused = true;

            targetAngle = healthManager.respawnRotation.eulerAngles.y;
            lastRotationAngle = healthManager.respawnRotation.eulerAngles.y;
        }

        inputProvider.enabled = true;

        soulAnim.SetBool("isRespawning", false);
        isRespawning = false;
    }

    IEnumerator WaitToResetCamera()
    {
        yield return new WaitForFixedUpdate();
        cameraRecenter.recenter = true;
    }

    IEnumerator DeathCoroutine()
    {
        if (cameraShake) impulseSource.GenerateImpulse(3);

        pauseManager.CheckPauseMenu();

        if (!soulAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_HurtDeath")) soulAnim.CrossFade("Player_HurtDeath", 0);
        soulAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        deathEffect.Play();

        kiteAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        kiteAnim.SetBool("opened", false);
        kiteAnim.CrossFade("KiteHide", 0);

        for (int i = 0; i < glidingTrails.Length; i++)
        {
            glidingTrails[i].enabled = false;
        }
        jumpingTrail.enabled = false;

        zoomHandlerTalk.Zoom(false, false);
        move_Throws.isTouchingCloud = false;

        eventTextAction.EndText();

        while (currentAnimation.normalizedTime < 1)
        {
            yield return null;
        }
        rigidbody.velocity = Vector3.zero;
        soulAnim.SetBool("isCloseToDamage", false);
        soulAnim.SetBool("isCloseToBoss", false);

        eventTextAction.isActive = false;
        eventTextAction.nextText = false;
        eventTextAction.EndText();
        canTalk = false;
        canInteract = false;
        NPCDetector.Instance.animator.SetBool("canTalk", false);
        ButtonDetector.Instance.animator.SetBool("canInteract", false);

        if (YoyoHandler.Instance) YoyoHandler.Instance.CaughtByPlayer();
    }

    IEnumerator WaitForLeavingGroundCoroutine()
    {
        finishedJumpCoroutine = false;
        while (isGrounded | move_Throws.isGroundedForSlide)
        {
            yield return new WaitForFixedUpdate();
        }
        finishedJumpCoroutine = true;
    }

    #endregion


    #region Voids

    public void DeathEffect()
    {
        StartCoroutine(DeathCoroutine());
    }

    public void GotPhoto(Vector3 photoPosition)
    {
        for (int i = 0; i < glidingTrails.Length; i++)
        {
            glidingTrails[i].enabled = false;
        }
        jumpingTrail.enabled = false;

        kiteAnim.SetBool("opened", false);
        kiteAnim.CrossFade("KiteHide", 0);
        kiteAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        canMove = false;

        characterModel.transform.rotation = Quaternion.Euler(characterModel.transform.rotation.eulerAngles.x, characterCamera.transform.rotation.eulerAngles.y - 180, characterModel.transform.rotation.eulerAngles.z);
        transform.position = photoPosition;

        soulAnim.CrossFade("Player_Transform", 0.0f);
        soulAnim.CrossFade("Player_EmptyAnim", 0, 8);

        soulAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        soulAnim.SetBool("isTransforming", true);

        soulAnim.SetLayerWeight(4, 0);
        soulAnim.SetLayerWeight(5, 0);

        //soulTailAnim.DeltaType = TailAnimator2.EFDeltaType.UnscaledDeltaTime;

        alphaChanger.MakePlayerMaterialOpaque();

        Time.timeScale = 0;
        cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
        cameraTarget.transform.position = characterModel.transform.position;

        if (!isDiving)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.velocity += Vector3.up * verticalAirDashMultiplier;
        }
    }

    public void HidePhoto()
    {
        soulAnim.updateMode = AnimatorUpdateMode.Normal;

        soulAnim.SetLayerWeight(4, 1);
        soulAnim.SetLayerWeight(5, 1);

        pauseManager.canBePaused = true;
        pauseManager.EnableInputState();

        //soulTailAnim.DeltaType = TailAnimator2.EFDeltaType.SafeDelta;
        canMove = true;

        airbornedTimer = 10;

        if (isJumping)
        {
            jumpingTrail.enabled = true;
        }

        Time.timeScale = 1;

        cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        kiteAnim.updateMode = AnimatorUpdateMode.Normal;

        soulAnim.SetBool("isTransforming", false);
        soulAnim.SetBool("isJumping", false);
        afterJumpTimer = 0;

        isGrounded = false;
        isJumping = false;
        isAirJumping = false;
        isLongJumping = false;
        afterLandingTimer = 0;

        if (isDiving)
        {
            soulAnim.CrossFade("Player_Dive", 0, 0);
        }
        else
        {
            soulAnim.CrossFade("Player_Fall", 0, 0);
        }

        soulAnim.CrossFade("Player_Fall", 0, 3);
    }

    public void QuickCameraLerpToPlayer(float speed)
    {
        cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, characterModel.transform.position, Time.deltaTime * normalHorizontalLerpSpeed * speed);
    }

    private void LerpCamera()
    {
        if (isDiving)
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * diveVerticalLerpSpeed);
        }
        else if ((!isJumping && !isGliding) | isDashing | isBouncing | ledgeJump | afterWallJumpTimer > 0 | (!isGrounded && !isJumping))
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * airVerticalLerpSpeed);
        }
        else if (isWallSliding)
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * slowVerticalLerpSpeed);
        }
        else if (isAirJumping && ((airbornedTimer > 18 && !isLongJumping) | airbornedTimer > 27))
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * airVerticalLerpSpeed);
        }
        else if (isGliding && (((afterJumpTimer == 0 | afterJumpTimer > 70) | airbornedTimer > 30) | !isGroundBelow | isAirJumping))
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * slowVerticalLerpSpeed);
        }
        else if (landed)
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * groundVerticalLerpSpeed * 2);
        }
        else if (afterSlopeJumpTimer > 0 | afterUnclimbableSlopeJumpTimer > 0)
        {
            if (slopeDot <= 0)
            {
                cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * groundVerticalLerpSpeed);
            }
            else
            {
                cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y - 2, cameraTarget.transform.position.z)), Time.deltaTime * groundVerticalLerpSpeed);
            }
        }
        else if ((afterJumpTimer == 0 | afterJumpTimer > 30 | (airbornedTimer > 30 && (afterJumpTimer == 0 | afterJumpTimer > 1)) && !isGliding))
        {
            if (isGroundBelow | isGrounded)
            {
                cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * airVerticalLerpSpeed);
            }
            else
            {
                cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime * downwardSlopeVerticalLerpSpeed);
            }
        }
        else
        {
            cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(cameraTarget.transform.position.x, characterModel.transform.position.y, cameraTarget.transform.position.z)), Time.deltaTime);
        }
        cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, (new Vector3(characterModel.transform.position.x, cameraTarget.transform.position.y, characterModel.transform.position.z)), Time.deltaTime * normalHorizontalLerpSpeed);
    }

    public void ResetState()
    {
        isGrounded = false;
        isJumping = false;
        isAirJumping = false;
        isLongJumping = false;
        isWallSliding = false;
        TPSWalkForm = 0;
        isDashing = false;
        landed = false;
        isThrowing = false;
        altThrowing = false;
        isDiving = false;
        additionalGravity = 0;
        isGliding = false;
        isTouchingSlope = false;

        transform.rotation = new Quaternion(0, 0, 0, 0);
        soulAnim.updateMode = AnimatorUpdateMode.Normal;
    }

    public void EnableMovement()
    {
        canMove = true;
        inputReader.enableDash = true;
        inputReader.enableJump = true;
        inputReader.enableYoyo = true;
        inputReader.enablePause = true;
    }

    public void Transformation()
    {
        if (!isGrounded)
        {
            if (isGliding)
            {
                soulAnim.CrossFade("Player_GlideFall", 0.0f, 0);
            }
            else if (isDiving)
            {
                soulAnim.CrossFade("Player_Dive", 0.0f, 0);
            }
            else
            {
                soulAnim.CrossFade("Player_Fall", 0.0f, 0);
            }
        }
    }

    private void AnimUpdate()
    {
        soulAnim.SetBool("isGrounded", isGrounded);
        clothAnim.SetBool("isGrounded", isGrounded);

        soulAnim.SetBool("isJumping", isJumping);

        soulAnim.SetBool("isWallSliding", isWallSliding);

        soulAnim.SetBool("isTouchingWall", isTouchingWall);

        soulAnim.SetBool("isCloseToWall", isCloseToWall);

        soulAnim.SetBool("isCloseToWallForAir", isCloseToWallForAir);

        soulAnim.SetBool("isCloseToCeilingForStretch", isCloseToCeilingForStretch);

        soulAnim.SetFloat("stickIncline", stickIncline);
        clothAnim.SetFloat("stickIncline", stickIncline);

        soulAnim.SetFloat("stickInclineRealValue", stickInclineRealValue);

        if (runBoostTimer >= runBoostTimerLimit)
        {
            soulAnim.SetBool("boostRun", true);
            clothAnim.SetBool("boostRun", true);
        }
        else
        {
            soulAnim.SetBool("boostRun", false);
            clothAnim.SetBool("boostRun", false);
        }

        soulAnim.SetBool("isHurt", healthManager.isHurt);

        soulAnim.SetBool("isDashing", isDashing);
        clothAnim.SetBool("isDashing", isDashing);

        soulAnim.SetInteger("dashRecoilTimer", dashRecoilTimer);

        soulAnim.SetBool("landed", landed);

        soulAnim.SetBool("isThrowing", isThrowing);

        soulAnim.SetBool("altThrowing", altThrowing);

        soulAnim.SetBool("isDiving", isDiving);

        soulAnim.SetBool("isGliding", isGliding);
        clothAnim.SetBool("isGliding", isGliding);

        soulAnim.SetBool("isTouchingSlope", isTouchingSlope);

        soulAnim.SetBool("isSlidingDown", isSlidingDown);

        soulAnim.SetBool("isRunningOnSlope", false);

        soulAnim.SetBool("isBouncing", isBouncing);

        soulAnim.SetBool("cameraTPSisActive", true);

        soulAnim.SetBool("isMouseAndKeyboard", inputReader.isMouseAndKeyboard);

        soulAnim.SetInteger("idleTimer", idleTimer);

        soulAnim.SetInteger("idleValue", idleValue);

        kiteAnim.SetBool("opened", isGliding);

    }

    #endregion


    #region Hit effects

    public void HitEffect(Transform hitPosition)
    {
        Instantiate<ParticleSystem>(hitEffect1, hitPosition.position, characterModel.transform.rotation);
        Instantiate<ParticleSystem>(hitEffect2, hitPosition.position, characterModel.transform.rotation);
        Instantiate<ParticleSystem>(hitEffect3, hitPosition.position, characterModel.transform.rotation);
        Instantiate<ParticleSystem>(hitEffect4, hitPosition.position, characterModel.transform.rotation);
        Instantiate<ParticleSystem>(hitEffect5, hitPosition.position, characterModel.transform.rotation);
        Instantiate<ParticleSystem>(hemisphereHitEffect1, hitPosition.position, characterModel.transform.rotation);
        Instantiate<ParticleSystem>(hemisphereHitEffect2, hitPosition.position, characterModel.transform.rotation);

        if (cameraShake) impulseSource.GenerateImpulse(1);
    }

    public void JumpHitEffect(Transform hitPosition)
    {
        Instantiate<ParticleSystem>(jumpHitEffect1, hitPosition.position + jumpOnEnemyVisualPosition, cameraTarget.transform.rotation);
        Instantiate<ParticleSystem>(jumpHitEffect2, hitPosition.position + jumpOnEnemyVisualPosition, cameraTarget.transform.rotation);
        Instantiate<ParticleSystem>(jumpHitEffect3, hitPosition.position + jumpOnEnemyVisualPosition, cameraTarget.transform.rotation);

        Instantiate<ParticleSystem>(jumpEffect3D, characterModel.transform.position, characterModel.transform.rotation);
    }

    public void HurtEffect()
    {
        hurtEffect1.Play();
        hurtEffect2.Play();
        hurtEffect3.Play();

        if (cameraShake) impulseSource.GenerateImpulse(1);

        IEnumerator isHurtEffectCoroutine()
        {
            isHurtEffect.Play();
            yield return new WaitForSeconds(1);
            isHurtEffect.Stop();
        }
    }

    #endregion


    #region Gravity

    private void ApplyGravity()
    {
        Vector3 gravity = Vector3.zero;
        //Vector3 force = this.currentLockOnSlope || this.isTouchingStep ? this.down * this.gravityMultiplier * -Physics.gravity.y * this.coyoteJumpMultiplier : this.globalDown * this.gravityMultiplier * -Physics.gravity.y * this.coyoteJumpMultiplier;
        if (currentLockOnSlope || isTouchingStep) gravity = down * gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier;
        else gravity = globalDown * gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier;

        //avoid little jump
        if (groundNormal.y != 1 && groundNormal.y != 0 && isTouchingSlope && prevGroundNormal != groundNormal)
        {
            //Debug.Log("Added correction jump on slope");
            gravity *= gravityMultiplayerOnSlideChange;
        }

        //slide if angle too big
        if (groundNormal.y != 1 && groundNormal.y != 0 && (currentSurfaceAngle > maxClimbableSlopeAngle && !isTouchingStep))
        {
            //Debug.Log("Slope angle too high, character is sliding");
            if (currentSurfaceAngle > 0f && currentSurfaceAngle <= 30f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope * -Physics.gravity.y;
            else if (currentSurfaceAngle > 30f && currentSurfaceAngle <= 89f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
        }

        //friction when touching wall
        if (!isGrounded && isTouchingWall && (afterWallJumpTimer == 0 | afterWallJumpTimer > 10) &&
            (afterJumpTimer == 0 | afterJumpTimer > 15) && !isDashing && !move_Throws.isHoldingToRope)
        {
            isWallSliding = true;

            if (isWallSlidingTimer < 10) isWallSlidingTimer++;

            afterWallSlideTimer++;
            isGliding = false;

            additionalGravity = 0;
            gravity *= frictionAgainstWall;

            if (currentAnimation.IsName("Player_WallSlideAlternate"))
            {
                Instantiate<ParticleSystem>(alternateWallSlideEffect3D, characterModel.transform.position, characterModel.transform.rotation);
            }
            else
            {
                Instantiate<ParticleSystem>(wallSlideEffect3D, characterModel.transform.position, characterModel.transform.rotation);
            }
        }
        else
        {
            isWallSliding = false;
            isWallSlidingTimer = 0;
        }

        if (!isGrounded && !isWallSliding && !isDashing && !isGliding && !move_Throws.isHoldingToRope)
        {
            if (additionalGravity > -180)
            {
                additionalGravity -= 3;
            }
            rigidbody.AddForce(Vector3.up * additionalGravity);
        }
        else if (isGliding)
        {
            additionalGravity = -15;
            rigidbody.AddForce(Vector3.up * additionalGravity);
        }
        else additionalGravity = 0;

        if (frontIsGround | backIsGround | leftIsGround | rightIsGround)
        {
            additionalGravity = 0;
        }

        if (isJumping && !isWallSliding && !canLedgeJump) rigidbody.AddForce(Vector3.up * -50);

        if ((!(isDashing && stickIncline < 0.1 || isGliding) && ((afterWallSlideTimer == 0 || afterWallSlideTimer > 12 | 
            tryingToClimbTimer > tryingToClimbTimerLimit) || !isWallSliding) && !(isWallSliding && (afterJumpTimer > 10 && afterJumpTimer < 24))))
        {
            rigidbody.AddForce(gravity);
        }

        if (isGrounded && afterLandingTimer == 0)
        {
            rigidbody.AddForce(gravity * 1.2f);
        }

        if (tryingToClimbTimer >= tryingToClimbTimerLimit)
        {
            additionalGravity -= 3;
            if (isWallSliding) rigidbody.AddForce(Vector3.up * additionalGravity * 60);
            else rigidbody.AddForce(Vector3.up * additionalGravity * 2.5f);
            gravity *= 1;
        }
    }

    #endregion


    #region Knockback

    public void Knockback(Vector3 direction, float knockBackForceHorizontal, float knockBackForceVertical)
    {
        rigidbody.AddForce(direction * knockBackForceHorizontal);
        rigidbody.AddForce(Vector3.up * knockBackForceVertical);
    }

    #endregion


    #region Bounce

    public void Bounce(float bounceStrength)
    {
        if (isGliding) rigidbody.velocity = Vector3.up * bounceStrength / 2 + Vector3.forward * rigidbody.velocity.z;
        else rigidbody.velocity = Vector3.up * bounceStrength + Vector3.forward * rigidbody.velocity.z;

        afterJumpTimer = 5;
        isJumping = true;
        isBouncing = true;

        if ((afterDashTimer == 0 | afterDashTimer > 6) && !isGliding)
        {
            float transitionTime = 0.25f;
            if (isDiving) transitionTime = 0;
            soulAnim.CrossFade("Player_TrampolineJump", transitionTime);
        }

        airbornedTimer = 10;
        additionalGravity = 0;

        stretchAnimationScript.DoStretch("StretchBounceAnimation");
        soulAnim.CrossFade("Player_Scale_StretchBounce", 0, 9);
    }

    #endregion


    #region Friction and Round

    private void SetFriction(float _frictionWall, bool _isMinimum)
    {
        collider.material.dynamicFriction = 0.6f * _frictionWall;
        collider.material.staticFriction = 0.6f * _frictionWall;

        if (_isMinimum) collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        else collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
    }


    private float RoundValue(float _value)
    {
        float unit = (float)Mathf.Round(_value);

        if (_value - unit < 0.000001f && _value - unit > -0.000001f) return unit;
        else return _value;
    }

    #endregion


    #region Gizmos

    private void OnDrawGizmos()
    {
        if (debug)
        {
            rigidbody = this.GetComponent<Rigidbody>();
            collider = this.GetComponent<CapsuleCollider>();

            Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);
            Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + heightWallCheckerTop, transform.position.z);

            //ground and slope
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThrashold);

            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), slopeCheckerThrashold);

            //direction
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + forward * 2f);

            Gizmos.color = UnityEngine.Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + globalForward * 2);

            Gizmos.color = UnityEngine.Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + reactionForward * 2f);

            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawLine(transform.position, transform.position + down * 2f);

            Gizmos.color = UnityEngine.Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + globalDown * 2f);

            Gizmos.color = UnityEngine.Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + reactionGlobalDown * 2f);

            //step check
            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(bottomStepPos, bottomStepPos + globalForward * stepCheckerThrashold);

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + new Vector3(0f, maxStepHeight, 0f) + globalForward * (stepCheckerThrashold + 0.05f));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * stepCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(45, Vector3.up) * (globalForward * stepCheckerThrashold) + new Vector3(0f, maxStepHeight, 0f));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(-45, transform.up) * (globalForward * stepCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(-45, Vector3.up) * (globalForward * stepCheckerThrashold) + new Vector3(0f, maxStepHeight, 0f));

            //wall check
            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + globalForward * wallCheckerThrashold);

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * wallCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(90, transform.up) * (globalForward * wallCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(135, transform.up) * (globalForward * wallCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(180, transform.up) * (globalForward * wallCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(225, transform.up) * (globalForward * wallCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(270, transform.up) * (globalForward * wallCheckerThrashold));

            Gizmos.color = UnityEngine.Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(315, transform.up) * (globalForward * wallCheckerThrashold));
        }
    }

    #endregion
}
