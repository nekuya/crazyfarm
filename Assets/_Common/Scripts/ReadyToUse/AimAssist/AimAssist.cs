using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimAssist : MonoBehaviour
{
    [SerializeField] protected float detectionRadius = 2f;
    [SerializeField] protected float detectionAngle = 40f;
    [SerializeField] protected float distanceWeight = 3f;
    [SerializeField] protected float directionWeight = 1f;
    [SerializeField] protected LayerMask aimTargetLayerMask;

    [SerializeField, Foldout("Debug")] protected Color debugColor = Color.white;

    /// <summary>
    /// Works with MonoBehaviours as well as Interfaces
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private List<T> GetElementsInRange<T>(Func<T, bool> filter = null) where T : class
    {
        List<T> elements = new List<T>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, aimTargetLayerMask);

        T lElement;
        MonoBehaviour lMonoBehaviour;

        foreach (Collider lCollider in colliders)
        {
            lElement = lCollider.gameObject.GetComponent<T>();

            if (lElement != null)
            {
                if (filter != null && !filter(lElement))
                    continue;

                lMonoBehaviour = lElement as MonoBehaviour;

                if (lMonoBehaviour != null)
                {
                    Vector3 interactableDirection = lMonoBehaviour.transform.position - transform.position;
                    float angleToInteractable = Vector3.Angle(interactableDirection, transform.forward);

                    if (lMonoBehaviour.transform != transform && angleToInteractable <= detectionAngle)
                        elements.Add(lElement);
                }
            }
        }

        return elements;
    }

    /// <param name="priorityFilter">Elements which satisfy priority filter will be taken instead of others.</param>
    /// <param name="mandatoryFilter">Elements which does not satisfy mandatory filter will not be taken.</param>
    public bool GetClosestElement<T>(out T closest, Func<T, bool> priorityFilter = null, Func<T, bool> mandatoryFilter = null) where T : class
    {
        List<T> lElements = GetElementsInRange(mandatoryFilter);

        if (lElements != null && lElements.Count > 0)
        {
            MonoBehaviour lMonoBehaviour;
            T lClosestElement = null;
            float lBestScore = float.MinValue;

            foreach (T lElement in lElements)
            {
                lMonoBehaviour = lElement as MonoBehaviour;

                Vector3 lToElement = lMonoBehaviour.transform.position - transform.position;
                float lDotScore = Vector3.Dot(lToElement.normalized, transform.forward);
                float lDistanceScore = 1f - lToElement.magnitude / detectionRadius;

                float lTotalScore = lDotScore * directionWeight + lDistanceScore * distanceWeight;

                bool lHasHigherPriorityThanClosest = priorityFilter != null && priorityFilter(lElement) 
                                                     && lClosestElement != null && !priorityFilter(lClosestElement);

                //If element has a higher priority than current closest, force it as closest.
                if (lTotalScore > lBestScore || lHasHigherPriorityThanClosest)
                {
                    lClosestElement = lElement;
                    lBestScore = lTotalScore;
                }
            }

            closest = lClosestElement;
            return true;
        }
        else
        {
            closest = null;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        GizmosExtensions.DrawAngle(transform.position, transform.forward, detectionAngle, detectionRadius);
    }
}