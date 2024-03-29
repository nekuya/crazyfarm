using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace SheepFold
{
    public class ParentSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnCooldownBtwParents = 8f;
        [SerializeField] private Parent[] prefabs;
        [SerializeField] private Transform[] spots;

        private void Awake()
        {
            Spawn();
            DOVirtual.DelayedCall(spawnCooldownBtwParents, Spawn).SetLoops(-1);
        }

        private void Spawn()
        {
            Transform[] lUsableSpots = spots.Where(x => x.gameObject.activeSelf).ToArray();

            if (lUsableSpots.Length == 0)
                return;

            Transform lSelectedSpot = lUsableSpots[0];
            Parent lParent = Instantiate(prefabs[Random.Range(0, prefabs.Length)], lSelectedSpot.position, Quaternion.identity, null);
            lParent.OnQuit += (Parent sender) =>
            {
                lSelectedSpot.gameObject.SetActive(true);
            };

            lSelectedSpot.gameObject.SetActive(false);
        }
    }
}
