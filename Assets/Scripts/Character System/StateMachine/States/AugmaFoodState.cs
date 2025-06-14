using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine.States
{
    public class AugmaFoodState : State<AugmaStates>
    {
        private AugmaStateManager _xAugmaStateManager;

        public AugmaFoodState(AugmaStates stateID, StatesMachine<AugmaStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xAugmaStateManager = (AugmaStateManager)stateMachine;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _xAugmaStateManager.xAugmaController.XFoodComponent.EnableComponent(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _xAugmaStateManager.xAugmaController.XFoodComponent.EnableComponent(false);
        }
    }
}
