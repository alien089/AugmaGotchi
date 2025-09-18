using System.Numerics;
using Character_System.StateMachine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Managers;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Character_System.Entity_Toy_System.Toy_State_Machine.States
{
    // Represents the Joy state of an entity, currently with default behavior.
    public class ToyMoveState : State<ToyStates>
    {
        private ToyStateManager _xToyStateManager;
        
        private OppyCharacterController _xOppyCharacterController;
        
        private NavMeshAgent _xNavMeshAgent;
        
        // Constructor linking this state to its state manager.
        public ToyMoveState(ToyStates stateID, OppyCharacterController characterController, StatesMachine<ToyStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xToyStateManager = (ToyStateManager)stateMachine;
            if (_xToyStateManager == null) return;
            _xOppyCharacterController = _xToyStateManager.XOppyCharacterController;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _xNavMeshAgent = _xToyStateManager.XToyController.XNavMeshAgent;
            Vector3 pos = _xToyStateManager.XToyController.XToyPosition;
            _xNavMeshAgent.SetDestination(pos);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_xNavMeshAgent.pathPending) return;
            if (_xNavMeshAgent.remainingDistance <= _xNavMeshAgent.stoppingDistance)
            {
                _xOppyCharacterController.SetAnimation("Running", false);
                _xNavMeshAgent.ResetPath();
                GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_TOY_STATE, ToyStates.GRAB);
            }
            else if (_xNavMeshAgent.isOnOffMeshLink)
            {
                if (_xNavMeshAgent.currentOffMeshLinkData.linkType != OffMeshLinkType.LinkTypeJumpAcross) return;

                if (_xOppyCharacterController.GetJumpingState() == OppyCharacterController.JumpingState.Grounded)
                {
                    _xOppyCharacterController.SetJumpingState(OppyCharacterController.JumpingState.JumpStarted);
                    _xOppyCharacterController.TriggerAnimation("Jumping");
                }
                else if (_xOppyCharacterController.GetJumpingState() == OppyCharacterController.JumpingState.JumpedAndAirborne)
                {
                    _xOppyCharacterController.TriggerAnimation("Landed");
                    _xOppyCharacterController.SetJumpingState(OppyCharacterController.JumpingState.Grounded);
                }
            }
            else
            {
                _xOppyCharacterController.SetAnimation("Running", true);
            }
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _xOppyCharacterController.SetAnimation("Running", false);
        }
    }
}
