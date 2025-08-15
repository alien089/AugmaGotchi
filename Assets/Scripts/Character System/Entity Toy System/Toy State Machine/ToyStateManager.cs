using Character_System.Entity_Toy_System.Toy_State_Machine.States;
using Character_System.StateMachine.States;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;

namespace Character_System.Entity_Toy_System.Toy_State_Machine
{
    // Manages entity states and initializes all possible entity states.
    public class ToyStateManager : StatesMachine<ToyStates>
    {
        public EntityToyState XToyController;
        private OppyCharacterController _xOppyCharacterController;
        
        // Constructor that links the state manager to its entity controller.
        public ToyStateManager(EntityToyState controller, OppyCharacterController characterController) : base()
        {
            XToyController = controller;
            _xOppyCharacterController = characterController;
        }

        // Initializes the dictionary of states with specific entity state instances.
        protected override void InitStates()
        {
            StatesList.Add(ToyStates.MOVE, new ToyMoveState(ToyStates.MOVE, _xOppyCharacterController, this));
            StatesList.Add(ToyStates.JUMP, new ToyJumpState(ToyStates.MOVE, this));
            StatesList.Add(ToyStates.GRAB, new ToyGrabState(ToyStates.MOVE, this));
        }
    }
}
