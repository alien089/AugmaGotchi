using Framework.Generics.Pattern.SingletonPattern;
using Misc;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private EventManager _xEventManager = Factory.CreateEventManager();
        private SaveManager _xSaveManager; 
        private AudioManager _xAudioManager; 
        private AugmaManager _xAugmaManager; 

        public EventManager EventManager { get => _xEventManager; }
        public SaveManager SaveManager { get => _xSaveManager; }
        public AudioManager AudioManager { get => _xAudioManager; }
        public AugmaManager AugmaManager { get => _xAugmaManager; }

        private void Start()
        {
            //_xSaveManager = GetComponentInChildren<SaveManager>();
            _xAudioManager = GetComponentInChildren<AudioManager>();
            _xAugmaManager = GetComponentInChildren<AugmaManager>();
        }
    }
}
