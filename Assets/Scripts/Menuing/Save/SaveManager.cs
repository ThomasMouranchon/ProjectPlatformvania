using FIMSpace.GroundFitter;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using static SaveManager;

public class SaveManager : MonoBehaviour
{
    public enum zones
    {
        CentralCity,
        Desertis,
        Corailla,
        OlympeMount,
        DarkSea
    }

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
    public bool[] activatedTeleportations = new bool[100];
    [Space(10)]
    public float[] activatedTeleportationsPositionsX = new float[100];
    public float[] activatedTeleportationsPositionsY = new float[100];
    public float[] activatedTeleportationsPositionsZ = new float[100];
    [Space(10)]
    public float[] activatedTeleportationsRotationsX = new float[100];
    public float[] activatedTeleportationsRotationsY = new float[100];
    public float[] activatedTeleportationsRotationsZ = new float[100];
    public float[] activatedTeleportationsRotationsW = new float[100];
    [Space(10)]
    public string[] activatedTeleportationsZone = new string[100];
    [Space(30)]

    [Header("Events")]
    public int numberOfEvents;
    public bool[] activatedEvents = new bool[100];
    [Space(30)]

    [Header("Play time")]
    public int playTimeSeconds, playTimeMinutes, playTimeHours;

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

        activatedTeleportationsPositionsX = data.activatedTeleportationsPositionsX;
        activatedTeleportationsPositionsY = data.activatedTeleportationsPositionsY;
        activatedTeleportationsPositionsZ = data.activatedTeleportationsPositionsZ;

        activatedTeleportationsRotationsX = data.activatedTeleportationsRotationsX;
        activatedTeleportationsRotationsY = data.activatedTeleportationsRotationsY;
        activatedTeleportationsRotationsZ = data.activatedTeleportationsRotationsZ;
        activatedTeleportationsRotationsW = data.activatedTeleportationsRotationsW;

        activatedTeleportationsZone = data.activatedTeleportationsZone;

        if (data.activatedEvents.Length > 0) activatedEvents = data.activatedEvents;

        playTimeSeconds = data.playTimeSeconds;
        playTimeMinutes = data.playTimeMinutes;
        playTimeHours = data.playTimeHours;

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

        data.activatedTeleportationsPositionsX = activatedTeleportationsPositionsX;
        data.activatedTeleportationsPositionsY = activatedTeleportationsPositionsY;
        data.activatedTeleportationsPositionsZ = activatedTeleportationsPositionsZ;

        data.activatedTeleportationsRotationsX = activatedTeleportationsRotationsX;
        data.activatedTeleportationsRotationsY = activatedTeleportationsRotationsY;
        data.activatedTeleportationsRotationsZ = activatedTeleportationsRotationsZ;
        data.activatedTeleportationsRotationsW = activatedTeleportationsRotationsW;

        data.activatedTeleportationsZone = activatedTeleportationsZone;

        data.activatedEvents = activatedEvents;

        data.playTimeSeconds = playTimeSeconds;
        data.playTimeMinutes = playTimeMinutes;
        data.playTimeHours = playTimeHours;

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
    public bool[] activatedTeleportations = new bool[100];
    [Space(10)]
    public float[] activatedTeleportationsPositionsX = new float[100];
    public float[] activatedTeleportationsPositionsY = new float[100];
    public float[] activatedTeleportationsPositionsZ = new float[100];
    [Space(10)]
    public float[] activatedTeleportationsRotationsX = new float[100];
    public float[] activatedTeleportationsRotationsY = new float[100];
    public float[] activatedTeleportationsRotationsZ = new float[100];
    public float[] activatedTeleportationsRotationsW = new float[100];
    [Space(10)]
    public string[] activatedTeleportationsZone = new string[100];
    [Space(30)]

    [Header("Events")]
    public bool[] activatedEvents = new bool[100];
    [Space(30)]

    [Header("Play time")]
    public int playTimeSeconds, playTimeMinutes, playTimeHours;
}