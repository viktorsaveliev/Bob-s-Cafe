using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageChanger
{
    private readonly MonoBehaviour _monoBehaviour;
    private bool _isActive;

    public LanguageChanger(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void NextLanguage()
    {
        if (_isActive) return;

        int maxLanguage = 2;

        StringBus stringBus = new();
        int currentLanguage = PlayerPrefs.GetInt(stringBus.Language);

        if (++currentLanguage >= maxLanguage)
        {
            currentLanguage = 0;
        }

        _monoBehaviour.StartCoroutine(SetLanguage(currentLanguage));
    }

    private IEnumerator SetLanguage(int langID)
    {
        _isActive = true;

        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[langID]; 

        StringBus stringBus = new();
        PlayerPrefs.SetInt(stringBus.Language, langID);
        PlayerPrefs.Save();

        _isActive = false;
    }
}
