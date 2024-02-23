///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 18/01/2023 11:15
///-----------------------------------------------------------------

using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace Com.WorldGame.ForeverInsurer.Localization {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class KeylessTextLocalizer : LocalizerBase
    {
        [SerializeField] private SerializedDictionary<E_Language, string> translations = default;

        protected override void Relocalize()
        {
            GetComponent<TextMeshProUGUI>().text = translations[LanguageManager.Instance.Localization];
        }
    }
}