using Managers;
using Oculus.Interaction;
using UnityEngine;

namespace Character_System.Entity_Caress_System
{
    [RequireComponent(typeof(SphereCollider))]
    // Detects when an interactor enters or exits the caress interaction zone.
    public class CaressDetector : MonoBehaviour
    {
        private SphereCollider _xCollider;
        private bool _bIsHovering;

        // Triggered when another collider enters this collider's trigger zone.
        private void OnTriggerEnter(Collider other)
        {
            // Ignore if already hovering or collider has no interactor view
            if (_bIsHovering || other.GetComponentInParent<IInteractorView>() == null) return;

            IInteractorView interactable = other.GetComponentInParent<IInteractorView>();

            // Ignore if interactor is currently selecting another interactable
            if (interactable.HasSelectedInteractable) return;

            // Set hovering state and trigger event indicating desire for caress
            _bIsHovering = true;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.WANT_CARESS);
        }

        // Triggered when another collider exits this collider's trigger zone.
        private void OnTriggerExit(Collider other)
        {
            // Ignore if not hovering or collider has no interactor view
            if (!_bIsHovering || other.GetComponentInParent<IInteractorView>() == null) return;

            IInteractorView interactable = other.GetComponentInParent<IInteractorView>();

            // Ignore if interactor is currently selecting another interactable
            if (interactable.HasSelectedInteractable) return;

            // Reset hovering state and trigger event indicating no longer wanting caress
            _bIsHovering = false;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.NOT_WANT_CARESS);
        }
    }
}
