using Framework.Generics.Pattern.SingletonPattern;
using Misc;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private EventManager _xEventManager = Factory.CreateEventManager();
        private SaveManager _xSaveManager; 

        public EventManager EventManager { get => _xEventManager; }
        public SaveManager SaveManager { get => _xSaveManager; }

        private void Start()
        {
            _xSaveManager = GetComponentInChildren<SaveManager>();
        }
    }
}
