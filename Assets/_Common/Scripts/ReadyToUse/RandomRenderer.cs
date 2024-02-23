using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class RandomRenderer : MonoBehaviour
{
    [SerializeField] private List<GameObject> renderers = default;

    private void OnEnable()
    {
        SetRandomRenderer();
    }

    [Button]
    private void SetRandomRenderer()
    {
        foreach (GameObject renderer in renderers)
            renderer.SetActive(false);

        renderers[Random.Range(0, renderers.Count)].SetActive(true);
    }
}
