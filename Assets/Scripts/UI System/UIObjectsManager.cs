using Enums;
using Managers;
using UnityEngine;

namespace UI_System
{
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
    
        // Start is called before the first frame update
        void Start()
        {
            if (_xFoodPrefab == null || _xFoodPosition == null) return;
            SpawnItem(ref _xFoodInstance, _xFoodPrefab, _xFoodPosition);
        
            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_GRABBED, FoodGrabbed);
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_GRABBED, ToyGrabbed);
            GameManager.Instance.EventManager.Register(CaressEventList.WANT_CARESS, WantCaress);
        
            if (_xToyPrefab == null || _xToyPosition == null) return;
            SpawnItem(ref _xToyInstance, _xToyPrefab, _xToyPosition);
        
            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_UNGRABBED, FoodUngrabbed);
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_UNGRABBED, ToyUngrabbed);
            GameManager.Instance.EventManager.Register(CaressEventList.NOT_WANT_CARESS, NotWantCaress);
        }
    
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(FoodEventList.FOOD_GRABBED, FoodGrabbed);
            GameManager.Instance.EventManager.Unregister(ToyEventList.TOY_GRABBED, ToyGrabbed);
            GameManager.Instance.EventManager.Unregister(CaressEventList.WANT_CARESS, WantCaress);
        
            GameManager.Instance.EventManager.Unregister(FoodEventList.FOOD_UNGRABBED, FoodUngrabbed);
            GameManager.Instance.EventManager.Unregister(ToyEventList.TOY_UNGRABBED, ToyUngrabbed);
            GameManager.Instance.EventManager.Unregister(CaressEventList.NOT_WANT_CARESS, NotWantCaress);
        }
    
        private void SpawnItem(ref GameObject itemInstance, GameObject item, Transform position)
        {
            itemInstance = Instantiate(item, position);
            itemInstance.transform.SetParent(this.transform);
        }

        #region Food System

        private void FoodGrabbed(object[] param)
        {
            _xFoodInstance.transform.SetParent(null);
            GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.CHANGE_AUGMA_STATE, AugmaStates.FOOD);
        }
    
        private void FoodUngrabbed(object[] param)
        {
            SpawnItem(ref _xFoodInstance, _xFoodPrefab, _xFoodPosition);
            GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.CHANGE_AUGMA_STATE, AugmaStates.IDLE);
        }

        #endregion

        #region Caress System

        private void WantCaress(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.CHANGE_AUGMA_STATE, AugmaStates.CARESS);
        }
    
        private void NotWantCaress(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AugmaEventList.CHANGE_AUGMA_STATE, AugmaStates.IDLE);
        }

        #endregion

        #region Toy System

        private void ToyGrabbed(object[] param)
        {
            _xToyInstance.transform.SetParent(null);
        }
    
        private void ToyUngrabbed(object[] param)
        {
            SpawnItem(ref _xToyInstance, _xToyPrefab, _xToyPosition);
        }

        #endregion
    }
}
