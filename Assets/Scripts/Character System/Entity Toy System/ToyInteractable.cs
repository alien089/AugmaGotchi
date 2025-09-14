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
        [SerializeField] private float fVelocityThreshold;

        public float FIncrementValue => fIncrementValue;
        public float FVelocityThreshold => fVelocityThreshold;

        private Rigidbody _xRigidBody;
        private HandGrabInteractable _xHandGrabInteractable;
        private SphereCollider _xCollider;
        private bool _bIsDocked = true;
        private bool _bIsGrabbed = true;

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
            
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_COLLECTED, ToyCollected);
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_RETURNED, ToyReturned);
        }

        private void Update()
        {
            if (_bIsDocked) return;
            if (_bIsGrabbed) return;
            if (_xRigidBody.velocity.magnitude != 0f) return;
            if (_xRigidBody.velocity.magnitude > fVelocityThreshold) return;
            SetGravity(false);
            ToyThrown();
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
            if (_bIsDocked == false) return;
            _bIsDocked = false;

            SetGravity(true);

            GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_GRABBED);
        }

        // Called when the object is released; triggers FOOD_UNGRABBED event and destroys the object.
        private void OnGrabExit(IInteractorView interactor)
        {
            if (_bIsDocked) return;
            _bIsGrabbed = false;
        }

        private void ToyThrown()
        {
            Vector3 pos = new Vector3(
                gameObject.transform.position.x, 
                gameObject.transform.position.y - gameObject.transform.localScale.x/2, 
                gameObject.transform.position.z);
            
            GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_THROWN, pos);
        }

        // Trigger respawn event when the object is destroyed.
        private void ToyCollected(object[] param)
        {
            Transform transformEntity = (Transform)param[0];
            transform.parent = transformEntity;
        }
        
        private void ToyReturned(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.RESPAWN_FOOD);
            Destroy(gameObject);
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
