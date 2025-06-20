using System;
using Internal_Use;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Character_System.Caress_System
{
    public class CaressComponent : MonoBehaviour
    {
        private HandGrabInteractable _xHandGrabInteractable;
        private bool _bIsPoking = false;
        private GameObject _xInteractorHand;
        private ParticleSystem _xParticleSystem;
        private Vector3 _vHandPosition;
        
        // Start is called before the first frame update
        void Start()
        {
            _xHandGrabInteractable = transform.parent.GetComponentInChildren<HandGrabInteractable>();
            _xParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (_bIsPoking)
                OnPokeContinuous(_xInteractorHand);
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
            if (_xInteractorHand != null) return;
            _bIsPoking = true;
            HandGrabInteractor grabInteractor = (HandGrabInteractor)interactor.Data;
            _xInteractorHand = grabInteractor.gameObject.transform.root.gameObject.GetComponentInChildren<IdentificatorAnchor>().gameObject;
            _vHandPosition = _xInteractorHand.transform.position;
        }

        private void OnPokeContinuous(GameObject hand)
        {
            Vector3 handPosition = hand.transform.position;
            if (handPosition != _vHandPosition)
            {
                if (!_xParticleSystem.isPlaying)
                    _xParticleSystem.Play();
                _vHandPosition = handPosition;
            }
        }
        
        private void OnPokeExit(IInteractorView interactor)
        {
            if (_xInteractorHand == null) return;
            _bIsPoking = false;
            _xInteractorHand = null;
            _xParticleSystem.Stop();
            _vHandPosition = Vector3.zero;
        }
    }
}
