using Stats;
using UnityEngine;

namespace SkillActions
{
    [CreateAssetMenu(menuName = "Skills/New Skill")]
    public class SkillActionData : ScriptableObject
    {
        [SerializeField] public StatPackageData abilityStats;
        [SerializeField] public ScriptableAction[] actions;
        public SkillAction AsSkillAction()
        {
            return new SkillAction(this);
        }
    }
}