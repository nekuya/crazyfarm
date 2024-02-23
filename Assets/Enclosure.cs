using UnityEngine;

namespace CrazyFarm
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Enclosure : Singleton<Enclosure>
    {
        public Bounds Bounds => collider.bounds;

        private new BoxCollider2D collider;

        protected override void Awake()
        {
            base.Awake();
            collider = GetComponent<BoxCollider2D>();
        }
    }
}