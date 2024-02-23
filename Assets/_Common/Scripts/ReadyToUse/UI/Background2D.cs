///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 04/06/2022 12:05
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.GabrielBernabeu.Common {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Background2D : MonoBehaviour
    {
        [SerializeField] private Rect addedRect = default;

        protected virtual void Start()
        {
            ScreenLimit lScreenLimit = ScreenLimit.Instance;
            SpriteRenderer lSpriteRenderer = GetComponent<SpriteRenderer>();

            if (lSpriteRenderer.drawMode != SpriteDrawMode.Tiled)
            {
                Debug.LogError("Background's spriteRenderer must be tiled!");
                return;
            }

            Vector2 lNewSize = lScreenLimit.Rect.size;
            lNewSize.x += addedRect.width;
            lNewSize.y += addedRect.height;
            Vector3 lNewPosition = lScreenLimit.Camera.transform.position;
            lNewPosition.x += addedRect.x;
            lNewPosition.y += addedRect.y;
            lNewPosition.z = transform.position.z;

            GetComponent<SpriteRenderer>().size = lNewSize;
            transform.position = lNewPosition;
        }
    }
}
