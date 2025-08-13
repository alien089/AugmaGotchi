using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI_System
{
    public class CanvasManagerSpawnEntity : MonoBehaviour
    {
        private Button _xSpawnBtn;
        private Button _xSpaceSetupBtn;
        
        void Start()
        {
            _xSpawnBtn = transform.Find("SpawnBtn").GetComponent<Button>();
            _xSpaceSetupBtn = transform.Find("SpaceSetupBtn").GetComponent<Button>();
            
            _xSpawnBtn.onClick.AddListener(SpawnEntity);
            _xSpaceSetupBtn.onClick.AddListener(SpaceSetup);
        }

        private void SpawnEntity()
        {
            GetComponentInChildren<Button>().gameObject.SetActive(false);
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.SPAWN_Entity);
        }

        private void SpaceSetup()
        {
            OVRScene.RequestSpaceSetup();
        }
    }
}
