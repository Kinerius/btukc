using SkillActions.Actions;
using Stats;
using System;
using UnityEngine;

namespace SkillActions
{
    [CreateAssetMenu(menuName = "Skills/New Skill")]
    public class SkillActionData : ScriptableObject
    {
        [SerializeField] public StatPackageData abilityStats;
        [ActionsEditor] public CompositeScriptableAction actions;
        public SkillAction AsSkillAction() => new (this);
    }
}
