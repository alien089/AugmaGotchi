using Managers;
using UnityEngine;
using AudioType = Enums.AudioType;

namespace Character_System.Audio
{
    // Handles character-related audio playback triggered by AudioManager events.
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField] protected AudioType _eAudioType;
        protected AudioSource _xAudioSource;

        // Initializes AudioSource and registers this component to listen for audio events.
        void Start()
        {
            _xAudioSource = GetComponent<AudioSource>();

            // Register this component to listen for the specified audio type event.
            GameManager.Instance.EventManager.Register(GameManager.Instance.AudioManager.AudioEvents[_eAudioType], CallAudio);
        }

        // Called when the registered audio event is triggered; intended for override.
        protected virtual void CallAudio(object[] param)
        {

        }
    }
}
