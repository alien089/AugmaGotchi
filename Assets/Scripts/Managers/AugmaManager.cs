using UnityEngine;
using Enums;

namespace Managers
{
    public class AugmaManager : MonoBehaviour
    {
        [SerializeField] private GameObject AugmaPrefab;
        
        [SerializeField] private StatsDictionary _fMaxValuesStats = new StatsDictionary();
        [SerializeField] private StatsDictionary _fCurrentValuesStats = new StatsDictionary();
        [SerializeField] private StatsDictionary _fDecrementValue = new StatsDictionary();
        
        public StatsDictionary FMaxValuesStats { get => _fMaxValuesStats; }
        public StatsDictionary FCurrentValuesStats { get => _fCurrentValuesStats; }
        
        private GameObject _xAugma;
        
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.EventManager.Register(AugmaEventList.SPAWN_AUGMA, SpawnAugma);
        }
        
        void Update()
        {
            if (!_xAugma) return;
            
            DecrementStats(Stats.JOY, _fDecrementValue[Stats.JOY]);
            DecrementStats(Stats.FOOD, _fDecrementValue[Stats.FOOD]);
            DecrementStats(Stats.CARESS, _fDecrementValue[Stats.CARESS]);
        }

        private void SpawnAugma(object[] param)
        {
            if (_xAugma != null) return;
            
            _xAugma = Instantiate(AugmaPrefab, PlayerController.Instance.FPlayerPosition + new Vector3(0,0,0.4f), Quaternion.identity);
            GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.GIVE_AUGMA_TO_UI, this);
            
            foreach (var x in _fMaxValuesStats)
            {
                _fCurrentValuesStats.Add(x.Key, x.Value);
            }
        }
        
        private void DecrementStats(Stats statType, float decrementValue)
        {
            if (_fCurrentValuesStats[statType] > 0)
            {
                _fCurrentValuesStats[statType] -= decrementValue * Time.deltaTime;
                if (_fCurrentValuesStats[statType] < 0) _fCurrentValuesStats[statType] = 0;
            }
        }
    }
}
