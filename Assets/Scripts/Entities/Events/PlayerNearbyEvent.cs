using Game.Entities;

namespace Entities.Events
{
    public struct PlayerNearbyEvent : IEntityEvent
    {
        public readonly IEntity PlayerEntity;

        public PlayerNearbyEvent(IEntity playerEntity)
        {
            this.PlayerEntity = playerEntity;
        }
    }
}
