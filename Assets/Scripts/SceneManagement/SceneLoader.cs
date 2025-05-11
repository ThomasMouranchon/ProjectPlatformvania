using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.SceneStreamer;


public class SceneLoader : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public string defaultSceneName;

    private void Start()
    {
        string currentScene = defaultSceneName;
        if (SaveManager.Instance.lastTeleportPoint != 0)
        {
            currentScene = SaveManager.Instance.activatedTeleportationsZone[SaveManager.Instance.lastTeleportPoint].ToString();
        }
        SceneStreamer.SetCurrentScene(currentScene);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (asyncOperation == null || asyncOperation.isDone)
            {
                LoadScene();
            }
        }
    }

    public void LoadScene()
    {
        if (defaultSceneName != "")
        {
            string currentScene = defaultSceneName;
            if (SaveManager.Instance.lastTeleportPoint != 0)
            {
                currentScene = SaveManager.Instance.activatedTeleportationsZone[SaveManager.Instance.lastTeleportPoint].ToString();
            }
            asyncOperation = SceneManager.LoadSceneAsync(currentScene);
            //asyncOperation.allowSceneActivation = false;
        }
        else
        {
            Debug.LogError("La scène à charger n'est pas assignée !");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnloadScene();
        }
    }

    private void UnloadScene()
    {
        if (defaultSceneName != "")
        {
            SceneManager.UnloadSceneAsync(defaultSceneName);
            Debug.Log($"Scène {defaultSceneName} déchargée.");
        }
        else
        {
            Debug.LogError("La scène à décharger n'est pas assignée !");
        }
    }
}