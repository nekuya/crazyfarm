using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CopyFov : MonoBehaviour
{
    [SerializeField] private Camera sourceCamera;

    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (camera.fieldOfView != sourceCamera.fieldOfView)
            camera.fieldOfView = sourceCamera.fieldOfView;
    }
}