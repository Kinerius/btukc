

using Handlers;
using System;
using UnityEngine;
using Utils;

namespace SkillActions.Views
{
    public class HurtBox : PooledObject
    {
        private int layerToIgnore;
        private Action<CollisionEvent> onCollisionEvent;

        public void Setup(Vector3 initialPosition, Vector3 forward, Vector3 size, int layerToIgnore, Action<CollisionEvent> onCollisionEvent)
        {
            var t = transform;
            t.position = initialPosition;
            t.forward = forward;
            t.localScale = size;

            this.onCollisionEvent = onCollisionEvent;
            this.layerToIgnore = layerToIgnore;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (LayerUtils.AreNotEqual(collision.gameObject.layer, layerToIgnore)
                && LayerUtils.AreNotEqual(collision.gameObject.layer, gameObject.layer))
            {
                if (EntityHandler.TryGet(collision.collider, out var targetEntity))
                    onCollisionEvent.Invoke(new CollisionEvent {entity = targetEntity, point = collision.contacts[0].point});
                else
                    onCollisionEvent.Invoke(new CollisionEvent {point = collision.contacts[0].point});

                ReturnToPool();
            }
        }

        public void ToggleDebugView(bool showDebugBox)
        {

        }
    }
}
