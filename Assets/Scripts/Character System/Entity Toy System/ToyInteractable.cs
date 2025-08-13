using System;
using Managers;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Character_System.Entity_Toy_System
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    // Represents a food item that can be grabbed and triggers related events.
    public class ToyInteractable : MonoBehaviour
    {
        [SerializeField] private float fIncrementValue;

        public float FIncrementValue => fIncrementValue;

        private Rigidbody _xRigidBody;
        private HandGrabInteractable _xHandGrabInteractable;
        private SphereCollider _xCollider;
        private bool _bIsGrabbed = false;

        // Initialize components and register grab event handlers.
        private void Start()
        {
            _xHandGrabInteractable = GetComponentInChildren<HandGrabInteractable>();

            _xCollider = GetComponent<SphereCollider>();
            _xCollider.isTrigger = false;
            
            _xRigidBody = GetComponent<Rigidbody>();
            SetGravity(false);

            _xHandGrabInteractable.WhenSelectingInteractorViewAdded += OnGrabEnter;
            _xHandGrabInteractable.WhenSelectingInteractorViewRemoved += OnGrabExit;
        }

        // Unregister event handlers on application quit.
        private void OnApplicationQuit()
        {
            _xHandGrabInteractable.WhenSelectingInteractorViewAdded -= OnGrabEnter;
            _xHandGrabInteractable.WhenSelectingInteractorViewRemoved -= OnGrabExit;
        }

        // Called when the object is grabbed; triggers TOY_GRABBED event.
        private void OnGrabEnter(IInteractorView interactor)
        {
            if (_bIsGrabbed) return;
            _bIsGrabbed = true;

            SetGravity(true);

            GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_GRABBED);
        }

        // Called when the object is released; triggers FOOD_UNGRABBED event and destroys the object.
        private void OnGrabExit(IInteractorView interactor)
        {
            if (!_bIsGrabbed) return;
            _bIsGrabbed = false;
        }

        //Called when the object lands on a surface
        private void OnCollisionEnter(Collision other)
        {
            GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_THROWN);
        }

        // Trigger respawn event when the object is destroyed.
        private void OnDestroy()
        {
            //GameManager.Instance.EventManager.TriggerEvent(FoodEventList.RESPAWN_FOOD);
        }

        private void SetGravity(bool isOn)
        {
            if (isOn)
            {
                _xRigidBody.isKinematic = false;
                _xRigidBody.useGravity = true;
            }
            else
            {
                _xRigidBody.isKinematic = true;
                _xRigidBody.useGravity = false;
            }
        }
    }
}
