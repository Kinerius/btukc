using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Entities;
using Handlers;
using UnityEngine;
using Utils;

#pragma warning disable CS4014

namespace SkillActions.Views
{
    public class Projectile : PooledObject
    {
        [SerializeField] private Rigidbody rigidbody;
        private int layerToIgnore;
        private Cooldown destroyCooldown;
        private CancellationTokenSource cancellationTokenSource;
        private Action<CollisionEvent> onCollisionEvent;

        public void Setup(Vector3 initialPosition, float speed, Vector3 direction, int layerToIgnore, GlobalClock globalClock, Action<CollisionEvent> onCollisionEvent)
        {
            rigidbody.isKinematic = true;
            transform.position = initialPosition;
            this.onCollisionEvent = onCollisionEvent;
            this.layerToIgnore = layerToIgnore;

            destroyCooldown = new Cooldown(globalClock, TimeSpan.FromSeconds(3), true);
            cancellationTokenSource = new CancellationTokenSource();

            DestroyAfterTime(cancellationTokenSource.Token);

            rigidbody.isKinematic = false;
            rigidbody.velocity = speed * direction;
            transform.forward = direction;
        }

        private async UniTask DestroyAfterTime(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => destroyCooldown.IsUsable(), cancellationToken: cancellationToken);
            ReturnToPool();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (LayerUtils.AreNotEqual(collision.gameObject.layer, layerToIgnore)
                && LayerUtils.AreNotEqual(collision.gameObject.layer, gameObject.layer))
            {
                if (EntityHandler.TryGet(collision.collider, out var targetEntity))
                {
                    onCollisionEvent.Invoke(new CollisionEvent {entity = targetEntity, point = collision.contacts[0].point});
                }
                else
                {
                    onCollisionEvent.Invoke(new CollisionEvent {point = collision.contacts[0].point});
                }

                ReturnToPool();
            }
        }

        public override void ReturnToPool()
        {
            cancellationTokenSource.Cancel();
            base.ReturnToPool();
        }

        private void OnDestroy()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }

    public struct CollisionEvent
    {
        public IEntity entity;
        public Vector3 point;
    }
}
