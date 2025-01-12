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
            Debug.LogError("La sc�ne � charger n'est pas assign�e !");
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
            Debug.Log($"Sc�ne {sceneName} d�charg�e.");
        }
        else
        {
            Debug.LogError("La sc�ne � d�charger n'est pas assign�e !");
        }
    }
}