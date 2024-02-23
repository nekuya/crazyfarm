///-----------------------------------------------------------------
/// Author : Gabriel Bernabeu
/// Date : 14/03/2022 11:36
///-----------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.GabrielBernabeu.Common
{
    public class DirectionIndicator : MonoBehaviour
    {
        [SerializeField] private Transform target = default;
        [SerializeField] private GameObject renderersBox = default;
        [SerializeField] private Image rotativeRenderer = default;
        [SerializeField] [Range(0.01f, 1f)] private float padding = 0.19f;

        private float centerToTopRightAngle;
        private float centerToTopLeftAngle;

        private void Awake()
        {
            Camera lMainCamera = Camera.main;

            Vector2 lScreenCenter = new Vector2(lMainCamera.pixelWidth * 0.5f, lMainCamera.pixelHeight * 0.5f);
            Vector2 lCenterToTopRight = new Vector2(lMainCamera.pixelWidth, lMainCamera.pixelHeight) - lScreenCenter;
            Vector2 lCenterToTopLeft = new Vector2(0f, lMainCamera.pixelHeight) - lScreenCenter;
            centerToTopRightAngle = Mathf.Atan2(lCenterToTopRight.y, lCenterToTopRight.x);
            centerToTopLeftAngle = Mathf.Atan2(lCenterToTopLeft.y, lCenterToTopLeft.x);
        }

        private void Update()
        {
            Camera lMainCamera = Camera.main;
            Vector2 lGolemPositionOnScreen = lMainCamera.WorldToScreenPoint(target.position);

            bool lIsOutOfScreen = lGolemPositionOnScreen.x > lMainCamera.pixelWidth || lGolemPositionOnScreen.x < 0f ||
                              lGolemPositionOnScreen.y > lMainCamera.pixelHeight || lGolemPositionOnScreen.y < 0f;

            //Deactivate itself if the Golem is visible on screen.
            renderersBox.SetActive(lIsOutOfScreen);

            if (lIsOutOfScreen)
                return;

            Vector2 lHalfScreenDimensions = new Vector2(lMainCamera.pixelWidth * 0.5f, lMainCamera.pixelHeight * 0.5f);
            Vector2 lGolemPositionFromScreenCenter = lGolemPositionOnScreen - lHalfScreenDimensions;
            float lAngle = Mathf.Atan2(lGolemPositionFromScreenCenter.y, lGolemPositionFromScreenCenter.x);
            float lTangent = Mathf.Sin(lAngle) / Mathf.Cos(lAngle);

            Vector2 lRestrainedToScreenPosition = new Vector2();
            //Vector2 lPaddedHalfScreenDimensions = lHalfScreenDimensions - new Vector2(lMainCamera.pixelWidth, lMainCamera.pixelWidth) * paddingRatioFromExtremities * 0.5f;

            Debug.Log(lAngle > -centerToTopRightAngle && lAngle <= centerToTopLeftAngle);

            if (lAngle > -centerToTopRightAngle && lAngle <= centerToTopRightAngle)
            {
                lRestrainedToScreenPosition = new Vector2(lHalfScreenDimensions.x, lHalfScreenDimensions.x * lTangent);
            }
            else if (lAngle > centerToTopRightAngle && lAngle <= centerToTopLeftAngle)
            {
                lRestrainedToScreenPosition = new Vector2(lHalfScreenDimensions.y / lTangent, lHalfScreenDimensions.y);
            }
            else if ((lAngle > centerToTopLeftAngle && lAngle <= 180f) || lAngle < -centerToTopLeftAngle && lAngle >= -180f)
            {
                lRestrainedToScreenPosition = new Vector2(-lHalfScreenDimensions.x, -lHalfScreenDimensions.x * lTangent);
            }
            else if (lAngle > -centerToTopLeftAngle && lAngle <= -centerToTopRightAngle)
            {
                lRestrainedToScreenPosition = new Vector2(-lHalfScreenDimensions.y / lTangent, -lHalfScreenDimensions.y);
            }

            lRestrainedToScreenPosition = lRestrainedToScreenPosition.normalized * 
                                          (lRestrainedToScreenPosition.magnitude - 
                                          lMainCamera.ViewportToScreenPoint(padding * 0.5f * Vector3.one).x);

            lRestrainedToScreenPosition += lHalfScreenDimensions;
            Vector3 lNewPosition = lMainCamera.ScreenToWorldPoint(new Vector3(lRestrainedToScreenPosition.x,
                                                                  lRestrainedToScreenPosition.y, 0f));

            transform.position = new Vector3(lNewPosition.x, lNewPosition.y, transform.position.z);

            //Show Golem direction.
            rotativeRenderer.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, lAngle * Mathf.Rad2Deg));
        }
    }
}