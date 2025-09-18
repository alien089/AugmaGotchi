using System.Numerics;
using Character_System.StateMachine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Managers;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;
using UnityEngine;

namespace Character_System.Entity_Toy_System.Toy_State_Machine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class ToyGrabState : State<ToyStates>
    {
        private ToyStateManager _xToyStateManager;
        
        // Constructor linking this state to its state manager.
        public ToyGrabState(ToyStates stateID, StatesMachine<ToyStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xToyStateManager = (ToyStateManager)stateMachine;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Transform transformEntity = _xToyStateManager.XToyController.XEntityStateManager.XEntityController.transform;
            GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_COLLECTED, transformEntity);
            
            GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_TOY_STATE, ToyStates.RETURN);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
