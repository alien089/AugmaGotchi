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
    public class ToyReturnState : State<ToyStates>
    {
        private ToyStateManager _xToyStateManager;
        
        private OppyCharacterController _xOppyCharacterController;
        
        private NavMeshAgent _xNavMeshAgent;
        
        // Constructor linking this state to its state manager.
        public ToyReturnState(ToyStates stateID, StatesMachine<ToyStates> stateMachine = null) : base(stateID, stateMachine)
        {
            _xToyStateManager = (ToyStateManager)stateMachine;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _xNavMeshAgent = _xToyStateManager.XToyController.XNavMeshAgent;
            _xOppyCharacterController = _xToyStateManager.XOppyCharacterController;
            _xNavMeshAgent.SetDestination(GetDestination());
        }

        private Vector3 GetDestination()
        {
            Vector3 destination = PlayerController.Instance.FPlayerPosition.position;
            destination.y = 0f;
            return destination;
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            _xNavMeshAgent.SetDestination(GetDestination());
            
            if (_xNavMeshAgent.pathPending) return;
            if (_xNavMeshAgent.remainingDistance <= _xNavMeshAgent.stoppingDistance)
            {
                _xOppyCharacterController.DisableAnimator();
                _xOppyCharacterController.SetAnimation("Running", false);
                _xNavMeshAgent.ResetPath();
                GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_TOY_STATE, ToyStates.IDLE);
                GameManager.Instance.EventManager.TriggerEvent(EntityEventList.CHANGE_ENTITY_STATE, EntityStates.IDLE);
                GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_RETURNED);
            }
            
            if (_xNavMeshAgent.isOnOffMeshLink)
            {
                if (_xNavMeshAgent.currentOffMeshLinkData.linkType != OffMeshLinkType.LinkTypeJumpAcross) return;

                if (_xOppyCharacterController.GetJumpingState() == OppyCharacterController.JumpingState.Grounded && 
                    _xOppyCharacterController.IsGrounded())
                {
                    _xOppyCharacterController.SetJumpingState(OppyCharacterController.JumpingState.JumpStarted);
                    _xOppyCharacterController.TriggerAnimation("Jumping");
                }
                else if (_xOppyCharacterController.GetJumpingState() == OppyCharacterController.JumpingState.JumpedAndAirborne && 
                    _xOppyCharacterController.IsGrounded())
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
        }
    }
}
