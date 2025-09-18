using System;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Character_System.Entity_Toy_System
{
    public class ToyComponent : MonoBehaviour
    {
        [SerializeField] private float fIncrementValue;
        
        private NavMeshAgent _xNavAgent;
        public NavMeshAgent XNavAgent { get => _xNavAgent; }
        public float FIncrementValue => fIncrementValue;
        
        // Start is called before the first frame update
        void Start()
        {
            _xNavAgent = transform.parent.GetComponent<NavMeshAgent>();
            _xNavAgent.enabled = false;
            
            GameManager.Instance.EventManager.Register(ToyEventList.TOY_GIVEN_CALL, ToyGiven);
        }

        private void OnDestroy()
        {
            GameManager.Instance.EventManager.Unregister(ToyEventList.TOY_GIVEN_CALL, ToyGiven);
        }

        public void EnableComponent(bool value)
        {
            _xNavAgent.enabled = value;
        }

        public void ToyGiven(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(ToyEventList.TOY_GIVEN, Stats.TOY, fIncrementValue);
        }
    }
}
