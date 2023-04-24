﻿using Cysharp.Threading.Tasks;
using Game.Level;
using SkillActions.Views;
using Stats;
using System;
using UnityEngine;

namespace SkillActions.Actions
{

    [CreateAssetMenu(menuName = "Skills/Actions/Spawn Fake Hurtbox")]
    public class SpawnFakeHurtBox : ScriptableAction
    {
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private Vector3 boxRelativePosition;
        [SerializeField] private string anchorTag;
        [SerializeField] private float durationInSeconds;
        [SerializeField] private HurtBox hurtBoxPrefab;

        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats)
        {
            Transform anchorTransform = data.view.GetAnchor(anchorTag);
            var instance = levelManager.poolManager.GetInactiveObject(hurtBoxPrefab).GetComponent<HurtBox>();

            var offsetX = anchorTransform.right * boxRelativePosition.x;
            var offsetY = anchorTransform.up * boxRelativePosition.y;
            var offsetZ = anchorTransform.forward * boxRelativePosition.z;
            Vector3 initialPosition = anchorTransform.position + offsetX + offsetY + offsetZ;

            // we can use the collision event for other gameplay mechanics here
            instance.Setup(initialPosition, anchorTransform.forward, boxSize, data.owner.GetLayer(), _ => { });

            instance.gameObject.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(durationInSeconds));

            instance.ReturnToPool();

            await UniTask.CompletedTask;
        }
    }
}
