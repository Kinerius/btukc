using Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentEntityView : EntityView
    {
        [HideInInspector] [SerializeField] private NavMeshAgent agent;

        private void OnValidate()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public override void OnAwake()
        {
            
        }

        public override void StopMoving()
        {
            agent.isStopped = true;
        }

        public override void LookAt(Vector3 position)
        {
            var transformPosition = transform.position;
            position.y = transformPosition.y;
            Quaternion rotation = Quaternion.LookRotation((position - transformPosition).normalized);
            transform.rotation = rotation;
        }

        public override void Teleport(Vector3 finalPosition)
        {
            agent.Warp(finalPosition);
        }

        public override void SetDestination(Vector3 position)
        {
            agent.destination = position;
        }
    }
}