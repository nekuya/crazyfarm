using UnityEngine;

public class PassiveConstantMove : MonoBehaviour
{
    [SerializeField] private Vector3 movement;

    protected virtual void Update()
    {
        transform.localPosition += movement * Time.unscaledDeltaTime;
    }
}