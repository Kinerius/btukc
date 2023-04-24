using System;
using Entities;
using Entities.Events;
using Game.Entities;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace AI.Behaviours
{
    [CreateAssetMenu(menuName = "AI/Run Away")]
    public class AIBehaviour_RunAway : AIBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private float randomAngle;
        [SerializeField] private float radius;
        [SerializeField] private float refreshPositionInSeconds;

        private IEntity targetEntity;
        private Vector3? targetPosition;
        private NavMeshQueryFilter navMeshQueryFilter;
        private Cooldown cooldown;
        private Cooldown failCooldown;
        protected override void OnSetup()
        {
            navMeshQueryFilter = new NavMeshQueryFilter
            {
                agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID,
                areaMask = 1 << NavMesh.GetAreaFromName("Walkable")
            };

            thisEntity.EventHandler.On<PlayerNearbyEvent>(OnPlayerNearby);

            cooldown = new Cooldown(levelManager.globalClock, TimeSpan.FromSeconds(refreshPositionInSeconds));
            failCooldown = new Cooldown(levelManager.globalClock, TimeSpan.FromSeconds(1));
        }

        public override void Activate()
        {
            TryGetNewTargetPosition();
        }

        private void TryGetNewTargetPosition()
        {
            CodeUtils.RecursiveOperation(SetTargetPosition, 8, () =>
            {
                Debug.LogError("Failed to get a new position");
                failCooldown.Use();
                RaiseBehaviourCheck();
            });
        }

        private bool SetTargetPosition()
        {
            var enemyPosition = targetEntity.GetPosition();
            var inverseDirection = (thisEntity.GetPosition() - enemyPosition).normalized;
            inverseDirection.y = 0;
            var rotationDirection = Random.value > 0.5f ? 1 : -1;

            var finalDirection = Quaternion.AngleAxis(Random.value * randomAngle * rotationDirection , Vector3.up) * inverseDirection;

            var samplePosition = enemyPosition + finalDirection * distance;
            var randomPoint = Random.insideUnitCircle;
            samplePosition.x += randomPoint.x * radius;
            samplePosition.z += randomPoint.y * radius;


            if (NavMesh.SamplePosition(samplePosition, out var sampleHit, 1, navMeshQueryFilter))
            {
                Debug.DrawLine(samplePosition, samplePosition + Vector3.up * 500, Color.green, 5f);

                targetPosition = sampleHit.position;
                return true;
            }
            else
            {
                Debug.DrawLine(samplePosition, samplePosition + Vector3.up * 500, Color.red, 5);
            }

            return false;
        }

        public override void Disable()
        {

        }

        private void OnPlayerNearby(PlayerNearbyEvent playerNearbyEvent)
        {
            targetEntity = playerNearbyEvent.PlayerEntity;
        }

        public override void Update()
        {
            if (targetEntity == null)
            {
                RaiseBehaviourCheck();
            }

            if (cooldown.IsUsable())
            {
                cooldown.Use();
                TryGetNewTargetPosition();
            }

            if (targetPosition != null)
                agent.MoveTowards(targetPosition.Value);
        }

        public override bool IsActive()
        {
            return targetEntity != null && failCooldown.IsUsable();
        }
    }
}
