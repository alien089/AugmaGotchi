using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI_System
{
    public class CanvasManagerSpawnEntity : MonoBehaviour
    {
        private Button _xSpawnBtn;
        
        void Start()
        {
            _xSpawnBtn = transform.Find("SpawnBtn").GetComponent<Button>();
            
            _xSpawnBtn.onClick.AddListener(SpawnEntity);
        }

        private void SpawnEntity()
        {
            GetComponent<Canvas>().enabled = false;
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.SPAWN_Entity);
        }
    }
}
