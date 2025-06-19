using Managers;
using Oculus.Interaction;
using UnityEngine;

namespace Character_System.Caress_System
{
    [RequireComponent(typeof(SphereCollider))]
    public class CaressDetector : MonoBehaviour
    {
        private SphereCollider _xCollider;
        private bool _bIsHovering;
    
        private void OnTriggerEnter(Collider other)
        {
            if (_bIsHovering || other.GetComponentInParent<IInteractorView>() == null) return;
        
            IInteractorView interactable = other.GetComponentInParent<IInteractorView>();
            if (interactable.HasSelectedInteractable) return;

            _bIsHovering = true;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.WANT_CARESS);
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (!_bIsHovering || other.GetComponentInParent<IInteractorView>() == null) return;

            IInteractorView interactable = other.GetComponentInParent<IInteractorView>();
            if (interactable.HasSelectedInteractable) return;

            _bIsHovering = false;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.NOT_WANT_CARESS);
        }
    }
}
