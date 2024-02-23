using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[Serializable]
public class LocalizedValue<TValue>
{
    [SerializeField] private SerializedDictionary<E_Language, TValue> values;

    protected TValue Value => values[LanguageManager.Instance.Localization];

    public static implicit operator TValue(LocalizedValue<TValue> localizedValue) => localizedValue.Value;
}