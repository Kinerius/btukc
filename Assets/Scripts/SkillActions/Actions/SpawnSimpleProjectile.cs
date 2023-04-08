using Cysharp.Threading.Tasks;
using Game.Level;
using Projectiles;
using Stats;
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
            instance.gameObject.SetActive(true);
            instance.transform.position = anchorTransformPosition;

            var dataTargetPosition = data.targetPosition;
            var dataOwnerPosition = data.owner.GetPosition();
            dataTargetPosition.y = anchorTransformPosition.y;
            dataOwnerPosition.y = anchorTransformPosition.y;
            var targetDirection = (dataTargetPosition - dataOwnerPosition).normalized;

            var projectileSpeed = skillStats.GetOrAddStat("ProjectileSpeed");
            instance.Setup(projectileSpeed.Value, targetDirection, data.owner.GetLayer(), levelManager.globalClock, evt =>
            {
                if (evt.entity != null)
                {
                    var modifiedData = data;
                    modifiedData.targetEntity = evt.entity;
                    onHit.StartAction(modifiedData, levelManager, skillStats);
                }
            });
            
            return UniTask.CompletedTask;
        }
    }
}