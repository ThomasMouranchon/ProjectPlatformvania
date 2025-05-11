using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class PlayTimeHandler : MonoBehaviour
{
    private static PlayTimeHandler instance = null;
    public static PlayTimeHandler Instance => instance;

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
            DontDestroyOnLoad(gameObject);
        }

        StartCoroutine(PlayTimeFollower());
    }

    private IEnumerator PlayTimeFollower()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            SaveManager.Instance.playTimeSeconds++;
            AdjustPlayTime();
        }
    }

    public void AdjustPlayTime()
    {
        SaveManager saveManager = SaveManager.Instance;
        if (saveManager.playTimeSeconds == 60)
        {
            saveManager.playTimeSeconds = 0;
            saveManager.playTimeMinutes++;

            if (saveManager.playTimeMinutes == 60)
            {
                saveManager.playTimeMinutes = 0;
                if (saveManager.playTimeHours < 99) saveManager.playTimeHours++;
            }
        }
    }

    public (float newTime, int newHours, int newMinutes, int newSeconds) GetCurrentPlayTime(float precedentTime)
    {
        float newTime = Time.time + precedentTime;

        int hours = (int)(newTime / 3600);
        int minutes = (int)((newTime % 3600) / 60);
        int seconds = (int)(newTime % 60);

        return (newTime, hours, minutes, seconds);
    }
}