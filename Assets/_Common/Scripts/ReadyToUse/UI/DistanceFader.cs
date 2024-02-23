using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DistanceFader : MonoBehaviour
{
    [SerializeField] private bool useMainCamera = true;
    [SerializeField, HideIf(nameof(useMainCamera))] private Transform target;
    [SerializeField] private float visibilityDistance = 100f;
    [SerializeField] private float tweenDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private bool previousWithinDistance = false;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (useMainCamera)
            target = Camera.main.transform;
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget > visibilityDistance && !previousWithinDistance)
        {
            previousWithinDistance = true;
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, tweenDuration).OnComplete(() => gameObject.SetActive(false));
        }
        else if (previousWithinDistance)
        {
            previousWithinDistance = false;
            canvasGroup.DOKill();
            gameObject.SetActive(true);
            canvasGroup.DOFade(1f, tweenDuration);
        }
    }
}