///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 18/01/2023 11:06
///-----------------------------------------------------------------

using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private E_Language language = default;

    public void Select()
    {
        LanguageManager.Instance.SetLocalization(language);
    }

    public void ResetLanguage()
    {
        LanguageManager.Instance.ResetLocalization();
    }
}