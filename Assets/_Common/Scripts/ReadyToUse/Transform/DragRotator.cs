///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 04/01/2023 09:19
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.WorldGame.ForeverInsurer.Tools {
    /// <summary>
    /// Manages the card rotation when dragged (OVERRIDES ITS ROTATION EACH FRAME. For rotating the card (ex: flip), touch to the
    /// "Renderer" game object.
    /// </summary>
    public class DragRotator : MonoBehaviour
    {
        [SerializeField] private DragRotatorInfo infos;

        private float pitchAngle;
        private float rollAngle;
        private float pitchVelocity;
        private float rollVelocity;
        private Vector3 previousPos;
        private Vector3 initAngles;

        private void Awake()
        {
            Reset();
            previousPos = transform.position;
            initAngles = transform.localRotation.eulerAngles;
        }

        private void Update()
        {
            Vector3 position = transform.position;
            Vector3 vector3 = position - previousPos;

            if (vector3.sqrMagnitude > 9.99999974737875E-05)
            {
                pitchAngle += vector3.y * infos.pitchInfo.forceMultiplier;
                pitchAngle = Mathf.Clamp(pitchAngle, infos.pitchInfo.minDegrees, infos.pitchInfo.maxDegrees);
                rollAngle -= vector3.x * infos.rollInfo.forceMultiplier;
                rollAngle = Mathf.Clamp(rollAngle, infos.rollInfo.minDegrees, infos.rollInfo.maxDegrees);
            }

            pitchAngle = Mathf.SmoothDamp(pitchAngle, 0.0f, ref pitchVelocity, infos.pitchInfo.restSeconds * 0.1f);
            rollAngle = Mathf.SmoothDamp(rollAngle, 0.0f, ref rollVelocity, infos.rollInfo.restSeconds * 0.1f);
            transform.localRotation = Quaternion.Euler(initAngles.x + pitchAngle, initAngles.y + rollAngle, initAngles.z);
            previousPos = position;
        }

        public void SetInfo(DragRotatorInfo info)
        {
            infos = info;
        }

        public void Reset()
        {
            previousPos = transform.position;
            transform.localRotation = Quaternion.Euler(initAngles);
            rollAngle = 0.0f;
            rollVelocity = 0.0f;
            pitchAngle = 0.0f;
            pitchVelocity = 0.0f;
        }

        [Serializable]
        public class DragRotatorInfo
        {
            public DragRotatorAxisInfo pitchInfo;
            public DragRotatorAxisInfo rollInfo;

            [Serializable]
            public class DragRotatorAxisInfo
            {
                public float forceMultiplier;
                public float minDegrees;
                public float maxDegrees;
                public float restSeconds;
            }
        }
    }
}