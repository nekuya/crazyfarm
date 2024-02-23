using DG.Tweening;
using NaughtyAttributes;
using SheepFold;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrazyFarm
{
    public delegate void BabyEventHandler(Baby baby);
    public class Baby : Family, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 minMaxStepDistance;
        [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 minMaxDurationBtwSteps;
        [SerializeField] private float stepDuration = 0.5f;

        private Parent inParent;

        public float RandomStepDistance => Random.Range(minMaxStepDistance.x, minMaxStepDistance.y);
        public float RandomDurationBtwSteps => Random.Range(minMaxDurationBtwSteps.x, minMaxDurationBtwSteps.y);

        public void StartStep()
        {
            Vector3 lEndPosition = transform.position +
                Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right * RandomStepDistance;
            lEndPosition = Enclosure.Instance.Bounds.ClosestPoint(lEndPosition);

            DOTween.Sequence(this)
                .Append(transform.DOMove(lEndPosition, stepDuration))
                .AppendInterval(RandomDurationBtwSteps)
                .AppendCallback(StartStep);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DOTween.Kill(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Camera.main.ScreenToWorldPoint(
                new (eventData.position.x, eventData.position.y, transform.position.z - Camera.main.transform.position.z));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (inParent != null)
            {
                if (type == inParent.type)
                {
                    Debug.Log("It's the right baby");
                    inParent.AddBaby(this);
                }
                else
                {
                    Debug.Log("It's the wrong baby");
                    PutToCenter();
                }
            }
            else
                PutToCenter();
        }

        private void PutToCenter()
        {
            //Call this function when it can't be dropped and you have to put it back to its original position 
            transform.DOMove(Enclosure.Instance.Bounds.center, 0.5f).OnComplete(() =>
            {
                StartStep();
            });
        }

        public void Cry()
        {
            //BABY ANIMAL SOUND
            transform.DOPunchScale(-Vector3.one * 0.2f, 0.25f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Parent lParent))
                inParent = lParent;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Parent lParent))
                inParent = null;
        }
    }
}
