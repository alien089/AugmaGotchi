using System;
using System.Collections.Generic;
using System.Linq;
using Character_System.Entity_Caress_System;
using Character_System.Entity_Food_System;
using Character_System.StateMachine;
using Enums;
using Managers;
using UnityEngine;

namespace Character_System
{
    // Controls entity state management, component instantiation, and state transitions during runtime.
    public class EntityController : MonoBehaviour
    {
        [SerializeField] private GameObject _xFoodComponentPrefab;
        [SerializeField] private GameObject _xCaressComponentPrefab;

        private EntityStateManager _xEntityStateManager;
        private Dictionary<EntityStates, bool> _mStateFlags = new Dictionary<EntityStates, bool>();

        private FoodComponent _xFoodComponent;
        private CaressComponent _xCaressComponent;

        public FoodComponent XFoodComponent { get => _xFoodComponent; }
        public CaressComponent XCaressComponent { get => _xCaressComponent; }

        // Initializes state manager, event registrations, initial flags, and component instantiation.
        private void Start()
        {
            // Initialize state manager and set initial state to IDLE.
            _xEntityStateManager = new EntityStateManager(this);
            _xEntityStateManager.CurrentState = _xEntityStateManager.StatesList[EntityStates.IDLE];

            // Register to listen for state change events.
            GameManager.Instance.EventManager.Register(EntityEventList.CHANGE_Entity_STATE, SetFlag);

            // Initialize state flags.
            _mStateFlags.Add(EntityStates.TOY, false);
            _mStateFlags.Add(EntityStates.FOOD, false);
            _mStateFlags.Add(EntityStates.CARESS, false);
            _mStateFlags.Add(EntityStates.IDLE, true);

            // Instantiate Food and Caress component prefabs as children.
            Instantiate(_xFoodComponentPrefab, transform.position, Quaternion.identity).transform.SetParent(transform);
            Instantiate(_xCaressComponentPrefab, transform.position, Quaternion.identity).transform.SetParent(transform);

            // Cache references to the instantiated components.
            _xFoodComponent = GetComponentInChildren<FoodComponent>();
            _xCaressComponent = GetComponentInChildren<CaressComponent>();
        }

        // Updates current state each frame and calls its OnUpdate.
        private void Update()
        {
            // Evaluate and set the current state based on flags.
            bool isTouching = SetState();

            // Call OnUpdate on the current state.
            _xEntityStateManager.CurrentState.OnUpdate();
        }

        // Handles incoming state change events and updates flags accordingly.
        private void SetFlag(object[] param)
        {
            // Extract the requested state from event parameters.
            EntityStates state = (EntityStates)param[0];

            // Reset all state flags before setting the new active state.
            _mStateFlags[EntityStates.TOY] = false;
            _mStateFlags[EntityStates.FOOD] = false;
            _mStateFlags[EntityStates.CARESS] = false;
            _mStateFlags[EntityStates.IDLE] = false;

            // Activate the requested state.
            _mStateFlags[state] = true;
        }

        // Evaluates flags and transitions to the appropriate state, with priority order.
        private bool SetState()
        {
            // Priority: Joy state.
            if (_mStateFlags[EntityStates.TOY])
            {
                _xEntityStateManager.ChangeState(EntityStates.TOY);
                return true;
            }
            // Priority: Food state.
            if (_mStateFlags[EntityStates.FOOD])
            {
                _xEntityStateManager.ChangeState(EntityStates.FOOD);
                return true;
            }
            // Priority: Caress state.
            if (_mStateFlags[EntityStates.CARESS])
            {
                _xEntityStateManager.ChangeState(EntityStates.CARESS);
                return true;
            }
            // Default fallback: Idle state.
            _xEntityStateManager.ChangeState(EntityStates.IDLE);
            return false;
        }

        // Cleans up event registration on application quit to prevent leaks.
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(EntityEventList.CHANGE_Entity_STATE, SetFlag);
        }
    }
}
