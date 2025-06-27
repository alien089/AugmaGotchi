using System;
using System.Collections.Generic;
using System.Linq;
using Character_System.Caress_System;
using Character_System.Food_System;
using Character_System.StateMachine;
using Enums;
using Managers;
using UnityEngine;

namespace Character_System
{
    public class AugmaController : MonoBehaviour
    {
        [SerializeField] private GameObject _xFoodComponentPrefab;
        [SerializeField] private GameObject _xCaressComponentPrefab;
        
        private AugmaStateManager _xAugmaStateManager;
        
        private Dictionary<AugmaStates, bool> _mStateFlags = new Dictionary<AugmaStates, bool>();
        
        private FoodComponent _xFoodComponent;
        private CaressComponent _xCaressComponent;
        
        public FoodComponent XFoodComponent { get => _xFoodComponent; }
        public CaressComponent XCaressComponent { get => _xCaressComponent; }

        private void Start()
        {
            _xAugmaStateManager = new AugmaStateManager(this);
            _xAugmaStateManager.CurrentState = _xAugmaStateManager.StatesList[AugmaStates.IDLE];
            
            GameManager.Instance.EventManager.Register(AugmaEventList.CHANGE_AUGMA_STATE, SetFlag);
            
            _mStateFlags.Add(AugmaStates.JOY, true);
            _mStateFlags.Add(AugmaStates.FOOD, false);
            _mStateFlags.Add(AugmaStates.CARESS, true);

            Instantiate(_xFoodComponentPrefab, transform.position, Quaternion.identity).transform.SetParent(transform);
            Instantiate(_xCaressComponentPrefab, transform.position, Quaternion.identity).transform.SetParent(transform);
            
            _xFoodComponent = GetComponentInChildren<FoodComponent>();
            _xCaressComponent = GetComponentInChildren<CaressComponent>();
        }

        private void Update()
        {
            bool isTouching = SetState();
            _xAugmaStateManager.CurrentState.OnUpdate();
        }

        private void SetFlag(object[] param)
        {
            AugmaStates state = (AugmaStates)param[0];
            
            _mStateFlags[AugmaStates.JOY] = false;
            _mStateFlags[AugmaStates.FOOD] = false;
            _mStateFlags[AugmaStates.CARESS] = false;
            _mStateFlags[AugmaStates.IDLE] = false;

            _mStateFlags[state] = true;
        }
        
        private bool SetState()
        {
            //Change state to Joy State
            if (_mStateFlags[AugmaStates.JOY])
            {
                _xAugmaStateManager.ChangeState(AugmaStates.JOY);
                return true;
            }
            //Change state to Food State
            if (_mStateFlags[AugmaStates.FOOD])
            {
                _xAugmaStateManager.ChangeState(AugmaStates.FOOD);
                return true;
            }
            //Change state to Caress State
            if (_mStateFlags[AugmaStates.CARESS])
            {
                _xAugmaStateManager.ChangeState(AugmaStates.CARESS);
                return true;
            }
            //Change state to Idle State if every bools are false
            _xAugmaStateManager.ChangeState(AugmaStates.IDLE);
            return false;
        }
        
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(AugmaEventList.CHANGE_AUGMA_STATE, SetFlag);
        }
    }
}
