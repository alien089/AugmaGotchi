using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Character_System.Caress_System
{
    public class CaressComponent : MonoBehaviour
    {
        private HandGrabInteractable _xHandGrabInteractable; 
        
        // Start is called before the first frame update
        void Start()
        {
            _xHandGrabInteractable = transform.parent.GetComponentInChildren<HandGrabInteractable>();
        }

        public void EnableComponent(bool value)
        {
            if (value)
            {
                _xHandGrabInteractable.WhenInteractorViewAdded += OnPokeEnter;
                _xHandGrabInteractable.WhenInteractorViewRemoved += OnPokeExit;
            }
            else
            {
                _xHandGrabInteractable.WhenInteractorViewAdded -= OnPokeEnter;
                _xHandGrabInteractable.WhenInteractorViewRemoved -= OnPokeExit;
            }
        }

        private void OnPokeEnter(IInteractorView interactor)
        {
            if (interactor.State == InteractorState.Hover)
                Debug.Log("enter " + interactor.State);
        }
        
        private void OnPokeExit(IInteractorView interactor)
        {
            Debug.Log("exit " + interactor.State);
        }
    }
}
