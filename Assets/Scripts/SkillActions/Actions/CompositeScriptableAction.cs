using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/Composite", order = 0)]
    public class CompositeScriptableAction : ScriptableAction
    {
        [SerializeField] private ScriptableAction[] actions;

        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            foreach (ScriptableAction action in actions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await action.StartAction(data, levelManager, skillStats, cancellationToken);
            }
        }
    }
}
