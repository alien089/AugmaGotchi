using System.Numerics;
using Character_System.StateMachine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;

namespace Character_System.Entity_Toy_System.Toy_State_Machine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class ToyIdleState : State<ToyStates>
    {
        private ToyStateManager _xToyStateManager;
        
        // Constructor linking this state to its state manager.
        public ToyIdleState(ToyStates stateID, StatesMachine<ToyStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xToyStateManager = (ToyStateManager)stateMachine;
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
