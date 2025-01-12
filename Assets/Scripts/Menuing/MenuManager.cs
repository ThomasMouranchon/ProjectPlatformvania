using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject defaultPanel;
    public GameObject defaultMainMenuPanel;
    public GameObject defaultMainMenuBackground;
    public GameObject credits;
    public GameObject options;
    public GameObject optionsBackground;
    public List<MainComponents> check;
    public string sceneToLoadGame;
    public string sceneToLoadMainMenu;
    public GameObject navigationIcon;
    [Space(10)]
    public InputReader input;
    public OptionsValues optionValues;
    [Space(10)]
    public GameObject mainMenuButton, mainMenuOptionButton, pauseMenuButton, optionsFirstButton;
    /*public OptionsSelection optionSelectionMainMenu;
    public OptionsSelection optionSelectionPauseMenu;*/
    // Start is called before the first frame update
    void Start()
    {
        defaultPanel.SetActive(false);
        defaultMainMenuPanel.SetActive(true);
        defaultMainMenuBackground.SetActive(true);
        credits.SetActive(false);
        options.SetActive(false);
        optionsBackground.SetActive(false);

        input.ChangeInputState(false);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);

        Cursor.lockState = CursorLockMode.Confined;

        /*OnStartOptionsSelection(optionSelectionMainMenu);
        OnStartOptionsSelection(optionSelectionPauseMenu);*/

        if (input.isMouseAndKeyboard)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }
    void Update()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        Scene scene = SceneManager.GetActiveScene();
        if (!options.activeSelf)
        {
            if (scene.name != "MainMenu")
            {
                //defaultPanel.SetActive(true);
                defaultMainMenuPanel.SetActive(false);
                defaultMainMenuBackground.SetActive(false);
                /*if (Time.timeScale != 0 | !input.isMouseAndKeyboard)
                {
                    //Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else Cursor.visible = true;*/

                /*if (Time.timeScale == 0 && input.isMouseAndKeyboard) Cursor.visible = true;
                else Cursor.visible = false;*/
                /*if (!input.isMouseAndKeyboard)
                {
                    if (options.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(defaultPanel);
                    }
                }*/
            }
            else
            {
                defaultPanel.SetActive(false);
                defaultMainMenuPanel.SetActive(true);
                defaultMainMenuBackground.SetActive(true);
                /*if (!input.isMouseAndKeyboard)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }*/
            }
        }
    }

    public void LoadGameScene()
    {
        //LoadScene();

        SceneManager.LoadScene(sceneToLoadGame);
        input.ChangeInputState(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //check.Add(FindObjectsOfType(typeof(ActivateGameplay)));
        //MainComponents check =
        /*if (FindObjectsOfType(typeof(ActivateGameplay)).Length > 1)
        {
            foreach (MainComponents element in FindObjectsOfType(typeof(MainComponents)))
            {
                check.Add(element);
            }
            for (int i = 1; i <= check.Count /*FindObjectsOfType(typeof(ActivateGameplay)).Length*//*; i++)
            {
                //Destroy(check[i]);
                Destroy(check[i].gameObject);
                //Debug.Log("tamer");
            }
            //FindObjectsOfType(typeof(ActivateGameplay)).Length[1].Destroy(GameObject);
            /*Destroy(mainComponents);*/
        /*}
        check.Clear();
        /*if (GameObject.Find("MainComponents"))
        {*/
        /*for (int i = 1, i <= FindObjectsOfType(typeof(ActivateGameplay)).Length, i++)
        {
            FindObjectsOfType(typeof(ActivateGameplay))[i].Destroy(mainComponents);
        }*/
        //FindObjectsOfType(typeof(ActivateGameplay)).Length[1].Destroy(GameObject);
        /*Destroy(mainComponents);*/
        //}
        //defaultMainMenuPanel.SetActive(false);
    }

    /*IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoadGame);
        asyncOperation.allowSceneActivation = false;
        /*while (!asyncOperation.isDone)
        {
            //Output the current progress
            //m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }*/
        /*while (!asyncOperation.isDone)
        {
            Debug.Log("yes");
            yield return null;
        }

        /*if (asyncOperation.isDone) *//*asyncOperation.allowSceneActivation = true;

        SceneManager.LoadScene(sceneToLoadGame);
        input.ChangeInputState(true);
    }*/

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(sceneToLoadMainMenu);
        input.ChangeInputState(false);

        Cursor.lockState = CursorLockMode.Confined;

        if (input.isMouseAndKeyboard)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
        /*if (FindObjectsOfType(typeof(ActivateGameplay)).Length > 1)
        {
            foreach (MainComponents element in GameObject.FindObjectsOfType(typeof(MainComponents)))
            {
                check.Add(element);
            }
            for (int i = 1; i <= check.Count /*FindObjectsOfType(typeof(ActivateGameplay)).Length*//*; i++)
            /*{
                //Destroy(check[i]);
                Destroy(check[i].gameObject);
                Debug.Log("tamer");
            }
            //FindObjectsOfType(typeof(ActivateGameplay)).Length[1].Destroy(GameObject);
            /*Destroy(mainComponents);*/
        /*}
        check.Clear();*/
        //if (FindObjectsOfType(typeof(ActivateGameplay)).Length > 1) Destroy(mainComponents);
        //defaultMainMenuPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        StartCoroutine(ShowCreditsCoroutine());
    }

    public IEnumerator ShowCreditsCoroutine()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "MainMenu")
        {
            defaultMainMenuPanel.SetActive(false);
        }
        else
        {
            defaultPanel.SetActive(false);
        }

        navigationIcon.SetActive(false);
        credits.SetActive(true);

        yield return new WaitForSecondsRealtime(2);

        credits.SetActive(false);
        navigationIcon.SetActive(true);

        if (scene.name == "MainMenu")
        {
            defaultPanel.SetActive(false);
            defaultMainMenuPanel.SetActive(true);
            defaultMainMenuBackground.SetActive(true);
        }
        else
        {
            defaultMainMenuPanel.SetActive(false);
            defaultMainMenuBackground.SetActive(false);
        }
    }

    public void SetActiveOptions(bool state)
    {
        Scene scene = SceneManager.GetActiveScene();
        options.SetActive(state);
        optionsBackground.SetActive(state);
        //optionValues.UpdateAllOptions();
        if (scene.name != "MainMenu")
        {
            defaultPanel.SetActive(!state);
            //defaultMainMenuPanel.SetActive(!state);

            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
            if (state) EventSystem.current.SetSelectedGameObject(optionsFirstButton);
            else EventSystem.current.SetSelectedGameObject(pauseMenuButton);
        }
        else
        {
            defaultMainMenuPanel.SetActive(!state);
            defaultMainMenuBackground.SetActive(!state);

            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
            if (state) EventSystem.current.SetSelectedGameObject(optionsFirstButton);
            else EventSystem.current.SetSelectedGameObject(mainMenuOptionButton);
        }
    }
    /*
    public void HideOptions()
    {
        buttons.SetActive(true);
        options.SetActive(false);
    }*/

    public void QuitGame()
    {
        Application.Quit();
    }
}
