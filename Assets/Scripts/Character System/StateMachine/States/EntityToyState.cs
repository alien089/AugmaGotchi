using System.Collections.Generic;
using System.Numerics;
using Character_System.Entity_Toy_System.Toy_State_Machine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Managers;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;

namespace Character_System.StateMachine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class EntityToyState : State<EntityStates>
    {
        private EntityStateManager _xEntityStateManager;
        private Vector3 _xToyPosition; 
        
        private ToyStateManager _xToyStateManager;
        private Dictionary<ToyStates, bool> _xStateFlags = new Dictionary<ToyStates, bool>();
        
        private OppyCharacterController _xOppyCharacterController;
        
        // Constructor linking this state to its state manager.
        public EntityToyState(EntityStates stateID, OppyCharacterController characterController, StatesMachine<EntityStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (EntityStateManager)stateMachine;
            _xOppyCharacterController = characterController;
            
            _xToyStateManager = new ToyStateManager(this, _xOppyCharacterController);
            _xToyStateManager.CurrentState = _xToyStateManager.StatesList[ToyStates.MOVE];
            
            GameManager.Instance.EventManager.Register(EntityEventList.CHANGE_TOY_STATE, SetFlag);
        }

        // Called when entering the Toy state.
        public override void OnEnter()
        {
            base.OnEnter();
            _xToyPosition = new Vector3(0, 0, 0);
            
            //enable ToyComponent
            
            //TO DO: obtain ball position
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
            
            //disenable ToyComponent
            
            _xToyPosition = Vector3.Zero;
        }
        
        private void SetFlag(object[] param)
        {
            // Extract the requested state from event parameters.
            ToyStates state = (ToyStates)param[0];

            // Reset all state flags before setting the new active state.
            _xStateFlags[ToyStates.JUMP] = false;
            _xStateFlags[ToyStates.GRAB] = false;
            _xStateFlags[ToyStates.MOVE] = false;

            // Activate the requested state.
            _xStateFlags[state] = true;
        }
        
        private bool SetState()
        {
            if (_xStateFlags[ToyStates.JUMP])
            {
                _xToyStateManager.ChangeState(ToyStates.JUMP);
                return true;
            }
            if (_xStateFlags[ToyStates.GRAB])
            {
                _xToyStateManager.ChangeState(ToyStates.GRAB);
                return true;
            }
            _xToyStateManager.ChangeState(ToyStates.MOVE);
            return false;
        }
    }
}
