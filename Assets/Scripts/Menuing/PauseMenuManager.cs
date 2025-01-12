using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject defaultPanel;
    public GameObject options;

    //public GameObject optionsFirstButton, optionsClosedButton;

    // Start is called before the first frame update
    void Start()
    {
        defaultPanel.SetActive(true);
        options.SetActive(false);
        //Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void SetActiveOptions(bool state)
    {
        options.SetActive(state);
        defaultPanel.SetActive(!state);

        /*// Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        if (state) EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        else EventSystem.current.SetSelectedGameObject(optionsClosedButton);*/
    }

    /*public void EnableOptions()
    {
        options.SetActive(true);
        defaultPanel.SetActive(false);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void DisableOptions()
    {
        options.SetActive(false);
        defaultPanel.SetActive(true);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }*/
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
