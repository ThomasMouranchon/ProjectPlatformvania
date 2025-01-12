using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateGameplay : MonoBehaviour
{
    public GameObject gameplayModule;
    public GameObject menuModule;
    public InputReader input;

    private PauseManager pauseManagerScript;

    /*private Vector3 worldPosition;
    public Camera characterCamera;

    public ParticleSystem mouseEffect;
    public TrailRenderer mouseTrailEffect;*/

    //public GameObject pauseMenuModule;

    // Start is called before the first frame update
    void Start()
    {
        pauseManagerScript = PauseManager.Instance;
        //Instantiate<TrailRenderer>(mouseTrailEffect, this.transform.position, this.transform.rotation);
        //SceneManager.LoadSceneAsync("DemoUrp");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Scene scene = SceneManager.GetActiveScene();

        //Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        //Vector3 mousePos = Input.mousePosition;
        //Plane objPlane = new Plane(Camera.main.transform.forward * -1, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
        
        /*Plane objPlane = new Plane(characterCamera.transform.forward * -1, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y));

        Ray mRay = characterCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y));

        float rayDistance;
        if (objPlane.Raycast(mRay, out rayDistance)) mouseTrailEffect.transform.position = mRay.GetPoint(rayDistance);*/

        /*mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);*/

        if (scene.name != "MainMenu"/* && scene.name != "MainMenuCopy"*/)
        {
            gameplayModule.SetActive(true);
            menuModule.SetActive(true);
            //pauseMenuModule.SetActive(true);
            /*if (Time.timeScale != 0) mainMenuModule.SetActive(false);
            else mainMenuModule.SetActive(true);*/
            //Cursor.lockState = CursorLockMode.Locked;

            //Cursor.lockState = CursorLockMode.Confined;

            if (Time.timeScale == 0 && input.isMouseAndKeyboard && pauseManagerScript.canBePaused)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else if (Time.timeScale == 0)
            {
                //Instantiate<ParticleSystem>(mouseEffect, mousePos, this.transform.rotation);
                Debug.Log(Input.mousePosition.x);
                Debug.Log(Input.mousePosition.y);
            }
            else
            {
                Cursor.visible = false;
                //Instantiate<ParticleSystem>(mouseEffect, mousePos, this.transform.rotation);

                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            gameplayModule.SetActive(false);
            menuModule.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            if (input.isMouseAndKeyboard)
            {
                Cursor.visible = true;
                /*Instantiate<ParticleSystem>(mouseEffect, mousePos, this.transform.rotation);
                Debug.Log(Input.mousePosition.x);
                Debug.Log(Input.mousePosition.y);*/
                //Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;
            }
            //pauseMenuModule.SetActive(false);
        }
    }
}
