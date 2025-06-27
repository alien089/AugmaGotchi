using System;
using Enums;
using Internal_Use;
using Managers;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using UnityEngine;

namespace Character_System.Caress_System
{
    public class CaressComponent : MonoBehaviour
    {
        [SerializeField, Range(1, 2)] private float _fDeadZone;
        [SerializeField] private float _fIncrementValue;
        
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
            if (grabInteractor.Hand.Handedness == Handedness.Right)
                _xInteractorHand = grabInteractor.gameObject.transform.root.GetComponentInChildren<IdentificatorAnchorRight>().gameObject;
            else
                _xInteractorHand = grabInteractor.gameObject.transform.root.GetComponentInChildren<IdentificatorAnchorLeft>().gameObject;
            _vHandPosition = _xInteractorHand.transform.position;
        }

        private void OnPokeContinuous(GameObject hand)
        {
            Vector3 handPosition = hand.transform.position;
            
            if (Vector3.Distance(handPosition, _vHandPosition) > _fDeadZone / 1000)
            {
                if (!_xParticleSystem.isPlaying)
                    _xParticleSystem.Play();
                _vHandPosition = handPosition;
                GameManager.Instance.EventManager.TriggerEvent(CaressEventList.CARESS_GIVEN, Stats.CARESS, _fIncrementValue / 10);
                GameManager.Instance.EventManager.TriggerEvent(CaressEventList.AUDIO_CARESS, true);
            }
            else
            {
                if (_xParticleSystem.isPlaying)
                    _xParticleSystem.Stop();
                GameManager.Instance.EventManager.TriggerEvent(CaressEventList.AUDIO_CARESS, false);
            }
        }
        
        private void OnPokeExit(IInteractorView interactor)
        {
            if (_xInteractorHand == null) return;
            _bIsPoking = false;
            _xInteractorHand = null;
            _xParticleSystem.Stop();
            _vHandPosition = Vector3.zero;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.AUDIO_CARESS, false);
        }
    }
}
