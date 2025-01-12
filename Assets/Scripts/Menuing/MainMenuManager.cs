using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance = null;
    public static MainMenuManager Instance => instance;


    public GameObject defaultMainMenuPanel;
    public GameObject saveFilesPanel;
    public GameObject credits;
    public GameObject options;
    public List<MainComponents> check;
    public string sceneToLoadGame;
    public GameObject navigationIcon;
    [Space(10)]
    public InputReader inputReader;
    private OptionsValues optionsValues;
    private GameLoader levelLoader;
    [Space(10)]
    public GameObject mainMenuButton, saveFilesFirstButton, mainMenuOptionButton, optionsFirstButton;

    [Space(10)]
    private bool back;
    public Button goBackButton;
    [HideInInspector]
    public float afterBackTimer;

    private SaveFileButtonHandler saveFileButtonHandler;

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

    void Start()
    {
        defaultMainMenuPanel.SetActive(true);
        saveFilesPanel.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);

        levelLoader = GameLoader.Instance;
        saveFileButtonHandler = SaveFileButtonHandler.Instance;

        optionsValues = FindObjectOfType<OptionsValues>();

        inputReader.ChangeInputState(false);
        Time.timeScale = 1;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);

        if (inputReader.isMouseAndKeyboard)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LoadGameScene()
    {
        //SceneManager.LoadScene(sceneToLoadGame);
        inputReader.ChangeInputState(true);
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        levelLoader.LoadLevel(sceneToLoadGame);
    }

    public void ShowMainMenu()
    {
        if (!defaultMainMenuPanel.activeSelf) StartCoroutine(ShowMainMenuCoroutine());
    }

    IEnumerator ShowMainMenuCoroutine()
    {
        defaultMainMenuPanel.SetActive(true);
        saveFilesPanel.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);

        saveFileButtonHandler = SaveFileButtonHandler.Instance;
        saveFileButtonHandler.SwitchNavigation(0);

        yield return new WaitForFixedUpdate();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);

    }

    public void ShowCredits()
    {
        StartCoroutine(ShowCreditsCoroutine());
    }

    public IEnumerator ShowCreditsCoroutine()
    {
        defaultMainMenuPanel.SetActive(false);

        navigationIcon.SetActive(false);
        credits.SetActive(true);

        yield return new WaitForSecondsRealtime(2);

        credits.SetActive(false);
        navigationIcon.SetActive(true);
                
        defaultMainMenuPanel.SetActive(true);
    }

    public void SetActiveSaveFiles(bool state)
    {
        Scene scene = SceneManager.GetActiveScene();
        saveFilesPanel.SetActive(state);
        defaultMainMenuPanel.SetActive(!state);

        EventSystem.current.SetSelectedGameObject(null);
        if (state) EventSystem.current.SetSelectedGameObject(saveFilesFirstButton);
        else EventSystem.current.SetSelectedGameObject(mainMenuOptionButton);
    }

    public void SetActiveOptions(bool state)
    {
        Scene scene = SceneManager.GetActiveScene();
        options.SetActive(state);
        defaultMainMenuPanel.SetActive(!state);
        InputReader.Instance.ReloadMovementActions();

        EventSystem.current.SetSelectedGameObject(null);
        if (state) EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        else
        {
            EventSystem.current.SetSelectedGameObject(mainMenuOptionButton);
            optionsValues.SaveConfiguration();
        }
    }

    public void QuitGame()
    {
        levelLoader.QuitGame();
    }

    public void LoadSaveFile(int selectedSaveFile)
    {
        SaveManager.Instance.Load(selectedSaveFile);
    }

    public void SaveSaveFile(int selectedSaveFile)
    {
        SaveManager.Instance.Save(selectedSaveFile);
    }

    public void DeleteSaveFile(int selectedSaveFile)
    {
        SaveManager.Instance.Delete(selectedSaveFile);
    }

    IEnumerator LoadSaveFileCoroutine(int selectedSaveFile)
    {
        SaveManager.Instance.Load(selectedSaveFile);

        yield return new WaitForSeconds(0.1f);

        inputReader.ChangeInputState(true);
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        levelLoader.LoadLevel(sceneToLoadGame);
    }

    void Update()
    {
        back = inputReader.back;
        MoveBack();
    }

    private void MoveBack()
    {
        if (back && afterBackTimer == 0)
        {
            afterBackTimer++;
            goBackButton.onClick.Invoke();
        }
        else if (afterBackTimer > 0 && afterBackTimer <= 0.5f) afterBackTimer++;
        else if (!back && afterBackTimer > 0.5f) afterBackTimer = 0;
    }
}
