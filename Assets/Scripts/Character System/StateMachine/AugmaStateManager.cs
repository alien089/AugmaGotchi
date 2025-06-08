using Character_System.StateMachine.States;
using Enums;
using Framework.Generics.Pattern.StatePattern;

namespace Character_System.StateMachine
{
    public class AugmaStateManager : StatesMachine<AugmaStates>
    {
        protected override void InitStates()
        {
            StatesList.Add(AugmaStates.IDLE, new AugmaIdleState(AugmaStates.IDLE, this));
            StatesList.Add(AugmaStates.JOY, new AugmaJoyState(AugmaStates.JOY, this));
            StatesList.Add(AugmaStates.FOOD, new AugmaFoodState(AugmaStates.FOOD, this));
            StatesList.Add(AugmaStates.CARESS, new AugmaCaressState(AugmaStates.CARESS, this));
        }
    }
}
