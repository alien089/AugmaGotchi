using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class EntityJoyState : State<EntityStates>
    {
        private EntityStateManager _xEntityStateManager;

        // Constructor linking this state to its state manager.
        public EntityJoyState(EntityStates stateID, StatesMachine<EntityStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (EntityStateManager)stateMachine;
        }

        // Called when entering the Joy state; currently invokes base behavior.
        public override void OnEnter()
        {
            base.OnEnter();
        }

        // Called every frame while in the Joy state; currently invokes base behavior.
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        // Called when exiting the Joy state; currently invokes base behavior.
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
