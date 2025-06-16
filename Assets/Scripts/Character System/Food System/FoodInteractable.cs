using System;
using Enums;
using Managers;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Character_System.Food_System
{
    [RequireComponent(typeof(MeshCollider))]
    public class FoodInteractable : MonoBehaviour
    {
        [SerializeField] private float _fIncrementValue;

        public float FIncrementValue { get => _fIncrementValue; }

        private Grabbable _xGrabbable;
        private HandGrabInteractable _xHandGrabInteractable; 
        private GrabInteractable _xGrabInteractable;
        private HandActiveState _xHandActiveState;
        
        private MeshCollider _xCollider;
        
        private bool _bIsGrabbed = false;
        
        private void Start()
        {
            _xGrabbable = GetComponentInChildren<Grabbable>();
            _xHandGrabInteractable = GetComponentInChildren<HandGrabInteractable>();
            _xGrabInteractable = GetComponentInChildren<GrabInteractable>();
            _xHandActiveState = GetComponent<HandActiveState>();
            
            _xCollider = GetComponent<MeshCollider>();
            _xCollider.isTrigger = false;

            //on grab event
            _xHandGrabInteractable.WhenSelectingInteractorViewAdded += OnGrabEnter;
            _xHandGrabInteractable.WhenSelectingInteractorViewRemoved += OnGrabExit;
        }
        
        private void OnApplicationQuit()
        {
            _xHandGrabInteractable.WhenSelectingInteractorViewAdded -= OnGrabEnter;
            _xHandGrabInteractable.WhenSelectingInteractorViewRemoved -= OnGrabExit;
        }

        private void OnGrabEnter(IInteractorView interactor)
        {
            if (_bIsGrabbed) return;
            _bIsGrabbed = true;
            
            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_GRABBED);
        }
        
        private void OnGrabExit(IInteractorView interactor)
        {
            if (!_bIsGrabbed) return;
            _bIsGrabbed = false;
            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_UNGRABBED);
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.RESPAWN_FOOD);
        }
    }
}
