using UnityEngine;
using UnityEngine.UI;

namespace Managers.MainMenu
{
    public class UIMainMenuCanvasManager : MonoBehaviour
    {
        private Button _xStartButton;
        
        void Start()
        {
            _xStartButton = transform.Find("StartBtn").GetComponent<Button>();
        
            _xStartButton.onClick.AddListener(LoadMainScene);
        }

        private void LoadMainScene()
        {
            MainMenuGameManager.Instance.EventManager.TriggerEvent(MainMenuEventList.LOAD_SCENE);
        }
    }
}
