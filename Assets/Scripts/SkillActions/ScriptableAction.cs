using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions
{
    public abstract class ScriptableAction : ScriptableObject, IAction
    {
        public virtual async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats)
        {
            await UniTask.CompletedTask;
        }
    }
}
