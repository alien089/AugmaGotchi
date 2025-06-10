using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Managers;
using UnityEngine;

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
        
        if (_xToyPrefab == null || _xToyPosition == null) return;
        SpawnItem(ref _xToyInstance, _xToyPrefab, _xToyPosition);
        
        GameManager.Instance.EventManager.Register(FoodEventList.FOOD_UNGRABBED, FoodUngrabbed);
        GameManager.Instance.EventManager.Register(ToyEventList.TOY_UNGRABBED, ToyUngrabbed);
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
    }
    
    private void FoodUngrabbed(object[] param)
    {
        SpawnItem(ref _xFoodInstance, _xFoodPrefab, _xFoodPosition);
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
