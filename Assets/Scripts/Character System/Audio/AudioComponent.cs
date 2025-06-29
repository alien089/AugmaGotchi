using Managers;
using UnityEngine;
using AudioType = Enums.AudioType;

namespace Character_System.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField] protected AudioType _eAudioType;
        protected AudioSource _xAudioSource;
    
        // Start is called before the first frame update
        void Start()
        {
            _xAudioSource = GetComponent<AudioSource>();
            
            GameManager.Instance.EventManager.Register(GameManager.Instance.AudioManager.AudioEvents[_eAudioType], CallAudio);
        }

        protected virtual void CallAudio(object[] param)
        {
            
        }
    }
}
