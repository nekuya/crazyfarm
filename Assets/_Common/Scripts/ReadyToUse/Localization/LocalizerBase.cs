using UnityEngine;

public abstract class LocalizerBase : MonoBehaviour
{
    private void Start()
    {
        LanguageManager.Instance.OnUpdate += Relocalize;
        Relocalize();
    }

    protected abstract void Relocalize();

    private void OnDestroy()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnUpdate -= Relocalize;
    }
}