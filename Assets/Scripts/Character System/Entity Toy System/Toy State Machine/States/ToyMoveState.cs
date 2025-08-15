using System.Numerics;
using Character_System.StateMachine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;

namespace Character_System.Entity_Toy_System.Toy_State_Machine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class ToyMoveState : State<ToyStates>
    {
        private ToyStateManager _xEntityStateManager;
        private Vector3 _xToyPosition; 
        
        private OppyCharacterController _xOppyCharacterController;
        
        // Constructor linking this state to its state manager.
        public ToyMoveState(ToyStates stateID, OppyCharacterController characterController, StatesMachine<ToyStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (ToyStateManager)stateMachine;
            _xOppyCharacterController = characterController;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
