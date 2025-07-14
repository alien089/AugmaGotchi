using System;
using Enums;
using Managers;
using UnityEngine;

namespace Character_System.Food_System
{
    [RequireComponent(typeof(SphereCollider))]
    // Manages food interactions by detecting when food objects enter its trigger zone.
    public class FoodComponent : MonoBehaviour
    {
        private SphereCollider _xSphereCollider;

        // Initializes the SphereCollider and sets it as a trigger.
        void Start()
        {
            _xSphereCollider = gameObject.GetComponent<SphereCollider>();
            _xSphereCollider.isTrigger = true;
        }

        // Enables or disables the collider to activate or deactivate the food component.
        public void EnableComponent(bool value)
        {
            _xSphereCollider.enabled = value;
        }

        // Detects when a food interactable enters the trigger and triggers a food given event.
        private void OnTriggerEnter(Collider other)
        {
            // If the collider belongs to a FoodInteractable
            if (other.TryGetComponent(out FoodInteractable interactable))
            {
                // Trigger event with food increment and destroy the interactable object
                GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_GIVEN, Stats.FOOD, interactable.FIncrementValue);
                Destroy(interactable.gameObject);
            }
        }
    }
}
