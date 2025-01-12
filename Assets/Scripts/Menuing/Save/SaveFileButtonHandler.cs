using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFileButtonHandler : MonoBehaviour
{
    private static SaveFileButtonHandler instance = null;
    public static SaveFileButtonHandler Instance => instance;


    public EventSystem eventSystem;
    public Button copyButton, deleteButton;
    private int lastState;
    private int targetFile;

    public Button[] saveFileButtons;
    public Button[] copyFileButtons;
    public Button[] copyTargetFileButtons;
    public Button[] deleteFileButtons;
    [Space(30)]

    public GameObject copyConfirmation;
    public Button copyConfirmationButton;
    public SaveFileIconHandler copiedSaveFileIconHandler, erasedSaveFileIconHandler;
    [Space(10)]

    public GameObject copyNotification;
    public Button copyNotificationButton;
    [Space(10)]

    public GameObject deleteConfirmation;
    public Button deleteConfirmationButton;
    public SaveFileIconHandler deletedSaveFileIconHandler;
    [Space(10)]

    public GameObject deleteConfirmation2;
    public Button deleteConfirmation2Button;
    public SaveFileIconHandler deletedSaveFileIconHandler2;
    [Space(10)]

    public GameObject deleteNotification;
    public Button deleteNotificationButton;
    [Space(30)]

    public Image blackScreenImage;
    private Animator blackScreenAnimator;

    private MainMenuManager mainMenuManager;
    private Button goBackButton;
    public ChangeString changeStringReset;

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
        mainMenuManager = MainMenuManager.Instance;
        goBackButton = mainMenuManager.goBackButton;
        lastState = 0;
        blackScreenAnimator = blackScreenImage.gameObject.GetComponent<Animator>();
        SwitchNavigation(0);
        goBackButton.onClick.Invoke();
        EventSystem.current.SetSelectedGameObject(mainMenuManager.mainMenuButton);
    }

    public void SwitchNavigation(int state)
    {
        for (int i = 0; i < saveFileButtons.Length; i++)
        {
            saveFileButtons[i].enabled = (state == 0);
            saveFileButtons[i].gameObject.SetActive(state == 0);
            saveFileButtons[i].gameObject.GetComponent<SaveFileIconHandler>().LoadElements();

            copyFileButtons[i].enabled = (state == 1);
            copyFileButtons[i].gameObject.SetActive(state == 1);
            copyFileButtons[i].gameObject.GetComponent<SaveFileIconHandler>().LoadElements();

            copyTargetFileButtons[i].enabled = (state == 2);
            copyTargetFileButtons[i].gameObject.SetActive(state == 2);
            copyTargetFileButtons[i].gameObject.GetComponent<SaveFileIconHandler>().LoadElements();

            deleteFileButtons[i].enabled = (state == 3);
            deleteFileButtons[i].gameObject.SetActive(state == 3);
            deleteFileButtons[i].gameObject.GetComponent<SaveFileIconHandler>().LoadElements();
        }

        copyButton.enabled = (state == 0);
        copyButton.gameObject.SetActive(state == 0);
        deleteButton.enabled = (state == 0);
        deleteButton.gameObject.SetActive(state == 0);

        Button[] buttonList = new Button[] { saveFileButtons[0], copyFileButtons[0], copyTargetFileButtons[0], deleteFileButtons[0] };

        goBackButton.onClick.RemoveAllListeners();

        if (state == 0)
        {
            if (lastState == 1 | lastState == 2)
            {
                eventSystem.SetSelectedGameObject(copyButton.gameObject);
            }
            else if (lastState == 3)
            {
                eventSystem.SetSelectedGameObject(deleteButton.gameObject);
            }
            else
            {
                eventSystem.SetSelectedGameObject(saveFileButtons[0].gameObject);
            }

            goBackButton.onClick.AddListener(mainMenuManager.ShowMainMenu);
            changeStringReset.ShowString();
        }
        else
        {
            eventSystem.SetSelectedGameObject(buttonList[state].gameObject);

            /*if (state == 1 | state == 3)*/ goBackButton.onClick.AddListener(SwitchNav0);
            //else goBackButton.onClick.AddListener(SwitchNav1);
        }
        copyConfirmation.SetActive(false);
        copyNotification.SetActive(false);
        deleteConfirmation.SetActive(false);
        deleteConfirmation2.SetActive(false);
        deleteNotification.SetActive(false);
        ShowBlackScreen(false);

        lastState = state;
    }

    private void SwitchNav0()
    {
        SwitchNavigation(0);
    }

    private void SwitchNav1()
    {
        SwitchNavigation(1);
    }

    public void SwitchCopiedFile(int value)
    {
        copiedSaveFileIconHandler.saveFileValue = value;
        copiedSaveFileIconHandler.LoadElements();
    }

    public void SwitchTargetFile(int value)
    {
        targetFile = value;

        erasedSaveFileIconHandler.saveFileValue = value;
        erasedSaveFileIconHandler.LoadElements();

        deletedSaveFileIconHandler.saveFileValue = value;
        deletedSaveFileIconHandler.LoadElements();

        deletedSaveFileIconHandler2.saveFileValue = value;
        deletedSaveFileIconHandler2.LoadElements();
    }

    public void ShowCopyConfirmation(bool show)
    {
        copyConfirmation.SetActive(show);
        blackScreenImage.raycastTarget = show;

        if (show)
        {
            eventSystem.SetSelectedGameObject(copyConfirmationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void ShowCopyNotification(bool show)
    {
        copyConfirmation.SetActive(false);
        copyNotification.SetActive(show);
        blackScreenImage.raycastTarget = show;        

        if (show)
        {
            eventSystem.SetSelectedGameObject(copyNotificationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void SaveSaveFile()
    {
        mainMenuManager.SaveSaveFile(targetFile);
    }

    public void ShowDeleteConfirmation(bool show)
    {
        deleteConfirmation.SetActive(show);
        blackScreenImage.raycastTarget = show;

        if (show)
        {
            eventSystem.SetSelectedGameObject(deleteConfirmationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void ShowDeleteConfirmation2(bool show)
    {
        deleteConfirmation2.SetActive(show);
        blackScreenImage.raycastTarget = show;

        if (show)
        {
            eventSystem.SetSelectedGameObject(deleteConfirmation2Button.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void ShowDeleteNotification(bool show)
    {
        deleteConfirmation.SetActive(false);
        deleteNotification.SetActive(show);

        if (show)
        {
            eventSystem.SetSelectedGameObject(deleteNotificationButton.gameObject);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(SwitchNav0);
        }
    }

    public void DeleteSaveFile()
    {
        mainMenuManager.DeleteSaveFile(targetFile);
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
