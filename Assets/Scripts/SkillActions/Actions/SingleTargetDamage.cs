using Cysharp.Threading.Tasks;
using Entities.Events;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/SingleTargetDamage")]
    public class SingleTargetDamage : ScriptableAction
    {
        public override UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager,
            StatRepository skillStats)
        {
            if (data.targetEntity != null)
            {
                long baseDamage = (long)skillStats.GetStat("damage")!.Value;
                data.targetEntity.ReceiveDamage(new DamageEvent(baseDamage, data.owner, data.targetPosition));
            }
            else
            {
                Debug.LogWarning("NO TARGET??");
            }

            return UniTask.CompletedTask;
        }
    }
}