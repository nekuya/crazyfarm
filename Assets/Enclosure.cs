using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CrazyFarm
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Enclosure : Singleton<Enclosure>
    {
        public Bounds Bounds => collider.bounds;
        [SerializeField] private int maxCapacity = 10;
        [SerializeField] private float delayTime = 5f;

        [SerializeField] private List<Baby> babyTypes = new List<Baby>();

        private List<Baby> babies = new List<Baby>();
        private new BoxCollider2D collider;
        private float elapsedTime;
        private int currentBabyType = 0;

        private void Start()
        {
            elapsedTime = delayTime;
        }

        public int CurrentCapacity { get 
            {
                int current = 0;
                for (int i = 0; i < babies.Count; i++)
                {
                    if (babies[i].enabled)
                        current++;
                }

                return current;
            }
        }
        public bool IsMaximumCapacity { get { return CurrentCapacity >= maxCapacity; } }

        private void Update()
        {
            if (!IsMaximumCapacity)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= delayTime)
                {
                    CreateBaby();
                    elapsedTime = 0;      
                }
            }
        }

        public Baby CreateBaby()
        {
            Baby baby = Instantiate(babyTypes[currentBabyType]);
            currentBabyType++;
            if (currentBabyType > babyTypes.Count - 1)
                currentBabyType = 0;

            babies.Add(baby);
            baby.transform.position = transform.position;
            baby.transform.localScale = Vector3.zero;
            baby.transform.DOScale(Vector3.one * 0.2f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
           {
               baby.StartStep();
           });
            return baby;
        }

        protected override void Awake()
        {
            base.Awake();
            collider = GetComponent<BoxCollider2D>();
        }
    }
}