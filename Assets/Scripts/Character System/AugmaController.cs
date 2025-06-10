using System;
using System.Collections.Generic;
using System.Linq;
using Character_System.StateMachine;
using Enums;
using Managers;
using UnityEngine;

namespace Character_System
{
    public class AugmaController : MonoBehaviour
    {
        private AugmaStateManager _xAugmaStateManager;
        
        private Dictionary<AugmaStates, bool> _states = new Dictionary<AugmaStates, bool>();
        private bool _bIsGrabbingToy = false;
        private bool _bIsGrabbingFood = false;
        private bool _bIsPokingHand = false;

        private void Start()
        {
            _xAugmaStateManager = new AugmaStateManager();
            _xAugmaStateManager.CurrentState = _xAugmaStateManager.StatesList[AugmaStates.IDLE];
            
            GameManager.Instance.EventManager.Register(AugmaEventList.CHANGE_AUGMA_STATE, SetFlag);
            
            _states.Add(AugmaStates.JOY, true);
            _states.Add(AugmaStates.FOOD, false);
            _states.Add(AugmaStates.CARESS, true);
        }

        private void Update()
        {
            bool isTouching = SetState();
            _xAugmaStateManager.CurrentState.OnUpdate();
        }

        private void SetFlag(object[] param)
        {
            AugmaStates state = (AugmaStates)param[0];
            
            _states[AugmaStates.JOY] = false;
            _states[AugmaStates.FOOD] = false;
            _states[AugmaStates.CARESS] = false;

            _states[state] = true;
        }
        
        private bool SetState()
        {
            //Change state to Joy State
            if (_states[AugmaStates.JOY])
            {
                _xAugmaStateManager.ChangeState(AugmaStates.JOY);
                return true;
            }
            //Change state to Food State
            if (_states[AugmaStates.FOOD])
            {
                _xAugmaStateManager.ChangeState(AugmaStates.FOOD);
                return true;
            }
            //Change state to Caress State
            if (_states[AugmaStates.CARESS])
            {
                _xAugmaStateManager.ChangeState(AugmaStates.FOOD);
                return true;
            }
            //Change state to Idle State if every bools are false
            _xAugmaStateManager.ChangeState(AugmaStates.IDLE);
            return false;
        }
    }
}
