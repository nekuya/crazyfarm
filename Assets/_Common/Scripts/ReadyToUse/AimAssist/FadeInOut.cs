using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float fadeOutDuration = 0.25f;

    private CanvasGroup canvasGroup;
    private Tween canvasTween;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Play()
    {
        canvasTween.Kill(true);
        canvasTween = DOVirtual.Float(0f, 1f, fadeInDuration,
        (float value) =>
        {
            canvasGroup.alpha = value;
        });

        canvasTween.OnComplete(
        () =>
        {
            DOVirtual.Float(1f, 0f, fadeOutDuration,
                (float value) =>
                {
                    canvasGroup.alpha = value;
                });
        });
    }
}