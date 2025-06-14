using System;
using Enums;
using Managers;
using UnityEngine;

namespace Character_System.Food_System
{
    [RequireComponent(typeof(SphereCollider))]
    public class FoodComponent : MonoBehaviour
    {
        private SphereCollider _xSphereCollider; 
    
        void Start()
        {
            _xSphereCollider = gameObject.GetComponent<SphereCollider>();
            _xSphereCollider.isTrigger = true;
        }

        public void EnableComponent(bool value)
        {
            _xSphereCollider.enabled = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out FoodInteractable interactable))
            {
                GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_GIVEN, Stats.FOOD, interactable.FIncrementValue);
                Destroy(interactable.gameObject);
            }
        }
    }
}
