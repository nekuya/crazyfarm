using CrazyFarm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SheepFold
{
    public class Parent : MonoBehaviour
    {
		[SerializeField] private AnimalType type;
        [SerializeField] private int minBabies;
        [SerializeField] private int maxBabies;

		private List<Baby> currentBabies;
		private int targetBabies;

		private void Start()
		{
			currentBabies = new List<Baby>();
			targetBabies = Random.Range(minBabies, maxBabies);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			//s'abonne au OnRelease du bébé
			/*if (collision.gameObject.TryGetComponent<Baby>(out Baby lBaby)
				lBaby.*/
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			//se désabonne au OnRelease du bébé
			/*if (collision.gameObject.TryGetComponent<Baby>(out Baby lBaby)
				lBaby.*/
		}

		private void CheckRightBaby(Baby baby)
		{
			if (baby.type == type)
			{
				Debug.Log("It's the right baby");
				currentBabies.Add(baby);

				if (currentBabies.Count == targetBabies)
					Despawn();
			}
			else
			{
				Debug.Log("It's the wrong baby");
				RejectBaby(baby);
			}
		}

		private void RejectBaby(Baby baby)
		{
			baby.gameObject.transform.position = Enclosure.Instance.Bounds.RandomPoint();
		}

		private void Despawn()
		{
			for (int i = currentBabies.Count - 1; i >= 0; i--)
				Destroy(currentBabies[i].gameObject);

			Destroy(this);
		}
	}
}
