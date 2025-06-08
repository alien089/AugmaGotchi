using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine.States
{
    public class AugmaIdleState : State<AugmaStates>
    {
        private AugmaStateManager _xAugmaStateManager;

        public AugmaIdleState(AugmaStates stateID, StatesMachine<AugmaStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xAugmaStateManager = (AugmaStateManager)stateMachine;
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
