using Framework.Generics.Pattern.SingletonPattern;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.MainMenu
{
    public class MainMenuGameManager : Singleton<MainMenuGameManager>
    {
        [SerializeField] private string _MainSceneName;
        
        private EventManager _xEventManager = Factory.CreateEventManager();
        private SaveManager _xSaveManager; 

        public EventManager EventManager { get => _xEventManager; }
        public SaveManager SaveManager { get => _xSaveManager; }

        private void Start()
        {
            _xSaveManager = GetComponentInChildren<SaveManager>();
            
            Instance.EventManager.Register(MainMenuEventList.LOAD_SCENE, LoadMainScene);
        }
        
        private void LoadMainScene(object[] param)
        {
            SceneManager.LoadScene(_MainSceneName);
        }
    }
}
