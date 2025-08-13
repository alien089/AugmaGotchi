using System.Numerics;
using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class EntityToyState : State<EntityStates>
    {
        private EntityStateManager _xEntityStateManager;
        private Vector3 _xToyPosition; 

        // Constructor linking this state to its state manager.
        public EntityToyState(EntityStates stateID, StatesMachine<EntityStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (EntityStateManager)stateMachine;
        }

        // Called when entering the Toy state.
        public override void OnEnter()
        {
            base.OnEnter();
            _xToyPosition = new Vector3(0, 0, 0);
            //TO DO: obtain ball position
        }

        // Called every frame while in the Toy state.
        public override void OnUpdate()
        {
            base.OnUpdate();
            Vector3 toyPosition = new Vector3(0, 0, 0);
            //TO DO: move to ball, grab the ball, return to player
        }

        // Called when exiting the Toy state; reset toy position when return to idle state.
        public override void OnExit()
        {
            base.OnExit();
            _xToyPosition = Vector3.Zero;
        }
    }
}
