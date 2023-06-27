using Cysharp.Threading.Tasks;
using Entities.Events;
using Game.Level;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    public class SingleTargetDamage : ScriptableAction
    {
        public override UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager,
            StatRepository skillStats, CancellationToken cancellationToken)
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
