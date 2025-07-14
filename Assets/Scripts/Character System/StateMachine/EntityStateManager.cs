using Character_System.StateMachine.States;
using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine
{
    // Manages entity states and initializes all possible entity states.
    public class EntityStateManager : StatesMachine<EntityStates>
    {
        public EntityController xEntityController;

        // Constructor that links the state manager to its entity controller.
        public EntityStateManager(EntityController controller) : base()
        {
            xEntityController = controller;
        }

        // Initializes the dictionary of states with specific entity state instances.
        protected override void InitStates()
        {
            StatesList.Add(EntityStates.IDLE, new EntityIdleState(EntityStates.IDLE, this));
            StatesList.Add(EntityStates.JOY, new EntityJoyState(EntityStates.JOY, this));
            StatesList.Add(EntityStates.FOOD, new EntityFoodState(EntityStates.FOOD, this));
            StatesList.Add(EntityStates.CARESS, new EntityCaressState(EntityStates.CARESS, this));
        }
    }
}
