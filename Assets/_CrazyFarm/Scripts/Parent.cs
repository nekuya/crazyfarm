using CrazyFarm;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SheepFold
{
	public delegate void ParentEventHandler(Parent sender);
    public class Parent : Family, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI requiredBabiesTxt;
        [SerializeField] private int minBabies;
        [SerializeField] private int maxBabies;

		private int requiredBabies;
		private List<Baby> addedBabies = new List<Baby>();

		public event ParentEventHandler OnQuit;

		private void Start()
		{
			requiredBabies = UnityEngine.Random.Range(minBabies, maxBabies);
			requiredBabiesTxt.text = requiredBabies.ToString();
		}

		public void AddBaby(Baby baby)
		{
			baby.enabled = false;
            addedBabies.Add(baby);
			requiredBabies--;
			requiredBabiesTxt.text = requiredBabies.ToString();

			if (addedBabies.Count >= requiredBabies)
                Despawn();
        }

		private void Despawn()
		{
			OnQuit?.Invoke(this);

            for (int i = addedBabies.Count - 1; i >= 0; i--)
				Destroy(addedBabies[i].gameObject);

			Destroy(this);
		}

        public void OnPointerClick(PointerEventData eventData)
        {
			//PARENT ANIMAL SOUND
			transform.DOPunchScale(-Vector3.one * 0.2f, 0.2f)
				.OnComplete(AllChildCry);
        }

		private void AllChildCry()
		{
			foreach (Baby lBaby in FindObjectsOfType<Baby>())
			{
				if (lBaby.type == type)
					lBaby.Cry();
			}
        }
    }
}
