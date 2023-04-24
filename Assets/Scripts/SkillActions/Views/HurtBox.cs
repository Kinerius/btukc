

using Handlers;
using System;
using System.Collections.Generic;
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

        private void OnTriggerEnter(Collider other)
        {
            if (LayerUtils.AreNotEqual(other.gameObject.layer, layerToIgnore)
                && LayerUtils.AreNotEqual(other.gameObject.layer, gameObject.layer))
            {
                if (EntityHandler.TryGet(other, out var targetEntity))
                    onCollisionEvent.Invoke(new CollisionEvent {entity = targetEntity, point = gameObject.transform.position});
                else
                    onCollisionEvent.Invoke(new CollisionEvent {point = gameObject.transform.position});
            }
        }

        public void ToggleDebugView(bool showDebugBox)
        {

        }
    }
}
