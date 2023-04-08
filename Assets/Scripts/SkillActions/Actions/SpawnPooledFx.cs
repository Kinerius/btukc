﻿using Cysharp.Threading.Tasks;
using Fx;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/Spawn Fx")]
    public class SpawnPooledFx: ScriptableAction
    {
        [SerializeField] private FxDestroyAfterFinish prefabGameObject;
        [SerializeField] private bool lookAtTarget;
        [SerializeField] private bool flattenRotation;
        
        [SerializeField] private string anchorTag;
        public override UniTask StartAction(SkillActionTriggerData data,
            LevelManager levelManager, StatRepository skillStats)
        {
            Transform anchorTransform = data.view.GetAnchor(anchorTag);
            var instance = (FxDestroyAfterFinish)levelManager.poolManager.GetInactiveObject(prefabGameObject);
            instance.Setup(levelManager.globalClock);
            instance.gameObject.SetActive(true);
            instance.transform.position = anchorTransform.position;
            
            if (lookAtTarget)
            {
                var finalTarget = data.targetPosition;
                if (flattenRotation)
                {
                    finalTarget.y = anchorTransform.position.y;
                }
                instance.transform.rotation = Quaternion.LookRotation(finalTarget - anchorTransform.position);
            }
            else
            {
                instance.transform.rotation = Quaternion.identity;
            }
            
            return UniTask.CompletedTask;
        }
    }
}