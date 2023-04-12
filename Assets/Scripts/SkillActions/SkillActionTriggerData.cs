using Entities;
using Game;
using Game.Entities;
using JetBrains.Annotations;
using UnityEngine;

namespace SkillActions
{
    public struct SkillActionTriggerData
    {
        public IEntity owner;
        public EntityView view;
        public Vector3 targetPosition;

        [CanBeNull] public IEntity targetEntity;

        public void Reset()
        {

        }
    }
}
