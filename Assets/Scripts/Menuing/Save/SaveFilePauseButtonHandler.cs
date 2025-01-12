using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFilePauseButtonHandler : MonoBehaviour
{
    private static SaveFilePauseButtonHandler instance = null;
    public static SaveFilePauseButtonHandler Instance => instance;


    public EventSystem eventSystem;
    private int lastState;
    private int targetFile;
    [Space(30)]

    public GameObject saveConfirmation;
    private Animator saveConfirmationAnim;
    public Button saveConfirmationButton;
    public SaveFileIconHandler savedSaveFileIconHandler, erasedSaveFileIconHandler;
    [Space(10)]

    public GameObject saveNotification;
    private Animator saveNotificationAnim;
    public Button saveNotificationButton;
    [Space(10)]

    public GameObject mainMenuConfirmation;
    private Animator mainMenuConfirmationAnim;
    public Button mainMenuConfirmationButton;
    [Space(10)]

    public Image blackScreenImage;
    private Animator blackScreenAnimator;
    [Space(10)]

    public PauseManager pauseManager;
    private Button goBackButton;

    private SaveManager saveManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        saveManager = SaveManager.Instance;
        pauseManager = PauseManager.Instance;
        goBackButton = pauseManager.goBackButton;
        lastState = 0;
        saveConfirmationAnim = saveConfirmation.GetComponent<Animator>();
        saveConfirmationAnim.CrossFade("PausePopup_idleOff", 0);

        saveNotificationAnim = saveNotification.GetComponent<Animator>();
        saveNotificationAnim.CrossFade("PausePopup_idleOff", 0);

        mainMenuConfirmationAnim = mainMenuConfirmation.GetComponent<Animator>();
        mainMenuConfirmationAnim.CrossFade("PausePopup_idleOff", 0);
        
        blackScreenAnimator = blackScreenImage.gameObject.GetComponent<Animator>();
        
        SwitchNavigation(0);

        ShowBlackScreen(false);
    }

    public void SwitchNavigation(int state)
    {
        pauseManager.enableRightInput = (state == 0);

        if (state == 1)
        {
            SwitchFile();
            saveConfirmationAnim.CrossFade("PausePopup_Open", 0);
        }
        else if (!saveConfirmationAnim.GetCurrentAnimatorStateInfo(0).IsName("PausePopup_idleOff"))
        {
            saveConfirmationAnim.CrossFade("PausePopup_Close", 0);
        }

        if (state == 2)
        {
            saveNotificationAnim.CrossFade("PausePopup_Open", 0);
        }
        else if (!saveNotificationAnim.GetCurrentAnimatorStateInfo(0).IsName("PausePopup_idleOff"))
        {
            saveNotificationAnim.CrossFade("PausePopup_Close", 0);
        }

        if (state == 3)
        {
            mainMenuConfirmationAnim.CrossFade("PausePopup_Open", 0);
        }
        else if (!mainMenuConfirmationAnim.GetCurrentAnimatorStateInfo(0).IsName("PausePopup_idleOff"))
        {
            mainMenuConfirmationAnim.CrossFade("PausePopup_Close", 0);
        }

        ShowBlackScreen(state != 0);

        goBackButton.onClick.RemoveAllListeners();

        if (state == 0)
        {
            eventSystem.SetSelectedGameObject(pauseManager.pauseFirstButton);
            goBackButton.onClick.AddListener(pauseManager.ResumeGame);
        }
        else
        {
            goBackButton.onClick.AddListener(SwitchNav0);
        }

        lastState = state;
    }

    private void SwitchNav0()
    {
        SwitchNavigation(0);
    }

    public void SwitchFile()
    {
        savedSaveFileIconHandler.saveFileValue = saveManager.currentSaveFile;
        savedSaveFileIconHandler.LoadCurrentElements();

        erasedSaveFileIconHandler.saveFileValue = saveManager.currentSaveFile;
        erasedSaveFileIconHandler.LoadElements();
    }

    public void ShowSaveConfirmation(bool show)
    {
        saveConfirmation.SetActive(show);
        blackScreenImage.raycastTarget = show;

        if (show)
        {
            eventSystem.SetSelectedGameObject(saveConfirmationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void ShowSaveNotification(bool show)
    {
        saveNotification.SetActive(show);
        blackScreenImage.raycastTarget = show;        

        if (show)
        {
            eventSystem.SetSelectedGameObject(saveNotificationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void ShowMainMenuConfirmation(bool show)
    {
        mainMenuConfirmation.SetActive(show);
        blackScreenImage.raycastTarget = show;        

        if (show)
        {
            eventSystem.SetSelectedGameObject(mainMenuConfirmationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void SaveSaveFile()
    {
        saveManager.Save(saveManager.currentSaveFile);
    }


    public void ShowBlackScreen(bool show)
    {
        blackScreenImage.raycastTarget = show;
        if (show)
        {
            blackScreenAnimator.CrossFade("Background_PauseStart", 0);
        }
        else
        {
            blackScreenAnimator.CrossFade("Background_PauseEnd", 0);
        }
    }
}
