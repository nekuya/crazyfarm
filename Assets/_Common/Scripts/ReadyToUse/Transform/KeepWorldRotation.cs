using UnityEngine;

public class KeepWorldRotation : MonoBehaviour
{
    private Quaternion initRotation;

    private void Start()
    {
        initRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = initRotation;
    }
}