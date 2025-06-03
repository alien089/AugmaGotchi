using UnityEngine;

namespace Managers
{
    public class AugmaManager : MonoBehaviour
    {
        [SerializeField] private GameObject AugmaPrefab;
        private GameObject _xAugma;
        
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.EventManager.Register(AugmaEventList.SPAWN_AUGMA, SpawnAugma);
        }

        private void SpawnAugma(object[] param)
        {
            if (_xAugma != null) 
                return;
            _xAugma = Instantiate(AugmaPrefab, PlayerController.Instance.FPlayerPosition + new Vector3(0,0,0.4f), Quaternion.identity);
        }
    }
}
