using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions
{
    public abstract class ScriptableAction : ScriptableObject, IAction
    {
        public abstract UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats);
    }
}