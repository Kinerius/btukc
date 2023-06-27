using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System.Linq;
using System.Threading;
using Tags;
using UnityEngine;

namespace SkillActions.Actions
{
    public class SkillCombo : ScriptableAction
    {
        [SerializeField] private SkillActionData[] skills;
        private SkillAction[] actions;

        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            TagService tagService = data.owner.Tags;

            actions ??= skills.Select(s =>
            {
                SkillAction skillAction = s.AsSkillAction();
                skillAction.Setup(levelManager.globalClock);
                return skillAction;
            }).ToArray();

            int comboHash = name.GetHashCode();
            int comboStep = tagService.GetStack(comboHash);

            var targetSkill = actions[comboStep];

            await targetSkill.StartSkillAction(data, levelManager);

            tagService.ApplyStack(comboHash);

            if (tagService.GetStack(comboHash) >= skills.Length)
                tagService.RemoveTag(comboHash);
        }
    }
}
