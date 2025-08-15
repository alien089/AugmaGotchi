using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine.States
{
    // Represents the Caress state of an entity, enabling the CaressComponent on enter and (likely intended) disabling on exit.
    public class EntityCaressState : State<EntityStates>
    {
        private EntityStateManager _xEntityStateManager;

        // Constructor linking this state to its state manager.
        public EntityCaressState(EntityStates stateID, StatesMachine<EntityStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (EntityStateManager)stateMachine;
        }

        // Enables the CaressComponent on entering the Caress state.
        public override void OnEnter()
        {
            base.OnEnter();
            _xEntityStateManager.xEntityController.XCaressComponent.EnableComponent(true);
        }

        // Called every frame while in the Caress state; currently invokes base behavior.
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        // Enables the CaressComponent on exiting the Caress state (likely intended to disable it).
        public override void OnExit()
        {
            base.OnExit();
            _xEntityStateManager.xEntityController.XCaressComponent.EnableComponent(false);
        }
    }
}
