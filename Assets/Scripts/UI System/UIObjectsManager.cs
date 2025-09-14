using Enums;
using Managers;
using UnityEngine;

namespace UI_System
{
    // Manages UI item instances and updates entity states based on interactions.
    public class UIObjectsManager : MonoBehaviour
    {
        [Header("Food Settings")]
        [SerializeField] private GameObject _xFoodPrefab;
        [SerializeField] private GameObject _xFoodInstance;
        [SerializeField] private Transform _xFoodPosition;

        [Header("Toy Settings")]
        [SerializeField] private GameObject _xToyPrefab;
        [SerializeField] private GameObject _xToyInstance;
        [SerializeField] private Transform _xToyPosition;

        // Initializes UI item instances and registers event listeners.
        void Start()
        {
            // Spawn food UI object and register food-related events
            if (_xFoodPrefab == null || _xFoodPosition == null) return;
            SpawnItem(ref _xFoodInstance, _xFoodPrefab, _xFoodPosition);

            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_GRABBED, FoodGrabbed);
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_GRABBED, ToyGrabbed);
            GameManager.Instance.EventManager.Register(CaressEventList.WANT_CARESS, WantCaress);

            // Spawn toy UI object and register toy-related events
            if (_xToyPrefab == null || _xToyPosition == null) return;
            SpawnItem(ref _xToyInstance, _xToyPrefab, _xToyPosition);

            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_UNGRABBED, FoodUngrabbed);
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_THROWN, ToyThrown);
            GameManager.Instance.EventManager.Register(ToyEventList.RESPAWN_TOY, ToyRespawn);
            GameManager.Instance.EventManager.Register(CaressEventList.NOT_WANT_CARESS, NotWantCaress);
        }

        // Unregisters all event listeners on application quit.
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(FoodEventList.FOOD_GRABBED, FoodGrabbed);
            GameManager.Instance.EventManager.Unregister(ToyEventList.TOY_GRABBED, ToyGrabbed);
            GameManager.Instance.EventManager.Unregister(CaressEventList.WANT_CARESS, WantCaress);

            GameManager.Instance.EventManager.Unregister(FoodEventList.FOOD_UNGRABBED, FoodUngrabbed);
            GameManager.Instance.EventManager.Unregister(ToyEventList.TOY_THROWN, ToyThrown);
            GameManager.Instance.EventManager.Unregister(ToyEventList.RESPAWN_TOY, ToyRespawn);
            GameManager.Instance.EventManager.Unregister(CaressEventList.NOT_WANT_CARESS, NotWantCaress);
        }

        // Instantiates and parents an item instance at a specified position.
        private void SpawnItem(ref GameObject itemInstance, GameObject item, Transform position)
        {
            itemInstance = Instantiate(item, position);
            itemInstance.transform.SetParent(transform);
        }

        #region Entity Food System

        // Detaches food UI instance and sets entity state to FOOD.
        private void FoodGrabbed(object[] param)
        {
            _xFoodInstance.transform.SetParent(null);
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_ENTITY_STATE, EntityStates.FOOD);
        }

        // Respawns food UI instance and sets entity state to IDLE.
        private void FoodUngrabbed(object[] param)
        {
            SpawnItem(ref _xFoodInstance, _xFoodPrefab, _xFoodPosition);
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_ENTITY_STATE, EntityStates.IDLE);
        }

        #endregion

        #region Entity Caress System

        // Sets entity state to CARESS when caress is wanted.
        private void WantCaress(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_ENTITY_STATE, EntityStates.CARESS);
        }

        // Sets entity state to IDLE when caress is no longer wanted.
        private void NotWantCaress(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_ENTITY_STATE, EntityStates.IDLE);
        }

        #endregion

        #region Entity Toy System

        // Detaches toy UI instance from parent.
        private void ToyGrabbed(object[] param)
        {
            _xToyInstance.transform.SetParent(null);
            
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_ENTITY_STATE, EntityStates.TOY);
            
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_TOY_STATE, ToyStates.IDLE);
        }

        // Respawns toy UI instance at designated position.
        private void ToyThrown(object[] param)
        { 
            Vector3 position = (Vector3)param[0];
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_TOY_STATE, ToyStates.MOVE, position);
        }
        
        private void ToyRespawn(object[] param)
        {
            SpawnItem(ref _xToyInstance, _xToyPrefab, _xToyPosition);
        }

        #endregion
    }
}
