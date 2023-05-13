using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/Wait Time")]
    public class WaitTime : ScriptableAction
    {
        [SerializeField] private float timeInSeconds;
        public override UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken) =>
            UniTask.Delay(TimeSpan.FromSeconds(timeInSeconds));
    }
}
