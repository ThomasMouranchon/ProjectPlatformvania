using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScriptLocations : MonoBehaviour
{
    private static ScriptLocations instance = null;
    public static ScriptLocations Instance => instance;

    public CharacterManager characterManager;
    [Space(5)]
    public GameObject gameplayModule;
    [Space(5)]
    public CameraFreeLookController cameraFreeLookController;
    public CinemachineFreeLook cinemachineFreeLook;
    public Camera mainCamera;
    public CinemachineBrain cinemachineBrain;
    [Space(5)]
    public GameObject inGameUI;
    public OptionsValues optionsValues;

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

        gameplayModule = GameObject.FindGameObjectWithTag("MainComponents").transform.Find("Gameplay").gameObject;

        characterManager = gameplayModule.transform.Find("Player").transform.Find("PlayerObject").gameObject.GetComponent<CharacterManager>();

        /*healthManager = gameplayModule.transform.Find("InGameManager").gameObject.GetComponent<HealthManager>();
        gameManager = gameplayModule.transform.Find("InGameManager").gameObject.GetComponent<GameManager>();
        randomGenerator = gameplayModule.transform.Find("InGameManager").gameObject.GetComponent<RandomGenerator>();
        followList = gameplayModule.transform.Find("InGameManager").gameObject.GetComponent<FollowList>();

        inputReader = GameObject.FindGameObjectWithTag("MainComponents").transform.Find("[InputSystem]").gameObject.GetComponent<InputReader>();
        pauseManager = this.transform.Find("Menuing").gameObject.GetComponent<PauseManager>();*/

        cameraFreeLookController = gameplayModule.transform.Find("FreeLookController").gameObject.GetComponent<CameraFreeLookController>();
        cinemachineFreeLook = gameplayModule.transform.Find("CM FreeLook1").gameObject.GetComponent<CinemachineFreeLook>();
        mainCamera = gameplayModule.transform.Find("MainCamera").gameObject.GetComponent<Camera>();
        cinemachineBrain = mainCamera.gameObject.GetComponent<CinemachineBrain>();

        inGameUI = gameplayModule.transform.Find("UI").gameObject;
        optionsValues = FindObjectOfType<OptionsValues>();
    }
}
