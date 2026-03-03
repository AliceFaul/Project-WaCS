using UnityEngine;
using UnityEngine.AI;

namespace _Project.Gameplay.Customer
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CustomerMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;

        public bool IsMoving => _agent.hasPath &&
            _agent.remainingDistance > _agent.stoppingDistance;
        public bool HasReachDestination => !_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 position)
        {
            _agent.SetDestination(position);
        }

        public void Stop()
        {
            _agent.ResetPath();
        }
    }
}
