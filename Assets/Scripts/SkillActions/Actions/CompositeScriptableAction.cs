using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    public class CompositeScriptableAction : ScriptableAction
    {
        [SerializeField] public ScriptableAction[] actions = Array.Empty<ScriptableAction>();

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
