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
            StartStep();
        }

        private void StartStep()
        {
            Vector3 lEndPosition = transform.position +
                Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right * RandomStepDistance;
            lEndPosition = Enclosure.Instance.Bounds.ClosestPoint(lEndPosition);

            //KILL WHEN DRAGGED, RESTART WHEN DROPPED
            DOTween.Sequence(this)
                .Append(transform.DOMove(lEndPosition, stepDuration))
                .AppendInterval(RandomDurationBtwSteps)
                .AppendCallback(StartStep);
        }
    }
}
