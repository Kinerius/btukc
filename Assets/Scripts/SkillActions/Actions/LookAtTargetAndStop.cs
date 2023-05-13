using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/LookAtTargetAndStop")]

    public class LookAtTargetAndStop : ScriptableAction
    {
        public override UniTask StartAction(SkillActionTriggerData data,
            LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            data.view.StopMoving();
            data.view.LookAtInstant(data.targetPosition);
            return UniTask.CompletedTask;
        }
    }
}
