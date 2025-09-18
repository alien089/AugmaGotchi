using System.Collections;
using System.Numerics;
using Character_System.StateMachine;
using Enums;
using Framework.Generics.Pattern.StatePattern;
using Managers;
using Meta.XR.MRUtilityKitSamples.PassthroughRelighting;
using UnityEngine;
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

        private bool _bHasStartedLink;
        
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
            _xNavMeshAgent.stoppingDistance = _xToyStateManager.XToyController.XEntityStateManager.XEntityController.FMoveStoppingDistance;
            _xNavMeshAgent.autoTraverseOffMeshLink = false;
            _xOppyCharacterController.EnableAnimator();
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
            else if (_xNavMeshAgent.isOnOffMeshLink && !_bHasStartedLink)
            {
                // Siamo al punto di ingresso del link
                _bHasStartedLink = true;
                _xToyStateManager.XToyController.XEntityStateManager.XEntityController.StartCoroutine(TraverseLink(_xNavMeshAgent.currentOffMeshLinkData));
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
        
        private IEnumerator TraverseLink(OffMeshLinkData data)
        {
            _xOppyCharacterController.SetAnimation("Running", false);
            
            _xOppyCharacterController.TriggerAnimation("Jumping");

            Vector3 start = _xNavMeshAgent.transform.position;
            Vector3 end   = data.endPos;

            float duration = 1f;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;

                Vector3 pos = Vector3.Lerp(start, end, t);
                pos.y += Mathf.Sin(Mathf.PI * t);
                _xNavMeshAgent.transform.position = pos;

                yield return null;
            }

            _xOppyCharacterController.TriggerAnimation("Landed");

            _xNavMeshAgent.CompleteOffMeshLink();

            _bHasStartedLink = false;
        }
    }
}
