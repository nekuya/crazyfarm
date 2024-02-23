using DG.Tweening;
using UnityEngine.Rendering.Universal;

public static class DecalProjectorExtensions
{
    public static Tweener DOFade(this DecalProjector decalProjector, float endValue, float duration)
    {
        return DOVirtual.Float(decalProjector.fadeFactor, endValue, duration, (float value) =>
        {
            decalProjector.fadeFactor = value;
        });
    }
}
