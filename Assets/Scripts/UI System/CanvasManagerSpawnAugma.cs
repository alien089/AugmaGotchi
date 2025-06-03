using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI_System
{
    public class CanvasManagerSpawnAugma : MonoBehaviour
    {
        private Button _xSpawnBtn;
        
        void Start()
        {
            _xSpawnBtn = transform.Find("SpawnBtn").GetComponent<Button>();
            
            _xSpawnBtn.onClick.AddListener(SpawnAugma);
        }

        private void SpawnAugma()
        {
            GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.SPAWN_AUGMA);
        }
    }
}
