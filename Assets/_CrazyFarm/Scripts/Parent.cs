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
			//currentBabies = 
			targetBabies = Random.Range(minBabies, maxBabies);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			//s'abonne au OnRelease du bébé
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			//se désabonne au OnRelease du bébé
		}

		private void CheckRightBaby(Baby baby)
		{
			/*if (baby.animalsInfo.type == animalsInfo.type)
			{
				Debug.Log("It's the right baby");
				currentBabies.Add(baby);

				if (currentBabies.Count == targetBabies)
					Despawn();
			}
			else
			{
				Debug.Log("It's the wrong baby");
				RejectBaby();
			}*/
		}

		private void RejectBaby()
		{

		}

		private void Despawn()
		{
			foreach (Baby lBaby in currentBabies)
			{
				Destroy(lBaby.gameObject);
				Destroy(this);
			}
		}
	}
}
