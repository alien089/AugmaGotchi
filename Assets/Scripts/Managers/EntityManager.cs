using UnityEngine;
using Enums;

namespace Managers
{
    [System.Serializable()]
    public class StatsDictionary : SerializableDictionaryBase<Stats, float> { }

    // Manages entity stats, their increments, decrements, and related events.
    public class EntityManager : MonoBehaviour
    {
        [SerializeField] private GameObject EntityPrefab;

        [SerializeField] private StatsDictionary _fMaxValuesStats = new StatsDictionary();
        [SerializeField] private StatsDictionary _fCurrentValuesStats = new StatsDictionary();
        [SerializeField] private StatsDictionary _fDecrementValue = new StatsDictionary();

        public StatsDictionary FMaxValuesStats { get => _fMaxValuesStats; }
        public StatsDictionary FCurrentValuesStats { get => _fCurrentValuesStats; }

        private GameObject _xEntity;
        private bool _bIsHunger = false;

        // Register event listeners on start
        void Start()
        {
            GameManager.Instance.EventManager.Register(EntityEventList.SPAWN_Entity, SpawnEntity);
            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_GIVEN, IncrementStatsEvent);
            GameManager.Instance.EventManager.Register(CaressEventList.CARESS_GIVEN, IncrementStatsEvent);
        }

        // Unregister event listeners on application quit
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(EntityEventList.SPAWN_Entity, SpawnEntity);
            GameManager.Instance.EventManager.Unregister(FoodEventList.FOOD_GIVEN, IncrementStatsEvent);
            GameManager.Instance.EventManager.Unregister(CaressEventList.CARESS_GIVEN, IncrementStatsEvent);
        }

        // Updates stats decrement and hunger state every frame
        void Update()
        {
            if (!_xEntity) return;

            // Decrement joy, food and caress stats over time
            DecrementStats(Stats.JOY, _fDecrementValue[Stats.JOY]);
            DecrementStats(Stats.FOOD, _fDecrementValue[Stats.FOOD]);
            DecrementStats(Stats.CARESS, _fDecrementValue[Stats.CARESS]);

            // Check and trigger hunger audio events based on food stat threshold
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
        }

        // Spawns the entity and initializes current stats from max stats
        private void SpawnEntity(object[] param)
        {
            if (_xEntity != null) return;

            _xEntity = Instantiate(EntityPrefab, PlayerController.Instance.FPlayerPosition.position + new Vector3(0,0,0.4f), Quaternion.identity);
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.GIVE_Entity_TO_UI, this);

            foreach (var x in _fMaxValuesStats)
            {
                _fCurrentValuesStats.Add(x.Key, x.Value);
            }
        }

        // Decrements a specific stat over time and triggers audio events at thresholds
        private void DecrementStats(Stats statType, float decrementValue)
        {
            if (_fCurrentValuesStats[statType] > 0)
            {
                _fCurrentValuesStats[statType] -= decrementValue * Time.deltaTime;
                if (_fCurrentValuesStats[statType] < 0) _fCurrentValuesStats[statType] = 0;
            }

            if (Mathf.Approximately(_fCurrentValuesStats[statType], 1 * (_fMaxValuesStats[statType] / 3)) ||
                Mathf.Approximately(_fCurrentValuesStats[statType], 2 * (_fMaxValuesStats[statType] / 3)))
            {
                GameManager.Instance.EventManager.TriggerEvent(EntityEventList.SAD_AUDIO);
            }
        }

        // Handles increment stat events triggered by food or caress inputs
        private void IncrementStatsEvent(object[] param)
        {
            IncrementStats((Stats)param[0], (float)param[1]);

            if (_fCurrentValuesStats[(Stats)param[0]] <= 1 * (_fMaxValuesStats[(Stats)param[0]] / 3) ||
                _fCurrentValuesStats[(Stats)param[0]] <= 2 * (_fMaxValuesStats[(Stats)param[0]] / 3) || 
                Mathf.Approximately(_fCurrentValuesStats[(Stats)param[0]], _fMaxValuesStats[(Stats)param[0]]))
            {
                GameManager.Instance.EventManager.TriggerEvent(EntityEventList.HAPPY_AUDIO);
            }
        }

        // Increments a specific stat up to its max value
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
