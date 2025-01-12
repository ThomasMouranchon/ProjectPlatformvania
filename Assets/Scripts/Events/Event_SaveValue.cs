using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Event_SaveValue : MonoBehaviour
{
    public enum VariableType
    {
        Integer,
        String,
        Boolean
    }

    public VariableType variableType;
    [Space(30)]

    public string elementToSave;
    [Space(30)]

    public int intToSave;
    public string stringToSave;
    public bool boolToSave;
    private SaveManager saveManager;

    public void SaveValue()
    {
        saveManager = SaveManager.Instance;
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo" + SaveManager.Instance.currentSaveFile.ToString() + ".dat");
        PlayerData_Storage data = new PlayerData_Storage();*/

        FieldInfo field = typeof(SaveManager).GetField(elementToSave);

        switch (variableType)
        {
            default:
                field.SetValue(saveManager, intToSave);
                break;
            case VariableType.String:
                field.SetValue(saveManager, stringToSave);
                break;
            case VariableType.Boolean:
                field.SetValue(saveManager, boolToSave);
                break;
        }
        /*
        bf.Serialize(file, data);
        file.Close();*/
    }
}