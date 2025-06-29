using UnityEngine;
using Enums;

namespace Managers
{
    [System.Serializable()]
    public class StatsDictionary : SerializableDictionaryBase<Stats, float> { }
    
    public class AugmaManager : MonoBehaviour
    {
        [SerializeField] private GameObject AugmaPrefab;
        
        [SerializeField] private StatsDictionary _fMaxValuesStats = new StatsDictionary();
        [SerializeField] private StatsDictionary _fCurrentValuesStats = new StatsDictionary();
        [SerializeField] private StatsDictionary _fDecrementValue = new StatsDictionary();
        
        public StatsDictionary FMaxValuesStats { get => _fMaxValuesStats; }
        public StatsDictionary FCurrentValuesStats { get => _fCurrentValuesStats; }
        
        private GameObject _xAugma;
        
        private bool _bIsHunger = false;
        
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.EventManager.Register(AugmaEventList.SPAWN_AUGMA, SpawnAugma);
            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_GIVEN, IncrementStatsEvent);
            
            GameManager.Instance.EventManager.Register(CaressEventList.CARESS_GIVEN, IncrementStatsEvent);
        }
        
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(AugmaEventList.SPAWN_AUGMA, SpawnAugma);
            GameManager.Instance.EventManager.Unregister(FoodEventList.FOOD_GIVEN, IncrementStatsEvent);
            
            GameManager.Instance.EventManager.Unregister(CaressEventList.CARESS_GIVEN, IncrementStatsEvent);
        }
        
        void Update()
        {
            if (!_xAugma) return;
            
            DecrementStats(Stats.JOY, _fDecrementValue[Stats.JOY]);
            DecrementStats(Stats.FOOD, _fDecrementValue[Stats.FOOD]);

            if (FCurrentValuesStats[Stats.FOOD] <= FMaxValuesStats[Stats.FOOD] / 3 && _bIsHunger == false) 
            {
                GameManager.Instance.EventManager.TriggerEvent(FoodEventList.IS_HUNGER, true);
                _bIsHunger = true;
            }
            else if (FCurrentValuesStats[Stats.FOOD] > FMaxValuesStats[Stats.FOOD] / 3)
            {
                GameManager.Instance.EventManager.TriggerEvent(FoodEventList.IS_HUNGER, false);
                _bIsHunger = false;
            }
            
            DecrementStats(Stats.CARESS, _fDecrementValue[Stats.CARESS]);
        }

        private void SpawnAugma(object[] param)
        {
            if (_xAugma != null) return;
            
            _xAugma = Instantiate(AugmaPrefab, PlayerController.Instance.FPlayerPosition.position + new Vector3(0,0,0.4f), Quaternion.identity);
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
            
            if (_fCurrentValuesStats[statType] == 1 * (_fMaxValuesStats[statType] / 3) ||
                _fCurrentValuesStats[statType] == 2 * (_fMaxValuesStats[statType] / 3))
            {
                GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.SAD_AUDIO);
            }
        }

        private void IncrementStatsEvent(object[] param)
        {
            IncrementStats((Stats)param[0], (float)param[1]);

            if (_fCurrentValuesStats[(Stats)param[0]] <= 1 * (_fMaxValuesStats[(Stats)param[0]] / 3) ||
                _fCurrentValuesStats[(Stats)param[0]] <= 2 * (_fMaxValuesStats[(Stats)param[0]] / 3) || 
                Mathf.Approximately(_fCurrentValuesStats[(Stats)param[0]], _fMaxValuesStats[(Stats)param[0]]))
            {
                GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.HAPPY_AUDIO);
            }
        }

        private void IncrementStats(Stats statType, float incrementValue)
        {
            if (_fCurrentValuesStats[statType] < _fMaxValuesStats[statType])
            {
                _fCurrentValuesStats[statType] += incrementValue;
                if (_fCurrentValuesStats[statType] > _fMaxValuesStats[statType]) _fCurrentValuesStats[statType] = _fMaxValuesStats[statType];
            }
        }
    }
}
