using Cysharp.Threading.Tasks;
using Game.Level;
using SkillActions.Views;
using Stats;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillActions.Actions
{
    public class SpawnSimpleProjectile : ScriptableAction
    {
        [SerializeField] private Projectile prefabGameObject;

        [SerializeField] private string anchorTag;

        [ActionsEditor][SerializeField] private CompositeScriptableAction onProjectileHit;

        public override UniTask StartAction(SkillActionTriggerData data,
            LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            Transform anchorTransform = data.view.GetAnchor(anchorTag);
            var anchorTransformPosition = anchorTransform.position;

            var instance = levelManager.poolManager.GetInactiveObject(prefabGameObject);
            instance.transform.position = anchorTransformPosition;

            var dataTargetPosition = data.targetPosition;
            var dataOwnerPosition = data.owner.GetPosition();
            dataTargetPosition.y = anchorTransformPosition.y;
            dataOwnerPosition.y = anchorTransformPosition.y;
            var targetDirection = (dataTargetPosition - dataOwnerPosition).normalized;

            var projectileSpeed = skillStats.GetOrAddStat("ProjectileSpeed");

            async void OnCollisionEvent(CollisionEvent evt)
            {
                if (evt.entity == null) return;
                var modifiedData = data;
                modifiedData.targetEntity = evt.entity;
                await onProjectileHit.StartAction(modifiedData, levelManager, skillStats, cancellationToken);
            }

            instance.Setup(anchorTransformPosition, projectileSpeed.Value, targetDirection, data.owner.GetLayer(), levelManager.globalClock, OnCollisionEvent);
            instance.gameObject.SetActive(true);

            return UniTask.CompletedTask;
        }
    }
}
