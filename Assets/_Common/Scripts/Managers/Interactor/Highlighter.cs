using DG.Tweening;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] private Transform signTransform;
    [SerializeField] private float signDuration = 0.5f;
    [SerializeField] private float deltaScale = 0.2f;

    public bool IsOn
    {
        get => _isOn;

        set
        {
            if (!enabled || _isOn == value) return;

            _isOn = value;
            Vector3 lDeltaScale = Vector3.one * deltaScale * (_isOn ? 1f : -1f);
            signTransform.DOBlendableScaleBy(lDeltaScale, signDuration);
        }
    }
    private bool _isOn = false;
}