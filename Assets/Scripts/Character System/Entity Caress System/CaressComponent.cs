using Enums;
using Internal_Use;
using Managers;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using UnityEngine;

namespace Character_System.Entity_Caress_System
{
    // Manages caress interactions by detecting poke gestures and triggering related events and particle effects.
    public class CaressComponent : MonoBehaviour
    {
        [SerializeField, Range(1, 2)] private float _fDeadZone;
        [SerializeField] private float _fIncrementValue;

        private HandGrabInteractable _xHandGrabInteractable;
        private bool _bIsPoking = false;
        private GameObject _xInteractorHand;
        private ParticleSystem _xParticleSystem;
        private Vector3 _vHandPosition;

        // Initializes references to HandGrabInteractable and ParticleSystem components.
        void Start()
        {
            _xHandGrabInteractable = transform.parent.GetComponentInChildren<HandGrabInteractable>();
            _xParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
        }

        // Continuously processes poke interaction if active.
        private void Update()
        {
            if (_bIsPoking)
                OnPokeContinuous(_xInteractorHand);
        }

        // Enables or disables poke interaction event subscriptions.
        public void EnableComponent(bool value)
        {
            if (value)
            {
                // Subscribe to poke enter and exit events
                _xHandGrabInteractable.WhenInteractorViewAdded += OnPokeEnter;
                _xHandGrabInteractable.WhenInteractorViewRemoved += OnPokeExit;
            }
            else
            {
                // Unsubscribe from poke enter and exit events
                _xHandGrabInteractable.WhenInteractorViewAdded -= OnPokeEnter;
                _xHandGrabInteractable.WhenInteractorViewRemoved -= OnPokeExit;
            }
        }

        // Handles the start of a poke interaction, setting the hand reference and initial position.
        private void OnPokeEnter(IInteractorView interactor)
        {
            // Prevent multiple pokes at once
            if (_xInteractorHand != null) return;

            _bIsPoking = true;

            HandGrabInteractor grabInteractor = (HandGrabInteractor)interactor.Data;

            // Identify which hand is interacting
            if (grabInteractor.Hand.Handedness == Handedness.Right)
                _xInteractorHand = grabInteractor.gameObject.transform.root.GetComponentInChildren<IdentificatorAnchorRight>().gameObject;
            else
                _xInteractorHand = grabInteractor.gameObject.transform.root.GetComponentInChildren<IdentificatorAnchorLeft>().gameObject;

            // Store initial hand position
            _vHandPosition = _xInteractorHand.transform.position;
        }

        // Called continuously during a poke to detect movement beyond the dead zone and trigger events accordingly.
        private void OnPokeContinuous(GameObject hand)
        {
            Vector3 handPosition = hand.transform.position;

            // Check if hand moved beyond dead zone threshold
            if (Vector3.Distance(handPosition, _vHandPosition) > _fDeadZone / 1000)
            {
                // Play particle effect if not playing
                if (!_xParticleSystem.isPlaying)
                    _xParticleSystem.Play();

                // Update stored hand position
                _vHandPosition = handPosition;

                // Trigger caress increment and audio start events
                GameManager.Instance.EventManager.TriggerEvent(CaressEventList.CARESS_GIVEN, Stats.CARESS, _fIncrementValue / 10);
                GameManager.Instance.EventManager.TriggerEvent(CaressEventList.AUDIO_CARESS, true);
            }
            else
            {
                // Stop particle effect if playing
                if (_xParticleSystem.isPlaying)
                    _xParticleSystem.Stop();

                // Trigger audio stop event
                GameManager.Instance.EventManager.TriggerEvent(CaressEventList.AUDIO_CARESS, false);
            }
        }

        // Handles the end of a poke interaction, resetting state and stopping effects.
        private void OnPokeExit(IInteractorView interactor)
        {
            // Return early if no hand is currently interacting
            if (_xInteractorHand == null) return;

            // Reset poke state and hand reference
            _bIsPoking = false;
            _xInteractorHand = null;

            // Stop particle effects and reset hand position
            _xParticleSystem.Stop();
            _vHandPosition = Vector3.zero;

            // Trigger audio stop event
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.AUDIO_CARESS, false);
        }
    }
}
