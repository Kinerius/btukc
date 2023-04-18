﻿using Cysharp.Threading.Tasks;
using Game.Level;
using SkillActions.Views;
using Stats;
using System;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/SpawnSimpleProjectile")]
    public class SpawnSimpleProjectile : ScriptableAction
    {
        [SerializeField] private Projectile prefabGameObject;

        [SerializeField] private string anchorTag;

        [SerializeField] private ScriptableAction onHit;

        public override UniTask StartAction(SkillActionTriggerData data,
            LevelManager levelManager, StatRepository skillStats)
        {
            Transform anchorTransform = data.view.GetAnchor(anchorTag);
            var anchorTransformPosition = anchorTransform.position;

            var instance = levelManager.poolManager.GetInactiveObject(prefabGameObject).GetComponent<Projectile>();
            instance.transform.position = anchorTransformPosition;

            var dataTargetPosition = data.targetPosition;
            var dataOwnerPosition = data.owner.GetPosition();
            dataTargetPosition.y = anchorTransformPosition.y;
            dataOwnerPosition.y = anchorTransformPosition.y;
            var targetDirection = (dataTargetPosition - dataOwnerPosition).normalized;

            var projectileSpeed = skillStats.GetOrAddStat("ProjectileSpeed");

            void OnCollisionEvent(CollisionEvent evt)
            {
                if (evt.entity == null) return;
                var modifiedData = data;
                modifiedData.targetEntity = evt.entity;
                onHit.StartAction(modifiedData, levelManager, skillStats);
            }

            instance.Setup(anchorTransformPosition, projectileSpeed.Value, targetDirection, data.owner.GetLayer(), levelManager.globalClock, OnCollisionEvent);
            instance.gameObject.SetActive(true);

            return UniTask.CompletedTask;
        }
    }
}
