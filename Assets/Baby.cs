using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CrazyFarm
{
    public class Baby : MonoBehaviour
    {
        [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 minMaxStepDistance;
        [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 minMaxDurationBtwSteps;
        [SerializeField] private float stepDuration = 0.5f;

        public float RandomStepDistance => Random.Range(minMaxStepDistance.x, minMaxStepDistance.y);
        public float RandomDurationBtwSteps => Random.Range(minMaxDurationBtwSteps.x, minMaxDurationBtwSteps.y);

        private void Start()
        {
            //DOTween.Sequence(this)
            //    .Append(transform.DOBlendableMoveBy(Vector3.one * RandomStepDistance, stepDuration, ));
        }
    }
}
