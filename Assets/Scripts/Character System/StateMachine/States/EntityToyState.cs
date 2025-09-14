using System.Collections.Generic;
using Character_System.Entity_Toy_System.Toy_State_Machine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Managers;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;
using UnityEngine;
using UnityEngine.AI;

namespace Character_System.StateMachine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class EntityToyState : State<EntityStates>
    {
        private ToyStateManager _xToyStateManager;
        private OppyCharacterController _xOppyCharacterController;
        private EntityStateManager _xEntityStateManager;
        private Dictionary<ToyStates, bool> _xStateFlags = new Dictionary<ToyStates, bool>();

        private Vector3 _xToyPosition;
        private NavMeshAgent _xNavMeshAgent;
        
        public Vector3 XToyPosition { get => _xToyPosition; }
        public NavMeshAgent XNavMeshAgent { get => _xNavMeshAgent; }
        public EntityStateManager XEntityStateManager { get => _xEntityStateManager; }
        public OppyCharacterController XOppyCharacterController { get => _xOppyCharacterController; }
        
        // Constructor linking this state to its state manager.
        public EntityToyState(EntityStates stateID, OppyCharacterController characterController, StatesMachine<EntityStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (EntityStateManager)stateMachine;
            _xOppyCharacterController = characterController;
            
            _xToyStateManager = new ToyStateManager(this);
            _xToyStateManager.CurrentState = _xToyStateManager.StatesList[ToyStates.IDLE];
            
            GameManager.Instance.EventManager.Register(EntityEventList.CHANGE_TOY_STATE, SetFlag);
        }

        // Called when entering the Toy state.
        public override void OnEnter()
        {
            base.OnEnter();
            _xToyPosition = new Vector3(0, 0, 0);
            
            _xEntityStateManager.XEntityController.XToyComponent.EnableComponent(true);
            _xNavMeshAgent = _xEntityStateManager.XEntityController.XToyComponent.XNavAgent;
        }

        // Called every frame while in the Toy state.
        public override void OnUpdate()
        {
            base.OnUpdate();
            //TO DO: move to ball, grab the ball, return to player
            
            bool isTouching = SetState();
            _xToyStateManager.CurrentState.OnUpdate();
        }

        // Called when exiting the Toy state; reset toy position when return to idle state.
        public override void OnExit()
        {
            base.OnExit();
            
            _xEntityStateManager.XEntityController.XToyComponent.EnableComponent(false);
            _xNavMeshAgent = null;
            _xToyPosition = Vector3.zero;
        }
        
        private void SetFlag(object[] param)
        {
            // Extract the requested state from event parameters.
            ToyStates state = (ToyStates)param[0];
            if (state == ToyStates.MOVE) _xToyPosition = (Vector3)param[1];

            // Reset all state flags before setting the new active state.
            _xStateFlags[ToyStates.GRAB] = false;
            _xStateFlags[ToyStates.MOVE] = false;
            _xStateFlags[ToyStates.RETURN] = false;
            _xStateFlags[ToyStates.IDLE] = false;

            // Activate the requested state.
            _xStateFlags[state] = true;
        }
        
        private bool SetState()
        {
            if (_xStateFlags[ToyStates.MOVE])
            {
                _xToyStateManager.ChangeState(ToyStates.MOVE);
                return true;
            }
            if (_xStateFlags[ToyStates.GRAB])
            {
                _xToyStateManager.ChangeState(ToyStates.GRAB);
                return true;
            }
            if (_xStateFlags[ToyStates.RETURN])
            {
                _xToyStateManager.ChangeState(ToyStates.RETURN);
                return true;
            }
            _xToyStateManager.ChangeState(ToyStates.IDLE);
            return false;
        }
    }
}
