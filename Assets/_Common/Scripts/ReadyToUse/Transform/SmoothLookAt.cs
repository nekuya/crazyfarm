using NaughtyAttributes;
using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    [SerializeField] private bool useMainCamera;
    [SerializeField, HideIf(nameof(useMainCamera))] private Transform target;
    [SerializeField] private float smoothLerp = 1f;

    private void Awake()
    {
        if (useMainCamera)
            target = Camera.main.transform;
    }

    void LateUpdate()
    {
        Quaternion lTargetRotation = target.rotation * Quaternion.AngleAxis(180f, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lTargetRotation, smoothLerp * Time.unscaledDeltaTime);
    }
}