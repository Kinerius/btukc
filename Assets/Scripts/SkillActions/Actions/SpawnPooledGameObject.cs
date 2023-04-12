using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/SpawnGameObject")]
    public class SpawnPooledGameObject : ScriptableAction
    {
        [SerializeField] private PooledObject prefabGameObject;
        [SerializeField] private bool lookAtTarget;
        [SerializeField] private bool flattenRotation;
        
        [SerializeField] private string anchorTag;
        public override UniTask StartAction(SkillActionTriggerData data,
            LevelManager levelManager, StatRepository skillStats)
        {
            Transform anchorTransform = data.view.GetAnchor(anchorTag);
            var instance = levelManager.poolManager.GetInactiveObject(prefabGameObject);
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