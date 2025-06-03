using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers.MainMenu
{
    public class UIMainMenuCanvasManager : MonoBehaviour
    {
        [SerializeField]
        private string _MainSceneName;
    
        private Button _xStartButton;
        
        void Start()
        {
            _xStartButton = transform.Find("StartBtn").GetComponent<Button>();
        
            _xStartButton.onClick.AddListener(LoadMainScene);
        }

        private void LoadMainScene()
        {
            MainMenuGameManager.Instance.EventManager.TriggerEvent(MainMenuEventList.LOAD_SCENE, _MainSceneName);
        }
    }
}
