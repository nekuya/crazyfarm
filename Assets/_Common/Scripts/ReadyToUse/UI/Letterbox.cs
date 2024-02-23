using DG.Tweening;
using UnityEngine;

public class Letterbox : MonoBehaviour
{
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private float tweenDuration = 1f;

    private void Awake()
    {
        top.localScale = bottom.localScale = new Vector3(1f, 0f, 1f);
    }

    public void Show()
    {
        top.DOKill();
        bottom.DOKill();

        top.DOScaleY(1f, tweenDuration);
        bottom.DOScaleY(1f, tweenDuration);
    }

    public void Hide()
    {
        top.DOKill();
        bottom.DOKill();

        top.DOScaleY(0f, tweenDuration);
        bottom.DOScaleY(0f, tweenDuration);
    }
}