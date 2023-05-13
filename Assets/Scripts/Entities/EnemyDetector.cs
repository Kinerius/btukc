using Entities.Events;
using Game;
using Handlers;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// This object is used by the player, so when it detects any enemy, it will notify them of your presence
    /// </summary>
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] private Entity entity;
        private PlayerNearbyEvent cachedEvent;

        private void Awake()
        {
            cachedEvent = new PlayerNearbyEvent(entity);
        }

        public void OnTriggerEnter(Collider other)
        {
            EntityColliderRegistry.TrySendEvent(other, cachedEvent);
        }
    }
}
