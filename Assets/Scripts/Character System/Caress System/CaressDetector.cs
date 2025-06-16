using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character_System.Caress_System
{
    [RequireComponent(typeof(SphereCollider))]
    public class CaressDetector : MonoBehaviour
    {
        private SphereCollider _xSphereCollider;
        
        // Start is called before the first frame update
        void Start()
        {
            _xSphereCollider =  GetComponent<SphereCollider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractorView interactor))
            {
                GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_GIVEN, Stats.FOOD, interactable.FIncrementValue);
                Destroy(interactable.gameObject);
            }
        }
    }
}
