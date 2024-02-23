///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 19/01/2023 16:28
///-----------------------------------------------------------------

using System;
using UnityEngine;

public class LanguageCarousel : MonoBehaviour
{
    public void ToNext()
    {
        LanguageManager.Instance.SetLocalization(GetNextLanguage());
    }

    public void ToPrevious()
    {
        LanguageManager.Instance.SetLocalization(GetPreviousLanguage());
    }

    private E_Language GetNextLanguage()
    {
        E_Language[] lArray = (E_Language[])Enum.GetValues(typeof(E_Language));
        int lNextIndex = Array.IndexOf(lArray, LanguageManager.Instance.Localization) + 1;
        return (lArray.Length <= lNextIndex) ? lArray[0] : lArray[lNextIndex];
    }

    private E_Language GetPreviousLanguage()
    {
        E_Language[] lArray = (E_Language[])Enum.GetValues(typeof(E_Language));
        int lNextIndex = Array.IndexOf(lArray, LanguageManager.Instance.Localization) - 1;
        return (lNextIndex < 0) ? lArray[lArray.Length - 1] : lArray[lNextIndex];
    }
}