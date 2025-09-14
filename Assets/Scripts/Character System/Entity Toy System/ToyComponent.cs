using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

namespace Character_System.Entity_Toy_System
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ToyComponent : MonoBehaviour
    {
        private NavMeshAgent _xNavAgent;
        public NavMeshAgent XNavAgent { get => _xNavAgent; }
        
        // Start is called before the first frame update
        void Start()
        {
            _xNavAgent = GetComponent<NavMeshAgent>();
            _xNavAgent.enabled = false;
        }
        
        public void EnableComponent(bool value)
        {
            _xNavAgent.enabled = value;
        }
    }
}
