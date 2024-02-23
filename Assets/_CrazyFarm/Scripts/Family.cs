using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrazyFarm
{
    public class Family : MonoBehaviour
    {
        [field: SerializeField] public AnimalType type { get; private set; } = default;
        [SerializeField] protected AudioClip sound = default;
        [SerializeField] protected AudioSource audioSource = default;
    }
}
