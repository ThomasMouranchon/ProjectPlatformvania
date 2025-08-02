using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private static LocaleSelector instance = null;
    public static LocaleSelector Instance => instance;

    [SerializeField] private int defaultLanguage = 0;
    private bool active = false;

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
            int ID = PlayerPrefs.GetInt("language", defaultLanguage);
            ChangeLocale(ID);
        }
    }

    public void ChangeLocale(int localeID)
    {
        if (active) return;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("language", localeID);

        if (LanguageSelectorDropdown.Instance) LanguageSelectorDropdown.Instance.languageDropdown.SetValueWithoutNotify(localeID);

        active = false;
    }
}
