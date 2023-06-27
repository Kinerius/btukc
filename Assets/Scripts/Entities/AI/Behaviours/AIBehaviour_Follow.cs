using Entities;
using Entities.Events;
using Game.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Behaviours
{
    [CreateAssetMenu(menuName = "AI/Follow Player")]
    public class AIBehaviour_Follow : AIBehaviour
    {
        public float minDistance = 0;
        public float maxDistance = 0;
        public bool circleAround = false;
        public float circleAroundForwardAnticipation = 1;

        private IEntity targetEntity;
        private Vector3? forcedPosition;
        private bool circleAroundClockWise;

        private NavMeshQueryFilter navMeshQueryFilter;

        protected override void OnSetup()
        {
            navMeshQueryFilter = new NavMeshQueryFilter
            {
                agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID,
                areaMask = 1 << NavMesh.GetAreaFromName("Walkable")
            };

            thisEntity.EventHandler.On<PlayerNearbyEvent>(OnPlayerNearby);
            thisEntity.EventHandler.On<AIForceGoTo>(OnForceGoto);
        }

        public override void Activate()
        {
            circleAroundClockWise = Random.value > 0.5f;
        }

        public override void Disable()
        {

        }

        private void OnForceGoto(AIForceGoTo forceGoTo)
        {
            forcedPosition = forceGoTo.WorldPosition;
        }

        // TODO: we have to unregister the player on death
        private void OnPlayerNearby(PlayerNearbyEvent playerNearbyEvent)
        {
            forcedPosition = null;
            targetEntity = playerNearbyEvent.PlayerEntity;
        }

        public override void Update()
        {
            if (targetEntity != null)
            {
                var targetPosition = targetEntity.GetPosition();
                var inverseDirection = (thisEntity.GetPosition() - targetPosition).normalized;
                var distance = Mathf.Lerp(minDistance, maxDistance, Random.value);
                var minPosition = targetPosition + inverseDirection * distance;

                // if we want to circle around we go towards the tangent
                if (circleAround)
                {
                    var direction = circleAroundClockWise ? 1 : -1;
                    var cross = Vector3.Cross(inverseDirection, Vector3.up);
                    minPosition += cross * circleAroundForwardAnticipation * direction;

                    if (!NavMesh.SamplePosition(minPosition, out var sampleHit, 1, navMeshQueryFilter))
                    {
                        SwitchCircleAroundDirection();
                    }

                }

                agent.MoveTowards(minPosition);

                return;
            }

            if (forcedPosition != null)
            {
                agent.MoveTowards(forcedPosition.Value);
                if (Vector3.Distance(forcedPosition.Value, thisEntity.GetPosition()) < 1) forcedPosition = null;

                return;
            }
        }

        public override bool IsActive() =>
            true;

        private void SwitchCircleAroundDirection()
        {
            circleAroundClockWise = !circleAroundClockWise;
        }
    }
}
