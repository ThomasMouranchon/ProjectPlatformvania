using PixelCrushers.SceneStreamer;
using SingularityGroup.HotReload;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private static GameLoader instance = null;
    public static GameLoader Instance => instance;

    public GameObject starTransition, starTransitionInverted, playerTransition;
    private Animator starTransitionAnim, starTransitionInvertedAnim, playerTransitionAnim;
    [Space(10)]

    public GameObject loadingOverlay;
    public Animator loadingPartAnim;
    public float transitionTime = 1;
    private int randomValue;
    [Space(10)]

    public OptionsValues optionsValues;

    private void Awake()
    {
        GameObject[] otherInstances = GameObject.FindGameObjectsWithTag("LevelLoader");
        if (otherInstances.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        starTransitionAnim = starTransition.GetComponent<Animator>();
        starTransitionInvertedAnim = starTransitionInverted.GetComponent<Animator>();
        playerTransitionAnim = playerTransition.GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PlayAnimationAfterDelay());
    }

    IEnumerator PlayAnimationAfterDelay()
    {
        starTransitionInvertedAnim.CrossFade("StarTransitionInverted_Loading", 0);
        loadingOverlay.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        starTransitionInvertedAnim.CrossFade("StarTransitionInverted_End1", 0);
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine());
    }
    IEnumerator QuitGameCoroutine()
    {
        while (!starTransitionAnim.GetCurrentAnimatorStateInfo(0).IsName("StarTransition_Idle"))
        {
            yield return null;
        }

        starTransitionInvertedAnim.CrossFade("StarTransitionInverted_Start1", 0);
        optionsValues.SavePlayerOptions();
        yield return new WaitForSecondsRealtime(1);
        Application.Quit();
    }

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LoadLevelCoroutine(levelToLoad));
    }

    IEnumerator LoadLevelCoroutine(string levelToLoad)
    {
        while (!starTransitionAnim.GetCurrentAnimatorStateInfo(0).IsName("StarTransition_Idle"))
        {
            yield return null;
        }

        randomValue = Random.Range(0, 4);

        //starTransition.SetActive(true);
        
        switch (randomValue)
        {
            default:
                starTransitionAnim.CrossFade("StarTransition_Start1", 0);
                break;
            case 1:
                starTransitionAnim.CrossFade("StarTransition_Start2", 0);
                break;
            case 2:
                starTransitionAnim.CrossFade("StarTransition_Start3", 0);
                break;
            case 3:
                starTransitionAnim.CrossFade("StarTransition_Start4", 0);
                break;
        }

        if (Time.timeScale == 0) yield return new WaitForSecondsRealtime(transitionTime);
        else yield return new WaitForSeconds(transitionTime);

        //SceneManager.LoadSceneAsync(levelToLoad);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
        
        if (levelToLoad != "MainMenu")
        {
            loadingOverlay.SetActive(true);
            loadingPartAnim.CrossFade("LoadingIcon_Start", 0);
        }

        if (Time.timeScale == 0) yield return new WaitForSecondsRealtime(0.1f);
        else yield return new WaitForSeconds(0.1f);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (levelToLoad != "MainMenu")
        {
            GameObject.Find("MainGameComponents").SetActive(true);
        }

        optionsValues.OnSceneChange();

        if (levelToLoad != "MainMenu")
        {
            loadingPartAnim.CrossFade("LoadingIcon_End", 0);
            GameObject.Find("MainGameComponents").SetActive(true);

            if (levelToLoad == "StartScene")
            {
                SceneStreamer.SetCurrentScene("Corailla");
            }
        }
        /*else
        {*/
            switch (randomValue)
            {
                default:
                    starTransitionAnim.CrossFade("StarTransition_End1", 0);
                    break;
                case 1:
                    starTransitionAnim.CrossFade("StarTransition_End2", 0);
                    break;
                case 2:
                    starTransitionAnim.CrossFade("StarTransition_End3", 0);
                    break;
                case 3:
                    starTransitionAnim.CrossFade("StarTransition_End4", 0);
                    break;
            }
        //}

        while (!starTransitionAnim.GetCurrentAnimatorStateInfo(0).IsName("StarTransition_Idle"))
        {
            yield return null;
        }

        //starTransition.SetActive(false);
        loadingOverlay.SetActive(false);
    }

    public void LoadFromDeath(float timer)
    {
        StartCoroutine(LoadFromDeathCoroutine(timer));
    }

    IEnumerator LoadFromDeathCoroutine(float timer)
    {
        randomValue = Random.Range(0, 3);

        //starTransitionInverted.SetActive(true);

        switch (randomValue)
        {
            default:
                starTransitionInvertedAnim.CrossFade("StarTransitionInverted_Start1", 0);
                break;
            case 1:
                starTransitionInvertedAnim.CrossFade("StarTransitionInverted_Start2", 0);
                break;
            case 2:
                starTransitionInvertedAnim.CrossFade("StarTransitionInverted_Start3", 0);
                break;
        }

        yield return new WaitForSeconds(timer);

        switch (randomValue)
        {
            default:
                starTransitionInvertedAnim.CrossFade("StarTransitionInverted_End1", 0);
                break;
            case 1:
                starTransitionInvertedAnim.CrossFade("StarTransitionInverted_End2", 0);
                break;
            case 2:
                starTransitionInvertedAnim.CrossFade("StarTransitionInverted_End3", 0);
                break;
        }

        while (!starTransitionInvertedAnim.GetCurrentAnimatorStateInfo(0).IsName("StarTransitionInverted_Idle"))
        {
            yield return null;
        }

        //starTransitionInverted.SetActive(false);
    }
}
