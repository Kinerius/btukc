using Cysharp.Threading.Tasks;
using Game.Entities;
using Game.Level;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions
{
    public abstract class ScriptableAction : ScriptableObject, IAction
    {
        public virtual async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            await UniTask.CompletedTask;
        }
    }
}
