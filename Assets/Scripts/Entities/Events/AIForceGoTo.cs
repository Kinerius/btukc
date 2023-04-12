using UnityEngine;

namespace Entities.Events
{
    public struct AIForceGoTo : IEntityEvent
    {
        public Vector3 WorldPosition;

        public AIForceGoTo(Vector3 worldPosition)
        {
            WorldPosition = worldPosition;
        }
    }
}