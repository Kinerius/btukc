using Cysharp.Threading.Tasks;
using Game.Level;
using SkillEffects;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    public class ApplyEffect : ScriptableAction
    {
        [SerializeField] private SkillEffectData skillEffect;
        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            if (data.targetEntity != null)
                levelManager.effectsManager.AddEffect(data, skillEffect);
        }
    }
}
