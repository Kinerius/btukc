using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/Toggle Entity Actions")]
    public class ToggleEntityActions : ScriptableAction
    {
        [SerializeField] private bool enabled;
        public override UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats)
        {
            data.owner.ToggleActions(enabled);
            return UniTask.CompletedTask;
        }
    }
}
