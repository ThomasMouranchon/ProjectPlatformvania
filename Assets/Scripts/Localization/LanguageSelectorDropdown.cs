using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageSelectorDropdown : MonoBehaviour
{
    private static LanguageSelectorDropdown instance = null;
    public static LanguageSelectorDropdown Instance => instance;

    public TMP_Dropdown languageDropdown;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
}
