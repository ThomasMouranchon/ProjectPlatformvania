using System;
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
    }

    public (float newTime, int newHours, int newMinutes, int newSeconds) GetCurrentPlayTime(float precedentTime)
    {
        float currentTime = Time.time + precedentTime;

        int hours = (int)(currentTime / 3600);
        int minutes = (int)((currentTime % 3600) / 60);
        int seconds = (int)(currentTime % 60);

        return (currentTime, hours, minutes, seconds);
    }
}