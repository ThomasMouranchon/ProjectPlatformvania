using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

public class InputReader : MonoBehaviour
{
    private static InputReader instance = null;
    public static InputReader Instance => instance;

    [Header("Input specs")]
    public UnityEvent changedInputToMouseAndKeyboard;
    public UnityEvent changedInputToGamepad;

    [Header("Enable inputs")]
    public bool enableAxisInput = true;
    [HideInInspector] public bool enableCameraInput = true;
    [HideInInspector] public bool enableJump = true;
    [HideInInspector] public bool enableInteract = true;
    [HideInInspector] public bool enableGlide = true;
    [HideInInspector] public bool enableBomb = true;
    [HideInInspector] public bool enableDash = true;
    [HideInInspector] public bool enableYoyo = true;
    public bool enableCameraSwitch = true;
    [HideInInspector] public bool enablePause = true;
    [HideInInspector] public bool enableValidate = true;
    [HideInInspector] public bool enableBack = true;
    [HideInInspector] public bool enableClick = true;


    [HideInInspector]
    public Vector2 axisInput;
    [HideInInspector]
    public Vector2 cameraInput;
    [HideInInspector]
    public bool jump;
    [HideInInspector]
    public bool jumpHold;
    [HideInInspector]
    public bool interact;
    [HideInInspector]
    public bool interactHold;
    [HideInInspector]
    public bool glide;
    [HideInInspector]
    public bool glideHold;
    [HideInInspector]
    public float zoom;
    [HideInInspector]
    public bool dash;
    [HideInInspector]
    public bool dashHold;
    [HideInInspector]
    public bool bomb;
    [HideInInspector]
    public bool bombHold;
    [HideInInspector]
    public bool yoyo;
    [HideInInspector]
    public bool yoyoHold;
    [HideInInspector]
    public bool cameraSwitch;
    [HideInInspector]
    public bool cameraSwitchHold;
    [HideInInspector]
    public bool pause;
    [HideInInspector]
    public bool validate;
    [HideInInspector]
    public bool validateHold;
    [HideInInspector]
    public bool back;
    [HideInInspector]
    public bool backHold;
    [HideInInspector]
    public bool click;
    [HideInInspector]
    public bool clickHold;


    private bool hasJumped;
    private bool hasInteracted;
    private bool hasGlide;
    private bool hasUsedBomb;
    private bool hasDashed;
    private bool hasUsedYoyo;
    private bool hasCameraSwitched;
    private bool hasPaused;
    private bool hasValidated;
    private bool hasBacked;
    private bool hasClicked;
    private bool skippedFrame;
    public bool isMouseAndKeyboard = true;
    private bool oldInput;

    public int jumpTimerLimit;
    public int currentJumpTimer;

    //DISABLE if using old input system
    [HideInInspector] public MovementActions movementActions;

    private PlayerInput playerInput;

    private InputAction axisInputAction;
    private InputAction cameraInputAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction glideAction;
    private InputAction bombAction;
    private InputAction dashAction;
    private InputAction yoyoAction;
    private InputAction cameraSwitchAction;
    private InputAction pauseAction;
    private InputAction validateAction;
    private InputAction backAction;


    private Action<InputAction.CallbackContext> movePerformed;
    private Action<InputAction.CallbackContext> moveCanceled;
    private Action<InputAction.CallbackContext> reservePerformed;
    private Action<InputAction.CallbackContext> reserveCanceled;
    /**/

    private ChangeInputIcon[] inputsIcons;
    private InputDevice lastDevice;


    //DISABLE if using old input system
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
        playerInput = GetComponent<PlayerInput>();

        ResetMovementActions();

        SetupInputActions();
    }

    public void ReloadMovementActions()
    {
        movementActions.Gameplay.Movement.performed += ctx => OnMove(ctx);
        movementActions.Gameplay.Movement.canceled += ctx => MoveEnded(ctx);

        movementActions.Gameplay.Camera.performed += ctx => OnCamera(ctx);
        movementActions.Gameplay.Camera.canceled += ctx => CameraEnded(ctx);

        movementActions.Gameplay.Jump.performed += ctx => OnJump(ctx);
        movementActions.Gameplay.Jump.canceled += ctx => JumpEnded(ctx);

        movementActions.Gameplay.Interact.performed += ctx => OnInteract(ctx);
        movementActions.Gameplay.Interact.canceled += ctx => InteractEnded(ctx);

        movementActions.Gameplay.Glide.performed += ctx => OnGlide(ctx);
        movementActions.Gameplay.Glide.canceled += ctx => GlideEnded(ctx);

        movementActions.Gameplay.Bomb.performed += ctx => OnBomb(ctx);
        movementActions.Gameplay.Bomb.canceled += ctx => BombEnded(ctx);

        movementActions.Gameplay.Dash.performed += ctx => OnDash(ctx);
        movementActions.Gameplay.Dash.canceled += ctx => DashEnded(ctx);

        movementActions.Gameplay.Yoyo.performed += ctx => OnYoyo(ctx);
        movementActions.Gameplay.Yoyo.canceled += ctx => YoyoEnded(ctx);

        movementActions.Gameplay.CameraSwitch.performed += ctx => OnCameraSwitch(ctx);
        movementActions.Gameplay.CameraSwitch.canceled += ctx => CameraSwitchEnded(ctx);

        movementActions.Gameplay.Pause.performed += ctx => OnPause(ctx);
        movementActions.Gameplay.Pause.canceled += ctx => PauseEnded(ctx);

        movementActions.Menu.Click.performed += ctx => OnClick(ctx);
        movementActions.Menu.Click.canceled += ctx => ClickEnded(ctx);

        movementActions.Menu.Validate.performed += ctx => OnValidate(ctx);
        movementActions.Menu.Validate.canceled += ctx => ValidateEnded(ctx);

        movementActions.Menu.Back.performed += ctx => OnBack(ctx);
        movementActions.Menu.Back.canceled += ctx => BackEnded(ctx);
    }

    public void ResetMovementActions()
    {
        movementActions = new MovementActions();

        movementActions.Disable();
        ClearMovementActions();

        ReloadMovementActions();

        movementActions.Enable();
    }

    public void ClearAndReloadMovementActions()
    {
        movementActions.Disable();
        ClearMovementActions();

        ReloadMovementActions();

        movementActions.Enable();
    }

    private void ClearMovementActions()
    {
        if (movementActions != null)
        {
            movementActions.Gameplay.Movement.performed -= OnMove;
            movementActions.Gameplay.Movement.canceled -= MoveEnded;

            movementActions.Gameplay.Camera.performed -= OnCamera;
            movementActions.Gameplay.Camera.canceled -= CameraEnded;

            movementActions.Gameplay.Jump.performed -= OnJump;
            movementActions.Gameplay.Jump.canceled -= JumpEnded;

            movementActions.Gameplay.Interact.performed -= OnInteract;
            movementActions.Gameplay.Interact.canceled -= InteractEnded;

            movementActions.Gameplay.Glide.performed -= OnGlide;
            movementActions.Gameplay.Glide.canceled -= GlideEnded;

            movementActions.Gameplay.Bomb.performed -= OnBomb;
            movementActions.Gameplay.Bomb.canceled -= BombEnded;

            movementActions.Gameplay.Dash.performed -= OnDash;
            movementActions.Gameplay.Dash.canceled -= DashEnded;

            movementActions.Gameplay.Yoyo.performed -= OnYoyo;
            movementActions.Gameplay.Yoyo.canceled -= YoyoEnded;

            movementActions.Gameplay.CameraSwitch.performed -= OnCameraSwitch;
            movementActions.Gameplay.CameraSwitch.canceled -= CameraSwitchEnded;

            movementActions.Gameplay.Pause.performed -= OnPause;
            movementActions.Gameplay.Pause.canceled -= PauseEnded;

            movementActions.Menu.Click.performed -= OnClick;
            movementActions.Menu.Click.canceled -= ClickEnded;

            movementActions.Menu.Validate.performed -= OnValidate;
            movementActions.Menu.Validate.canceled -= ValidateEnded;

            movementActions.Menu.Back.performed -= OnBack;
            movementActions.Menu.Back.canceled -= BackEnded;
        }
    }



    private void Update()
    {
        UpdateMoveInput();
        UpdateCameraInput();

        UpdateJumpInput();
        UpdateInteractInput();
        UpdateGlideInput();
        UpdateDashInput();
        UpdateYoyoInput();
        UpdateBombInput();
        UpdateCameraSwitchInput();
        UpdatePauseInput();
        UpdateValidateInput();
        UpdateBackInput();
        //UpdateClickInput();

        if (Application.targetFrameRate < 60 && Application.targetFrameRate > 0)
        {
            UpdateEffect();
        }
    }

    private void FixedUpdate()
    {
        if (Application.targetFrameRate <= 0 | Application.targetFrameRate >= 60)
        {
            UpdateEffect();
        }
    }

    private void UpdateEffect()
    {
        if (hasJumped && skippedFrame)
        {
            jump = false;
            hasJumped = false;
        }
        if (currentJumpTimer > 0)
        {
            currentJumpTimer--;
        }

        if (hasInteracted && skippedFrame)
        {
            interact = false;
            hasInteracted = false;
        }
        if (hasGlide && skippedFrame)
        {
            glide = false;
            hasGlide = false;
        }
        if (hasUsedBomb && skippedFrame)
        {
            bomb = false;
            hasUsedBomb = false;
        }
        if (hasDashed && skippedFrame)
        {
            dash = false;
            hasDashed = false;
        }
        if (hasUsedYoyo && skippedFrame)
        {
            yoyo = false;
            hasUsedYoyo = false;
        }
        if (hasCameraSwitched && skippedFrame)
        {
            cameraSwitch = false;
            hasCameraSwitched = false;
        }
        if (hasPaused && skippedFrame)
        {
            pause = false;
            hasPaused = false;
        }
        if (hasValidated && skippedFrame)
        {
            validate = false;
            hasValidated = false;
        }
        if (hasBacked && skippedFrame)
        {
            back = false;
            hasBacked = false;
        }
        if (!skippedFrame) skippedFrame = true;
    }


    //DISABLE if using old input system
    private void GetDeviceNew(InputAction.CallbackContext ctx)
    {
        oldInput = isMouseAndKeyboard;

        if (ctx.control.device is Keyboard || ctx.control.device is Mouse) isMouseAndKeyboard = true;
        else isMouseAndKeyboard = false;

        if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
        else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();
    }

    public void ChangeInputState(bool state)
    {
        enableJump = state;
        enableBomb = state;
        enableDash = state;
        enableYoyo = state;
        enableCameraSwitch = state;
        enablePause = state;
    }


    #region Actions
    private void SetupInputActions()
    {
        axisInputAction = playerInput.actions["Movement"];
        cameraInputAction = playerInput.actions["Camera"];
        jumpAction = playerInput.actions["Jump"];
        interactAction = playerInput.actions["Interact"];
        glideAction = playerInput.actions["Glide"];
        bombAction = playerInput.actions["Bomb"];
        dashAction = playerInput.actions["Dash"];
        yoyoAction = playerInput.actions["Yoyo"];
        cameraSwitchAction = playerInput.actions["CameraSwitch"];
        pauseAction = playerInput.actions["Pause"];
        validateAction = playerInput.actions["Validate"];
        backAction = playerInput.actions["Back"];
    }

    #region Move
    public void UpdateMoveInput()
    {
        if (enableAxisInput)
        {
            axisInput = axisInputAction.ReadValue<Vector2>();
        }
        else
        {
            axisInput = new Vector2(0, 0);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (enableAxisInput)
        {
            axisInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }
        else
        {
            axisInput = new Vector2(0, 0);
        }
    }

    public void MoveEnded(InputAction.CallbackContext ctx)
    {
        axisInput = new Vector2(0, 0);
    }
    #endregion



    #region Camera
    public void UpdateCameraInput()
    {
        if (!enableCameraInput)
        {
            cameraInput = new Vector2(0, 0);
        }
    }

    public void OnCamera(InputAction.CallbackContext ctx)
    {
        if (enableCameraInput)
        {
            cameraInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }
    }

    public void CameraEnded(InputAction.CallbackContext ctx)
    {
        cameraInput = new Vector2(0, 0);
    }
    #endregion



    #region Jump
    public void UpdateJumpInput()
    {
        if (!enableJump)
        {
            jump = false;
            jumpHold = false;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (enableJump)
        {
            jump = true;
            jumpHold = true;

            hasJumped = true;
            skippedFrame = false;
            if (Time.timeScale > 0)
            {
                currentJumpTimer = jumpTimerLimit;
            }
        }
    }

    public void JumpEnded(InputAction.CallbackContext ctx)
    {
        jump = false;
        jumpHold = false;
    }
    #endregion



    #region Interact
    public void UpdateInteractInput()
    {
        if (!enableInteract)
        {
            interact = false;
            interactHold = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (enableInteract)
        {
            interact = true;
            interactHold = true;

            hasInteracted = true;
            skippedFrame = false;
        }
    }

    public void InteractEnded(InputAction.CallbackContext ctx)
    {
        interact = false;
        interactHold = false;
    }
    #endregion



    #region Glide
    public void UpdateGlideInput()
    {
        if (!enableGlide)
        {
            glide = false;
            glideHold = false;
        }
    }

    public void OnGlide(InputAction.CallbackContext ctx)
    {
        if (enableGlide)
        {
            glide = true;
            glideHold = true;

            hasGlide = true;
            skippedFrame = false;
        }
    }

    public void GlideEnded(InputAction.CallbackContext ctx)
    {
        glide = false;
        glideHold = false;
    }
    #endregion



    #region Bomb
    public void UpdateBombInput()
    {
        if (!enableBomb)
        {
            bomb = false;
            bombHold = false;
        }
    }

    public void OnBomb(InputAction.CallbackContext ctx)
    {
        if (enableBomb)
        {
            bomb = true;
            bombHold = true;

            hasUsedBomb = true;
            skippedFrame = false;
        }
    }

    public void BombEnded(InputAction.CallbackContext ctx)
    {
        bomb = false;
        bombHold = false;
    }
    #endregion



    #region Dash
    public void UpdateDashInput()
    {
        if (!enableDash)
        {
            dash = false;
            dashHold = false;
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (enableDash)
        {
            dash = true;
            dashHold = true;

            hasDashed = true;
            skippedFrame = false;
        }
    }

    public void DashEnded(InputAction.CallbackContext ctx)
    {
        dash = false;
        dashHold = false;
    }
    #endregion



    #region Bomb
    public void UpdateYoyoInput()
    {
        if (!enableYoyo)
        {
            yoyo = false;
            yoyoHold = false;
        }
    }

    public void OnYoyo(InputAction.CallbackContext ctx)
    {
        if (enableYoyo)
        {
            yoyo = true;
            yoyoHold = true;

            hasUsedYoyo = true;
            skippedFrame = false;
        }
    }

    public void YoyoEnded(InputAction.CallbackContext ctx)
    {
        yoyo = false;
        yoyoHold = false;
    }
    #endregion



    #region CameraSwitch
    public void UpdateCameraSwitchInput()
    {
        if (!enableCameraSwitch)
        {
            cameraSwitch = false;
            cameraSwitchHold = false;
        }
    }

    public void OnCameraSwitch(InputAction.CallbackContext ctx)
    {
        if (enableCameraSwitch)
        {
            cameraSwitch = true;
            cameraSwitchHold = true;

            hasCameraSwitched = true;
            skippedFrame = false;
        }
    }

    public void CameraSwitchEnded(InputAction.CallbackContext ctx)
    {
        cameraSwitch = false;
        cameraSwitchHold = false;
    }
    #endregion



    #region Pause
    public void UpdatePauseInput()
    {
        if (!enablePause)
        {
            pause = false;
        }
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (enablePause)
        {
            pause = true;

            hasPaused = true;
            skippedFrame = false;
        }
    }

    public void PauseEnded(InputAction.CallbackContext ctx)
    {
        pause = false;
    }
    #endregion



    #region Validate
    public void UpdateValidateInput()
    {
        if (enableValidate && movementActions.Menu.Validate.WasPressedThisFrame())
        {
            validate = true;
            validateHold = true;

            hasValidated = true;
            skippedFrame = false;
        }
        else if (!enableValidate | validateAction.WasReleasedThisFrame())
        {
            validate = false;
            validateHold = false;
        }
    }

    public void OnValidate(InputAction.CallbackContext ctx)
    {
        if (enableValidate)
        {
            validate = true;
            validateHold = true;

            hasValidated = true;
            skippedFrame = false;
        }
    }

    public void ValidateEnded(InputAction.CallbackContext ctx)
    {
        validate = false;
        validateHold = false;
    }
    #endregion



    #region Back
    public void UpdateBackInput()
    {
        if (!enableBack)
        {
            back = false;
            backHold = false;
        }
    }

    public void OnBack(InputAction.CallbackContext ctx)
    {
        if (enableBack)
        {
            back = true;
            backHold = true;

            hasBacked = true;
            skippedFrame = false;
        }
    }

    public void BackEnded(InputAction.CallbackContext ctx)
    {
        back = false;
        backHold = false;
    }
    #endregion



    #region Click
    public void UpdateClickInput()
    {
        if (!enableClick)
        {
            click = false;
            clickHold = false;
        }
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (enableClick)
        {
            click = true;
            clickHold = true;

            hasClicked = true;
            skippedFrame = false;
        }
        else
        {
            click = false;
            clickHold = false;
        }
    }

    public void ClickEnded(InputAction.CallbackContext ctx)
    {
        click = false;
        clickHold = false;
    }
    #endregion


    #endregion


    #region Enable / Disable

    //DISABLE if using old input system
    private void OnEnable()
    {
        movementActions.Enable();
        inputsIcons = FindObjectsOfType<ChangeInputIcon>(true);
        InputSystem.onDeviceChange += OnDeviceChange;
        InputSystem.onEvent += ChangeAllInputIcons;
    }


    //DISABLE if using old input system
    private void OnDisable()
    {
        movementActions.Disable();
        InputSystem.onDeviceChange -= OnDeviceChange;
        InputSystem.onEvent -= ChangeAllInputIcons;
    }

    #endregion
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
        {
            InputUser user;

            if (InputUser.all.Count == 0)
            {
                user = InputUser.CreateUserWithoutPairedDevices();
            }
            else
            {
                user = InputUser.all[0];
            }

            InputUser.PerformPairingWithDevice(device, user);

            if (device is Gamepad gamepad)
            {
                SwitchControlScheme(device);
            }
            else
            {
                InputUser.PerformPairingWithDevice(device, InputUser.all[0]);
                movementActions.bindingMask = InputBinding.MaskByGroup("MouseKeyboard");
            }
        }
        else if (change == InputDeviceChange.Removed)
        {
            foreach (var user in InputUser.all)
            {
                if (user.pairedDevices.Any(d => d.deviceId == device.deviceId))
                {
                    user.UnpairDevice(device);
                }
            }
        }
        ClearAndReloadMovementActions();
    }

    private void SwitchControlScheme(InputDevice device)
    {
        if (InputUser.all.Count == 0)
        {
            Debug.LogWarning("No InputUser found. Creating a new one.");
            InputUser.CreateUserWithoutPairedDevices();
        }

        InputUser user = InputUser.all[0];

        if (!user.pairedDevices.Contains(device))
        {
            Debug.Log($"Pairing {device.name} with user.");
            InputUser.PerformPairingWithDevice(device, user);
        }

        // S'assurer que la manette est activée
        if (!device.enabled)
        {
            Debug.Log($"Re-enabling {device.name}");
            InputSystem.EnableDevice(device);
        }

        if (device is DualShockGamepad || device is DualSenseGamepadHID)
        {
            Debug.Log("Switching to DualShock control scheme.");
            movementActions.bindingMask = InputBinding.MaskByGroup("PsController");
        }
        else if (device is SwitchProControllerHID)
        {
            Debug.Log("Switching to Switch control scheme.");
            movementActions.bindingMask = InputBinding.MaskByGroup("SwitchController");
        }
        else
        {
            Debug.Log("Switching to Xbox control scheme.");
            movementActions.bindingMask = InputBinding.MaskByGroup("XboxController");
        }
        SetupInputActions();
    }


    public void ChangeAllInputIcons(InputEventPtr eventPtr, InputDevice device)
    {
        if (device == lastDevice) return;

        SetupInputActions();
        lastDevice = device;

        if (inputsIcons.Length == 0)
        {
            Debug.LogError("Aucune input icône n'a été assignée !");
            return;
        }

        isMouseAndKeyboard = (device is Mouse || device is Keyboard);

        foreach (ChangeInputIcon inputIcon in inputsIcons)
        {
            inputIcon.UpdateIcon(device);
        }
    }
}