using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLocalizer : LocalizerBase
{
    [SerializeField] private SerializedDictionary<E_Language, GameObject> gameObjects = default;

    protected override void Relocalize()
    {
        foreach (KeyValuePair<E_Language, GameObject> lPair in gameObjects)
            lPair.Value.SetActive(LanguageManager.Instance.Localization == lPair.Key);
    }
}