using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public string sceneName;

    void OnTriggerEnter(Collider other)
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
        if (sceneName != "")
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
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
        if (sceneName != "")
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log($"Scène {sceneName} déchargée.");
        }
        else
        {
            Debug.LogError("La scène à décharger n'est pas assignée !");
        }
    }
}