using System.Collections.Generic;
using Entities.Events;
using Game.Entities;
using System;
using UnityEngine;

namespace Handlers
{
    /// <summary>
    /// This class caches all entities colliders so we can easily know if a collider is an entity or not
    /// </summary>
    public static class EntityColliderRegistry
    {
        private static readonly Dictionary<Collider, IEntity> Colliders = new();

        public static void Register(IEntity entity, Collider collider)
        {
            Colliders[collider] = entity;
        }

        public static bool IsEntity(Collider collider) =>
            Colliders.ContainsKey(collider);

        public static bool TryGet(Collider collider, out IEntity entity) =>
            Colliders.TryGetValue(collider, out entity);

        public static void TrySendEvent<T>(Collider collider, T evt) where T : struct, IEntityEvent
        {
            if (Colliders.TryGetValue(collider, out IEntity entity))
                entity.EventHandler.Send(evt);
        }

        public static void UnRegister(Collider collider)
        {
            Colliders.Remove(collider);
        }
    }
}
