using Managers;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Character_System.Entity_Food_System
{
    [RequireComponent(typeof(MeshCollider))]
    // Represents a food item that can be grabbed and triggers related events.
    public class FoodInteractable : MonoBehaviour
    {
        [SerializeField] private float _fIncrementValue;

        public float FIncrementValue => _fIncrementValue;

        private HandGrabInteractable _xHandGrabInteractable;
        private MeshCollider _xCollider;
        private bool _bIsGrabbed = false;

        // Initialize components and register grab event handlers.
        private void Start()
        {
            _xHandGrabInteractable = GetComponentInChildren<HandGrabInteractable>();

            _xCollider = GetComponent<MeshCollider>();
            _xCollider.isTrigger = false;

            _xHandGrabInteractable.WhenSelectingInteractorViewAdded += OnGrabEnter;
            _xHandGrabInteractable.WhenSelectingInteractorViewRemoved += OnGrabExit;
        }

        // Unregister event handlers on application quit.
        private void OnApplicationQuit()
        {
            _xHandGrabInteractable.WhenSelectingInteractorViewAdded -= OnGrabEnter;
            _xHandGrabInteractable.WhenSelectingInteractorViewRemoved -= OnGrabExit;
        }

        // Called when the object is grabbed; triggers FOOD_GRABBED event.
        private void OnGrabEnter(IInteractorView interactor)
        {
            if (_bIsGrabbed) return;
            _bIsGrabbed = true;

            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_GRABBED);
        }

        // Called when the object is released; triggers FOOD_UNGRABBED event and destroys the object.
        private void OnGrabExit(IInteractorView interactor)
        {
            if (!_bIsGrabbed) return;
            _bIsGrabbed = false;

            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.FOOD_UNGRABBED);
            Destroy(gameObject);
        }

        // Trigger respawn event when the object is destroyed.
        private void OnDestroy()
        {
            GameManager.Instance.EventManager.TriggerEvent(FoodEventList.RESPAWN_FOOD);
        }
    }
}
