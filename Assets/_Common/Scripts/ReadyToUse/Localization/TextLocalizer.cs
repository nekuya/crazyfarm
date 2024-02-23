///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 18/01/2023 11:15
///-----------------------------------------------------------------

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizer : LocalizerBase
{
    [SerializeField] private string key = default;

    protected override void Relocalize()
    {
        GetComponent<TextMeshProUGUI>().text = LanguageManager.Instance.GetLocalizedText(key);
    }
}