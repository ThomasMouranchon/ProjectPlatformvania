using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
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
    [HideInInspector] public bool enableGlide = true;
    [HideInInspector] public bool enableBomb = true;
    [HideInInspector] public bool enableDash = true;
    [HideInInspector] public bool enableYoyo = true;
    [HideInInspector] public bool enableCameraSwitch = true;
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
    private InputAction glideAction;
    private InputAction bombAction;
    private InputAction dashAction;
    private InputAction yoyoAction;
    private InputAction cameraSwitchAction;
    private InputAction pauseAction;
    private InputAction validateAction;
    private InputAction backAction;


    /**/


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
        ReloadMovementActions();
    }

    public void ReloadMovementActions()
    {
        movementActions = new MovementActions();

        movementActions.Menu.Click.performed += ctx => OnClick(ctx);
        movementActions.Menu.Click.canceled += ctx => ClickEnded(ctx);
    }


    //ENABLE if using old input system
    private void Update()
    {
        //Debug.Log(string.Format("Current Control Scheme: {0}", movementActions.controlSchemes));
        /*

        axisInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

        if (enableJump)
        {
            if (Input.GetButtonDown("Jump")) OnJump();
            if (Input.GetButtonUp("Jump")) JumpEnded();
        }

        if (enableSprint) sprint = Input.GetButton("Fire3");
        if (enableCrouch) crouch = Input.GetButton("Fire1");

        GetDeviceOld();

        */
        SetupInputActions();

        UpdateMoveInput();
        UpdateCameraInput();
        UpdateJumpInput();
        UpdateGlideInput();
        UpdateBombInput();
        UpdateDashInput();
        UpdateYoyoInput();
        UpdateCameraSwitchInput();
        UpdatePauseInput();
        UpdateValidateInput();
        UpdateBackInput();

        if (hasValidated && skippedFrame)
        {
            validate = false;
            hasValidated = false;
        }
        
        if (hasClicked && skippedFrame)
        {
            click = false;
            hasClicked = false;
        }
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


    //ENABLE if using old input system
    private void GetDeviceOld()
    {
        /*

        oldInput = isMouseAndKeyboard;

        if (Input.GetJoystickNames().Length > 0) isMouseAndKeyboard = false;
        else isMouseAndKeyboard = true;

        if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
        else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();

        */
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
        glideAction = playerInput.actions["Glide"];
        bombAction = playerInput.actions["Bomb"];
        dashAction = playerInput.actions["Dash"];
        yoyoAction = playerInput.actions["Yoyo"];
        cameraSwitchAction = playerInput.actions["CameraSwitch"];
        pauseAction = playerInput.actions["Pause"];
        validateAction = playerInput.actions["Validate"];
        backAction = playerInput.actions["Back"];
    }

    //DISABLE if using old input system

    public void UpdateMoveInput()
    {
        if (enableAxisInput)
        {
            axisInput = axisInputAction.ReadValue<Vector2>();
        }
    }

    public void UpdateJumpInput()
    {
        if (enableJump)
        {
            if (jumpAction.WasPressedThisFrame())
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
            else if (jumpAction.WasReleasedThisFrame())
            {
                jump = false;
                jumpHold = false;
            }
        }
    }

    public void UpdateGlideInput()
    {
        if (enableGlide)
        {
            if (glideAction.WasPressedThisFrame())
            {
                glide = true;
                glideHold = true;

                hasGlide = true;
                skippedFrame = false;
            }
            else if (glideAction.WasReleasedThisFrame())
            {
                glide = false;
                glideHold = false;
            }
        }
    }

    public void UpdateBombInput()
    {
        if (enableBomb)
        {
            if (bombAction.WasPressedThisFrame())
            {
                bomb = true;
                bombHold = true;

                hasUsedBomb = true;
                skippedFrame = false;
            }
            else if (bombAction.WasReleasedThisFrame())
            {
                bomb = false;
                bombHold = false;
            }
        }
    }

    public void UpdateDashInput()
    {
        if (enableDash)
        {
            if (dashAction.WasPressedThisFrame())
            {
                dash = true;
                dashHold = true;

                hasDashed = true;
                skippedFrame = false;
            }
            else if (dashAction.WasReleasedThisFrame())
            {
                dash = false;
                dashHold = false;
            }
        }
    }

    public void UpdateYoyoInput()
    {
        if (enableYoyo)
        {
            if (yoyoAction.WasPressedThisFrame())
            {
                yoyo = true;
                yoyoHold = true;

                hasUsedYoyo = true;
                skippedFrame = false;
            }
            else if (yoyoAction.WasReleasedThisFrame())
            {
                yoyo = false;
                yoyoHold = false;
            }
        }
    }

    public void UpdateCameraSwitchInput()
    {
        if (enableCameraSwitch)
        {
            if (cameraSwitchAction.WasPressedThisFrame())
            {
                cameraSwitch = true;
                cameraSwitchHold = true;

                hasCameraSwitched = true;
                skippedFrame = false;
            }
            else if (cameraSwitchAction.WasReleasedThisFrame())
            {
                cameraSwitch = false;
                cameraSwitchHold = false;
            }
        }
    }

    public void UpdatePauseInput()
    {
        if (enablePause)
        {
            if (pauseAction.WasPressedThisFrame())
            {
                pause = true;

                hasPaused = true;
                skippedFrame = false;
            }
            else if (pauseAction.WasReleasedThisFrame())
            {
                pause = false;
            }
        }
    }

    public void UpdateValidateInput()
    {
        if (enableValidate)
        {
            if (validateAction.WasPressedThisFrame())
            {
                validate = true;
                validateHold = true;

                hasValidated = true;
                skippedFrame = false;
            }
            else if (validateAction.WasReleasedThisFrame())
            {
                validate = false;
                validateHold = false;
            }
        }
    }

    public void UpdateBackInput()
    {
        if (enableBack)
        {
            if (backAction.WasPressedThisFrame())
            {
                back = true;
                backHold = true;

                hasBacked = true;
                skippedFrame = false;
            }
            else if (backAction.WasReleasedThisFrame())
            {
                back = false;
                backHold = false;
            }
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
    }

    public void ClickEnded(InputAction.CallbackContext ctx)
    {
        click = false;
        clickHold = false;
    }

    private void FixedUpdate()
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
        }
        if (hasBacked && skippedFrame)
        {
            back = false;
            hasBacked = false;
        }
        if (!skippedFrame && enableJump) skippedFrame = true;
    }

    public void UpdateCameraInput()
    {
        if (enableCameraInput)
        {
            cameraInput = cameraInputAction.ReadValue<Vector2>();
        }
    }

    /*//DISABLE if using old input system
    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (enableCrouch) crouch = true;
    }


    //DISABLE if using old input system
    public void CrouchEnded(InputAction.CallbackContext ctx)
    {
        crouch = false;
    }*/

    #endregion


    #region Enable / Disable

    //DISABLE if using old input system
    private void OnEnable()
    {
        movementActions.Enable();
        InputSystem.onDeviceChange += OnDeviceChange;
    }


    //DISABLE if using old input system
    private void OnDisable()
    {
        movementActions.Disable();
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    #endregion

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
        {
            // Vérifiez si le périphérique est une manette
            if (device is Gamepad gamepad)
            {
                Debug.Log("Manette connectée : " + gamepad.name);
                SwitchControlScheme(gamepad);
            }
        }
    }

    private void SwitchControlScheme(Gamepad gamepad)
    {
        if (gamepad is DualShockGamepad)
        {
            Debug.Log("Switching to DualShock control scheme.");
            InputUser.PerformPairingWithDevice(gamepad, InputUser.all[0]);
            movementActions.bindingMask = InputBinding.MaskByGroup("PsController");
        }
        else if (gamepad is XInputController)
        {
            Debug.Log("Switching to Xbox control scheme.");
            InputUser.PerformPairingWithDevice(gamepad, InputUser.all[0]);
            movementActions.bindingMask = InputBinding.MaskByGroup("XboxController");
        }
        else if (gamepad is SwitchProControllerHID)
        {
            Debug.Log("Switching to Switch control scheme.");
            InputUser.PerformPairingWithDevice(gamepad, InputUser.all[0]);
            movementActions.bindingMask = InputBinding.MaskByGroup("SwitchController");
        }
        else
        {
            Debug.LogWarning("Manette non reconnue.");
        }
    }
}