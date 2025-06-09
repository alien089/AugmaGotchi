using System;
using Managers;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Character_System.Food_System
{
    [RequireComponent(typeof(MeshCollider))]
    public class FoodInteractable : MonoBehaviour
    {
        private Grabbable _xGrabbable;
        private HandGrabInteractable _xHandGrabInteractable; 
        private GrabInteractable _xGrabInteractable;
        private HandActiveState _xHandActiveState;
        
        private MeshCollider _xCollider;
        
        private void Start()
        {
            _xGrabbable = GetComponentInChildren<Grabbable>();
            _xHandGrabInteractable = GetComponentInChildren<HandGrabInteractable>();
            _xGrabInteractable = GetComponentInChildren<GrabInteractable>();
            _xHandActiveState = GetComponent<HandActiveState>();
            
            _xCollider = GetComponent<MeshCollider>();
            _xCollider.isTrigger = false;
            
            
        }

        private void OnChangeState()
        {
            
        }

        private void OnDestroy()
        {
            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.RESPAWN_FOOD);
        }
    }
}
