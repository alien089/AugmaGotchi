using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine.States
{
    // Represents the Food state of an entity, enabling and disabling the FoodComponent accordingly.
    public class EntityFoodState : State<EntityStates>
    {
        private EntityStateManager _xEntityStateManager;

        // Constructor linking this state to its state manager.
        public EntityFoodState(EntityStates stateID, StatesMachine<EntityStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xEntityStateManager = (EntityStateManager)stateMachine;
        }

        // Enables the FoodComponent on entering the Food state.
        public override void OnEnter()
        {
            base.OnEnter();
            _xEntityStateManager.xEntityController.XFoodComponent.EnableComponent(true);
        }

        // Called every frame while in the Food state; currently invokes base behavior.
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        // Disables the FoodComponent on exiting the Food state.
        public override void OnExit()
        {
            base.OnExit();
            _xEntityStateManager.xEntityController.XFoodComponent.EnableComponent(false);
        }
    }
}
