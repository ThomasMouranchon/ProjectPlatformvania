using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class PauseManager : MonoBehaviour
{
    private static PauseManager instance = null;
    public static PauseManager Instance => instance;

    private ScriptLocations scriptLocations;
    [Space(10)]

    [Tooltip("Input reference")]
    public InputReader inputReader;
    public GameMenuManager gameMenuManager;
    private CameraFreeLookController cameraFreeLookController;
    private CinemachineBrain cinemachineBrain;
    public UIPauseCharacterHandler pauseCharacterHandler;
    public InputSystemUIInputModule inputSystemUIInputModule;
    private HealthManager healthManager;
    private SaveFilePauseButtonHandler saveFilePauseButtonHandler;
    private bool pause;
    private float afterPauseTimer;
    private bool back;
    [HideInInspector]
    public float afterBackTimer;
    private Vector2 axisInput;
    [Space(10)]
    public bool canBePaused;

    [Space(10)]

    public GameObject menu, menuBackground;
    public Animator menuAnim;
    public Animator pauseTextAnim;
    public Animator mainPanelButtonsAnim, mainPanelBgColorAnim;
    //public Mask defaultPanelMask;
    [Space(5)]
    public GameObject inGameUI;
    public TMP_Text currentMenuText;
    public LocalizeStringEvent currentMenuTextStringEvent;
    private string initialMenuText;
    public Animator darkScreenAnim;
    [Space(5)]
    //public GameObject projectilesTarget;
    [Space(5)]
    public GameObject gotPhotoUI;
    //private MMFeedbacks gotPhotoUIFeedback;
    [HideInInspector] public Animator gotPhotoUIAnim;
    public Animator photoUIValidationAnim;
    [Space(5)]
    public GameObject pauseFirstButton;
    public OptionsValues optionsValues;
    [Space(10)]
    public GameObject navigationIcon;
    [HideInInspector]
    public Animator navigationIconAnim;
    [Space(10)]
    public GameObject goBackButtonGo;
    [HideInInspector] public Button goBackButton;
    /*private MMFeedbacks goBackFeedback;
    public MMFeedbacks goBackInvertFeedback;*/
    [Space(10)]
    [HideInInspector]
    public bool enableRightInput;
    [HideInInspector]
    public bool isOnRight;
    [HideInInspector]
    public bool isOnMainPanel;

    // Start is called before the first frame update
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

        canBePaused = true;
        //menuAnim = menuBackground.gameObject.GetComponent<Animator>();
        gotPhotoUIAnim = gotPhotoUI.gameObject.GetComponent<Animator>();
        //goBackFeedback = goBackButtonGo.gameObject.GetComponent<MMFeedbacks>();
        goBackButton = goBackButtonGo.GetComponent<Button>();
    }

    void Start()
    {
        scriptLocations = ScriptLocations.Instance;

        optionsValues = scriptLocations.optionsValues;
        healthManager = HealthManager.Instance;
        cameraFreeLookController = CameraFreeLookController.Instance;
        saveFilePauseButtonHandler = SaveFilePauseButtonHandler.Instance;
        cinemachineBrain = scriptLocations.cinemachineBrain;
        inGameUI = scriptLocations.inGameUI;
        inputReader.enablePause = true;
        navigationIconAnim = navigationIcon.GetComponent<Animator>();
        currentMenuTextStringEvent.RefreshString();
        initialMenuText = currentMenuText.text;
        enableRightInput = true;
        BeginGame();
    }

    // Update is called once per frame
    void Update()
    {
        pause = inputReader.pause;
        back = inputReader.back;
        axisInput = inputSystemUIInputModule.move.action.ReadValue<Vector2>();
        MovePause();
        MoveBack();
        MoveRight();
        /*
        if (input.isMouseAndKeyboard && Time.timeScale == 0) Cursor.visible = true;
        else Cursor.visible = false;*/

        /*if (!input.isMouseAndKeyboard | Time.timeScale != 0) Cursor.visible = false;
        else Cursor.visible = true;*/
        /*if (pause && afterPauseTimer == 0)
        {
            afterPauseTimer++;
            Debug.Log("yes");
            if (Time.timeScale == 0) Time.timeScale = 1;
            else Time.timeScale = 0;
        }
        else if (afterPauseTimer > 0 && afterPauseTimer <= 10) afterPauseTimer++;
        else if (afterPauseTimer >= 10) afterPauseTimer = 0;*/
    }

    private void MovePause()
    {
        if (pause && afterPauseTimer == 0 && canBePaused && menuAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            afterPauseTimer++;
            enableRightInput = true;
            if (Time.timeScale == 0 && (menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleOn") | menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleRight")) )
            {
                StartCoroutine(ResumeGameCoroutine());
            }
            else if (menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleOff"))
            {
                PauseGame();
            }
        }
        else if (afterPauseTimer > 0 && afterPauseTimer <= 0.5f) afterPauseTimer += Time.timeScale;
        else if (!pause && afterPauseTimer > 0.5f) afterPauseTimer = 0;
    }

    private void MoveBack()
    {
        if (back && afterBackTimer == 0 && afterPauseTimer == 0 && !pause && canBePaused /*&& !menuFeedback.IsPlaying && !menuBackFeedback.IsPlaying*/)
        {
            afterBackTimer++;
            goBackButton.onClick.Invoke();
        }
        else if (afterBackTimer > 0 && afterBackTimer <= 0.5f) afterBackTimer++;
        else if (!back && afterBackTimer > 0.5f) afterBackTimer = 0;
    }

    private void MoveRight()
    {
        if (enableRightInput && afterPauseTimer == 0 && isOnMainPanel && Time.timeScale == 0)
        {
            if (axisInput.x > 0.65f && !isOnRight && menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleOn"))
            {
                StartCoroutine(openRightMenuCoroutine());
            }
            else if (axisInput.x < -0.65f && isOnRight && menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleRight"))
            {
                StartCoroutine(resetRightMenuCoroutine());
            }
        }
    }

    IEnumerator openRightMenuCoroutine()
    {
        menuAnim.CrossFade("MainPanel_OpenRight", 0);
        pauseTextAnim.CrossFade("PauseText_RotateRight", 0);

        navigationIconAnim.CrossFade("NavigationIcon_IdleOff", 0);
        isOnRight = true;
        afterPauseTimer += Time.deltaTime;
        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForSecondsRealtime(0.1f);

        pauseCharacterHandler.StopPauseMenu();
    }

    IEnumerator resetRightMenuCoroutine()
    {
        menuAnim.CrossFade("MainPanel_OpenLeft", 0);
        pauseTextAnim.CrossFade("PauseText_RotateLeft", 0);
        afterPauseTimer += Time.deltaTime;
        pauseCharacterHandler.UpdatePauseMenu(0);

        yield return new WaitForSecondsRealtime(0.1f);
        navigationIconAnim.CrossFade("NavigationIcon_OpenRight", 0);

        yield return new WaitForSecondsRealtime(0.05f);
        
        isOnRight = false;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    /*
    private void ShowMenu()
    {
        menu.SetActive(true);
    }
    private void HideMenu()
    {
        menu.SetActive(false);
    }*/
    
    public void PauseGame()
    {
        if (Time.timeScale > 0 && !healthManager.isRespawning && canBePaused && inputReader.enableValidate)
        {
            StartCoroutine(PauseGameCoroutine());
        }
    }
    
    IEnumerator PauseGameCoroutine()
    {
        menu.SetActive(true);
        menuBackground.SetActive(true);
        //defaultPanelMask.enabled = false;

        menuAnim.CrossFade("MainPanel_Open", 0);
        currentMenuText.text = initialMenuText;
        /*
        if (inputReader.isMouseAndKeyboard)
        {
            goBackButtonGo.SetActive(true);
            goBackFeedback.PlayFeedbacks();
        }*/
        goBackButton.onClick.RemoveAllListeners();
        goBackButton.onClick.AddListener(ResumeGame);
        //inGameUI.SetActive(false);
        gotPhotoUI.SetActive(false);
        //projectilesTarget.SetActive(false);

        isOnMainPanel = true;
        isOnRight = false;
        canBePaused = false;
        inputReader.enableValidate = false;

        healthManager.HideUI(false);

        gameMenuManager.DisableSubMenus();
        Cursor.lockState = CursorLockMode.Confined;
        darkScreenAnim.CrossFade("Background_PauseStart", 0);

        yield return new WaitForSecondsRealtime(0.05f);

        canBePaused = true;
        inputReader.enableValidate = true;

        mainPanelButtonsAnim.CrossFade("MainPanelButtons_Open", 0);
        mainPanelBgColorAnim.SetInteger("panel", 0);

        navigationIconAnim.CrossFade("NavigationIcon_Open", 0);

        pauseCharacterHandler.UpdatePauseMenu(0);
        //costumeHandler.PauseCostumeSwitch(false);

        if (inputReader.isMouseAndKeyboard)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        //DisableInputState();
        Time.timeScale = 0;
        cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void BeginGame()
    {
        //WaitBeforeResuming();

        menu.SetActive(false);
        menuBackground.SetActive(false);
        //defaultPanelMask.enabled = true;

        goBackButtonGo.SetActive(false);
        inGameUI.SetActive(true);
        gotPhotoUI.SetActive(false);
        gameMenuManager.DisableSubMenus();

        navigationIconAnim.CrossFade("NavigationIcon_IdleOff", 0);

        optionsValues.UpdateAllOptions();

        darkScreenAnim.CrossFade("Background_PauseEnd", 0);

        pauseCharacterHandler.UpdatePauseMenu(0);
        pauseCharacterHandler.StopPauseMenu();
        //costumeHandler.PauseCostumeSwitch(false);

        isOnRight = false;
        isOnMainPanel = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //EnableInputState();
        Time.timeScale = 1;
        cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
    }
    public void ResumeGame()
    {
        //WaitBeforeResuming();
        if (Time.timeScale == 0 && canBePaused && inputReader.enableValidate && afterPauseTimer == 0 
            && menuAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            (menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleOn") |
            menuAnim.GetCurrentAnimatorStateInfo(0).IsName("MainPanel_IdleRight")) )
        {
            StartCoroutine(ResumeGameCoroutine());
        }
    }

    public IEnumerator ResumeGameCoroutine()
    {
        if (isOnRight)
        {
            menuAnim.CrossFade("MainPanel_CloseRight", 0);
        }
        else
        {
            menuAnim.CrossFade("MainPanel_Close", 0);
        }

        //if (currentMenuText.text == "Pause") mainPanelButtonsAnim.CrossFade("MainPanelButtons_Close", 0);
        mainPanelBgColorAnim.SetInteger("panel", 0);
        currentMenuText.text = initialMenuText;

        //goBackInvertFeedback.PlayFeedbacks();

        navigationIconAnim.CrossFade("NavigationIcon_IdleOff", 0);
        gameMenuManager.DisableSubMenusFeedbacks();

        canBePaused = false;
        inputReader.enableValidate = false;

        saveFilePauseButtonHandler.SwitchNavigation(0);

        yield return new WaitForSecondsRealtime(0.05f);
        pauseTextAnim.CrossFade("PauseText_RotateLeft", 0);
        yield return new WaitForSecondsRealtime(0.16f);
        menu.SetActive(false);
        menuBackground.SetActive(false);
        pauseCharacterHandler.StopPauseMenu();

        //defaultPanelMask.enabled = true;
        canBePaused = true;
        inputReader.enableValidate = true;
        isOnRight = false;
        isOnMainPanel = true;

        //goBackButtonGo.SetActive(false);

        gameMenuManager.DisableSubMenus();

        darkScreenAnim.CrossFade("Background_PauseEnd", 0);

        inGameUI.SetActive(true);
        gotPhotoUI.SetActive(false);
        /*if (healthManager.currentHealth >= 3) projectilesTarget.SetActive(true);
        else projectilesTarget.SetActive(false);*/

        optionsValues.UpdateAllOptions();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        EnableInputState();
        Time.timeScale = 1;
        cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
    }

    public void SetActiveMainPauseMenu()
    {
        StartCoroutine(SetActiveMainPauseMenuCoroutine());
    }

    public IEnumerator SetActiveMainPauseMenuCoroutine()
    {
        menu.SetActive(true);
        
        if (mainPanelButtonsAnim.isActiveAndEnabled) mainPanelButtonsAnim.CrossFade("MainPanelButtons_Open", 0);
        mainPanelBgColorAnim.SetInteger("panel", 0);
        currentMenuText.text = initialMenuText;
        //defaultPanelMask.enabled = false;

        //inGameUI.SetActive(false);
        gotPhotoUI.SetActive(false);
        //projectilesTarget.SetActive(false);
        gameMenuManager.DisableSubMenusFeedbacks();

        navigationIconAnim.CrossFade("NavigationIcon_Open", 0);

        Cursor.lockState = CursorLockMode.Confined;

        pauseCharacterHandler.UpdatePauseMenu(0);
        //costumeHandler.PauseCostumeSwitch(false);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        goBackButton.onClick.RemoveAllListeners();
        goBackButton.onClick.AddListener(ResumeGame);
        isOnMainPanel = true;
        yield return new WaitForSecondsRealtime(0.3f);

        gameMenuManager.DisableSubMenus();
    }

    private void ChangeInputState()
    {
        inputReader.enableJump = !inputReader.enableJump;
        inputReader.enableDash = !inputReader.enableDash;
        inputReader.enableYoyo = !inputReader.enableYoyo;
        inputReader.enableCameraSwitch = !inputReader.enableCameraSwitch;
        cameraFreeLookController.enabled = cameraFreeLookController.enabled;
        cinemachineBrain.enabled = false;
        canBePaused = !canBePaused;
        /*input.enableJump = !input.enableJump;
        input.enableJump = !input.enableJump;*/
    }

    public void EnableInputState()
    {
        inputReader.enableJump = true;
        inputReader.enableDash = true;
        inputReader.enableYoyo = true;
        inputReader.enableCameraSwitch = true;
        cameraFreeLookController.enabled = true;
        cinemachineBrain.enabled = true;
        inputReader.enableAxisInput = true;
        inputReader.enableBomb = true;
        //canBePaused = true;
        /*input.enableJump = !input.enableJump;
        input.enableJump = !input.enableJump;*/
    }

    public void DisableInputState()
    {
        inputReader.enableJump = false;
        inputReader.enableDash = false;
        inputReader.enableYoyo = false;
        inputReader.enableCameraSwitch = false;
        inputReader.enableAxisInput = false;
        inputReader.enableBomb = false;

        //canBePaused = false;
        /*input.enableJump = !input.enableJump;
        input.enableJump = !input.enableJump;*/
    }

    IEnumerator WaitBeforeResuming()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void ToggleSubMenu()
    {
        mainPanelButtonsAnim.CrossFade("MainPanelButtons_Close", 0);
        //StartCoroutine(ToggleSubMenuCoroutine());
    }

    IEnumerator ToggleSubMenuCoroutine()
    {
        //canBePaused = false;
        inputReader.enableBack = false;
        //goBackButton.interactable = false;
        mainPanelButtonsAnim.CrossFade("MainPanelButtons_Close", 0);
        yield return new WaitForSecondsRealtime(0.8f);
        //canBePaused = true;
        inputReader.enableBack = true;
        //goBackButton.interactable = true;
    }

    public void ShowGotPhotoUI()
    {
        StartCoroutine(ShowGotPhotoUICoroutine());
    }

    IEnumerator ShowGotPhotoUICoroutine()
    {
        gotPhotoUI.SetActive(true);
        gotPhotoUIAnim.CrossFade("GotPhoto_Show", 0);
        gameMenuManager.DisableSubMenus();

        canBePaused = false;
        //DisableInputState();
        //if (!menuAnim.GetNextAnimatorStateInfo(0).IsName("MainPanel_IdleOff"))
        //AnimatorStateInfo animatorStateInfo = menuAnim.GetNextAnimatorStateInfo(0);

        CheckPauseMenu();

        menu.SetActive(false);
        menuBackground.SetActive(false);

        photoUIValidationAnim.SetBool("play", false);

        yield return new WaitForSecondsRealtime(0.1f);

        while (gotPhotoUIAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        photoUIValidationAnim.SetBool("play", true);
    }

    public void HideGotPhotoUI()
    {
        StartCoroutine(HideGotPhotoUICoroutine());
    }

    IEnumerator HideGotPhotoUICoroutine()
    {
        menu.SetActive(false);
        menuBackground.SetActive(false);
        //defaultPanelMask.enabled = true;

        inGameUI.SetActive(true);
        navigationIcon.SetActive(true);
        navigationIconAnim.CrossFade("NavigationIcon_IdleOff", 0);
        //gameMenuManager.DisableSubMenus();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gotPhotoUIAnim.CrossFade("GotPhoto_Hide", 0);
        photoUIValidationAnim.SetBool("play", false);

        yield return new WaitForSecondsRealtime(0.1f);
        
        while (gotPhotoUIAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        gotPhotoUI.SetActive(false);
    }

    public void CheckPauseMenu()
    {
        if (!menuAnim.GetNextAnimatorStateInfo(0).IsName("MainPanel_IdleOff"))
        {
            menuAnim.CrossFade("MainPanel_IdleOff", 0);
            if (navigationIcon.activeSelf)
            {
                navigationIconAnim.CrossFade("NavigationIcon_IdleOff", 0);
            }
            pauseCharacterHandler.StopPauseMenu();

            navigationIcon.SetActive(false);

            darkScreenAnim.CrossFade("Background_PauseEnd", 0);

            isOnRight = false;
            isOnMainPanel = true;

            //goBackButtonGo.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        }
    }
}
