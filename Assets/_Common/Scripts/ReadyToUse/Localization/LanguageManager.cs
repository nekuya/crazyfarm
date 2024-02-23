using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 18/01/2023 10:41
/// -----------------------------------------------------------------
public delegate void LanguageManagerEventHandler();
public class LanguageManager : Singleton<LanguageManager>
{
    private const string FILE_PATH = "Localization/";
    private const string DEFAULT_KEY = "DEFAULT";

    private Dictionary<E_Language, Dictionary<string, string>> locaDictionary;

    public E_Language Localization { get; private set; } = E_Language.FRENCH;

    public event LanguageManagerEventHandler OnUpdate;

    protected override void Awake()
    {
        base.Awake();
        InitLocaDictionary();
        ResetLocalization();
    }

    public void ResetLocalization()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                SetLocalization(E_Language.ENGLISH);
                break;
            case SystemLanguage.French:
                SetLocalization(E_Language.FRENCH);
                break;
            default:
                SetLocalization(E_Language.ENGLISH);
                break;
        }
    }

    public void SetLocalization(E_Language localization)
    {
        Localization = localization;
        OnUpdate?.Invoke();
    }

    public string GetLocalizedText(string key)
    {
        Dictionary<string, string> lDictionary = locaDictionary[Localization];

        if (key != null && lDictionary.ContainsKey(key))
            return lDictionary[key];
        else
            return lDictionary[DEFAULT_KEY];
    }

    private void InitLocaDictionary()
    {
        locaDictionary = new Dictionary<E_Language, Dictionary<string, string>>();

        foreach (E_Language language in (E_Language[])Enum.GetValues(typeof(E_Language)))
            locaDictionary[language] = LoadJsonLocalizationFile(FILE_PATH, language);
    }

    private Dictionary<string, string> LoadJsonLocalizationFile(string path, E_Language lang)
	{
        Dictionary<string, string> lResult = new Dictionary<string, string>();
		string lFilePath = $"{path}{lang}";
		TextAsset lJsonFile = Resources.Load<TextAsset>(lFilePath);

        if (lJsonFile == null)
        {
            Debug.LogWarning($"No JSON Found. Path: {lFilePath}");
            return null;
        }
        else
            JsonConvert.PopulateObject(lJsonFile.text, lResult);
        
        return lResult;
	}
}