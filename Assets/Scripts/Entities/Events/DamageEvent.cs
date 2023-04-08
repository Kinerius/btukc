using Game.Entities;
using UnityEngine;

namespace Entities.Events
{
    public struct DamageEvent : IEntityEvent
    {
        public readonly long PreComputedDamage;
        public readonly IEntity Entity;
        public readonly Vector3 TargetPosition;

        public DamageEvent(long preComputedDamage, IEntity entity, Vector3 targetPosition)
        {
            this.PreComputedDamage = preComputedDamage;
            this.Entity = entity;
            this.TargetPosition = targetPosition;
        }
    }
}
