using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ChangeString : MonoBehaviour
{
    public TMP_Text[] textBoxList;
    [TextArea(3, 3)]
    public LocalizeStringEvent[] stringList;
    public bool effectOnHover, refreshOnStart = true;

    void Start()
    {
        if (refreshOnStart)
        {
            if (stringList.Length > 0)
            {
                for (int i = 0; i < stringList.Length; i++)
                {
                    stringList[i].RefreshString();
                }
            }
        }
    }

    public void ShowString()
    {
        if (textBoxList.Length == stringList.Length)
        {
            for (int i = 0; i < textBoxList.Length; i++)
            {
                if (stringList[i] != null)
                {
                    int index = i;
                    stringList[i].OnUpdateString.AddListener((localizedString) =>
                    {
                        textBoxList[index].text = localizedString;
                    });

                    stringList[i].RefreshString();
                }
            }
        }
        else
        {
            Debug.LogError("Assurez-vous qu'il y autant de zones de texte que de descriptions");
        }
    }


    public void ShowHiddenString()
    {
        if (textBoxList.Length == stringList.Length)
        {
            for (int i = 0; i < textBoxList.Length; i++)
            {
                if (stringList[i] != null)
                {
                    textBoxList[i].text = "???";
                }
            }
        }
        else
        {
            Debug.LogError("Assurez-vous qu'il y autant de zones de texte que de descriptions");
        }
    }
}