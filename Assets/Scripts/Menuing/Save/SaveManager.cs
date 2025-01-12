using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance = null;
    public static SaveManager Instance => instance;

    public int currentSaveFile;

    [Header("Elements to save")]
    public bool startedGame;
    [Header("Items")]
    public bool enableDash, enableGlide, enableWallJump, enableYoyo, enableSpeedBoost, enableBomb, enablePhantomDash;
    [Space(30)]

    [Header("Story")]
    public int storyState;
    [Space(10)]
    public bool[] finishedDungeons;
    [Space(30)]

    [Header("Teleports")]
    public int lastTeleportPoint;
    [Space(10)]
    public bool[] activatedTeleportations;
    [Space(30)]

    [Header("Events")]
    public int numberOfEvents;
    public bool[] activatedEvents;

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

    public void Load(int selectedSaveFile)
    {
        currentSaveFile = selectedSaveFile;

        string filePath = Application.persistentDataPath + "/playerInfo" + selectedSaveFile.ToString() + ".dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        PlayerData_Storage data;

        if (File.Exists(filePath))
        {
            file = File.Open(filePath, FileMode.Open);
            data = (PlayerData_Storage)bf.Deserialize(file);
        }
        else
        {
            file = File.Create(filePath);

            data = new PlayerData_Storage();
            bf.Serialize(file, data);
        }

        startedGame = data.startedGame;

        enableDash = data.enableDash;
        enableGlide = data.enableGlide;
        enableWallJump = data.enableWallJump;
        enableYoyo = data.enableYoyo;
        enableSpeedBoost = data.enableSpeedBoost;
        enableBomb = data.enableBomb;
        enablePhantomDash = data.enablePhantomDash;

        storyState = data.storyState;
        finishedDungeons = data.finishedDungeons;

        lastTeleportPoint = data.lastTeleportPoint;

        activatedTeleportations = data.activatedTeleportations;

        if (data.activatedEvents.Length > 0) activatedEvents = data.activatedEvents;

        file.Close();
    }

    public void Save(int selectedSaveFile)
    {
        currentSaveFile = selectedSaveFile;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo" + selectedSaveFile.ToString() + ".dat");
        PlayerData_Storage data = new PlayerData_Storage();

        data.startedGame = startedGame;

        data.enableDash = enableDash;
        data.enableGlide = enableGlide;
        data.enableWallJump = enableWallJump;
        data.enableYoyo = enableYoyo;
        data.enableSpeedBoost = enableSpeedBoost;
        data.enableBomb = enableBomb;
        data.enablePhantomDash = enablePhantomDash;

        data.storyState = storyState;
        data.finishedDungeons = finishedDungeons;

        data.lastTeleportPoint = lastTeleportPoint;

        data.activatedTeleportations = activatedTeleportations;

        data.activatedEvents = activatedEvents;

        data.playTimeTuple = PlayTimeHandler.Instance.GetCurrentPlayTime(data.playTimeTuple.currentPlayTime);

        bf.Serialize(file, data);
        file.Close();
    }
    public bool Delete(int selectedSaveFile)
    {
        string filePath = Application.persistentDataPath + "/playerInfo" + selectedSaveFile.ToString() + ".dat";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        else
        {
            return false;
        }
    }
}

[Serializable]
class PlayerData_Storage
{
    public bool startedGame;
    [Header("Items")]
    public bool enableDash, enableGlide, enableWallJump, enableYoyo, enableSpeedBoost, enableBomb, enablePhantomDash;
    [Space(30)]

    [Header("Story")]
    public int storyState;
    [Space(10)]
    public bool[] finishedDungeons;
    [Space(30)]

    [Header("Teleports")]
    public int lastTeleportPoint;
    [Space(10)]
    public bool[] activatedTeleportations;
    [Space(30)]

    [Header("Events")]
    public bool[] activatedEvents = new bool[20];
    [Space(30)]

    [Header("Play time")]
    public (float currentPlayTime, int currentHours, int currentMinutes, int currentSeconds) playTimeTuple;
}