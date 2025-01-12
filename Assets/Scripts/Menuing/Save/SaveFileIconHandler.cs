using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileIconHandler : MonoBehaviour
{
    public int saveFileValue = 1;
    public bool checksCurrentElements = false;
    [Space(30)]

    public GameObject noSaveFileBox, hasSaveFileBox;
    public TMP_Text gameTimeText;/*
    public Image[] enableItemsImages;
    public Image[] finishedDungeonsImages;
    public Image finishedGameStar;*/

    private SaveManager saveManager;

    void Start()
    {
        saveManager = SaveManager.Instance;
        if (checksCurrentElements) LoadCurrentElements();
        else LoadElements();
    }

    public void LoadElements()
    {
        string filePath = Application.persistentDataPath + "/playerInfo" + saveFileValue.ToString() + ".dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        PlayerData_Storage data;

        if (File.Exists(filePath))
        {
            file = File.Open(filePath, FileMode.Open);
            data = (PlayerData_Storage)bf.Deserialize(file);
            
            ShowSaveFile(data.startedGame);

            string formattedHours = data.playTimeTuple.currentHours.ToString();
            string formattedMins = data.playTimeTuple.currentMinutes.ToString();

            if (formattedHours.Length == 1) formattedHours = formattedHours.PadLeft(2, '0');
            if (formattedMins.Length == 1) formattedMins = formattedMins.PadLeft(2, '0');

            gameTimeText.text = formattedHours + " : " + formattedMins;
            /*
            enableItemsImages[0].enabled = data.enableGlide;
            enableItemsImages[1].enabled = data.enableDash;
            enableItemsImages[2].enabled = data.enableWallJump;
            enableItemsImages[3].enabled = data.enableYoyo;
            enableItemsImages[4].enabled = data.enableSpeedBoost;
            enableItemsImages[5].enabled = data.enableBomb;

            for (int i = 0; i < finishedDungeonsImages.Length; i++)
            {
                finishedDungeonsImages[i].enabled = data.finishedDungeons[i];
            }
            */
            file.Close();
        }
        else
        {
            ShowSaveFile(false);
        }
    }

    public void LoadCurrentElements()
    {
        saveManager = SaveManager.Instance;
        string filePath = Application.persistentDataPath + "/playerInfo" + saveFileValue.ToString() + ".dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        PlayerData_Storage data;

        if (File.Exists(filePath))
        {
            file = File.Open(filePath, FileMode.Open);
            data = (PlayerData_Storage)bf.Deserialize(file);

            ShowSaveFile(saveManager.startedGame);

            (float currentPlayTime, int currentHours, int currentMinutes, int currentSeconds) playTimeTuple = PlayTimeHandler.Instance.GetCurrentPlayTime(data.playTimeTuple.currentPlayTime);

            string formattedHours = playTimeTuple.currentHours.ToString();
            string formattedMins = playTimeTuple.currentMinutes.ToString();

            if (formattedHours.Length == 1) formattedHours = formattedHours.PadLeft(2, '0');
            if (formattedMins.Length == 1) formattedMins = formattedMins.PadLeft(2, '0');

            gameTimeText.text = formattedHours + " : " + formattedMins;
            /*
            enableItemsImages[0].enabled = saveManager.enableGlide;
            enableItemsImages[1].enabled = saveManager.enableDash;
            enableItemsImages[2].enabled = saveManager.enableWallJump;
            enableItemsImages[3].enabled = saveManager.enableYoyo;
            enableItemsImages[4].enabled = saveManager.enableSpeedBoost;
            enableItemsImages[5].enabled = saveManager.enableBomb;

            for (int i = 0; i < finishedDungeonsImages.Length; i++)
            {
                finishedDungeonsImages[i].enabled = saveManager.finishedDungeons[i];
            }
            */
            file.Close();
        }
        else
        {
            ShowSaveFile(false);
        }
    }

    private void ShowSaveFile(bool hasFile)
    {
        noSaveFileBox.SetActive(!hasFile);
        hasSaveFileBox.SetActive(hasFile);
    }
}
