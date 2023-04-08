using Entities;
using Entities.Animation;
using Entities.Events;
using Game.Entities;
using Stats;
using System;
using UnityEngine;
using EventHandler = Entities.EventHandler;

namespace Game
{
    public class EntityView : MonoBehaviour, IEntity, IEntityAnimation
    {
        [SerializeField] private StatPackageData stats;
        [SerializeField] private EntityController entityController;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private EntityAnimationDataPack animations;

        private IStatService statService;
        private CharacterController characterController;
        private IReactiveStat<float> movementSpeed;
        private Quaternion lastLookRotation;
        private EventHandler eventHandler;

        public IStatService Stats => statService;
        public EventHandler EventHandler => eventHandler;

        private void Start()
        {
            statService = new StatService(stats.AsStatRepository());
            movementSpeed = statService.ObserveStat("MovementSpeed");
            eventHandler = new EventHandler();

            entityController.Initialize(this);
            characterController = GetComponent<CharacterController>();
            lastLookRotation = Quaternion.LookRotation(transform.forward);
            EntityAnimationController entityAnimationController = new EntityAnimationController()
        }

        private void Update()
        {
            entityController.Update();
        }

        public void Move(Vector3 movementDirection)
        {
            var finalDirection = movementDirection * movementSpeed.Value * Time.deltaTime;

            if (finalDirection.sqrMagnitude > 0)
                lastLookRotation = Quaternion.LookRotation(finalDirection);

            viewTransform.transform.rotation = Quaternion.RotateTowards(viewTransform.transform.rotation, lastLookRotation, 360 * 2 * Time.deltaTime);

            finalDirection.y = Physics.gravity.y * Time.deltaTime;
            characterController.Move(finalDirection);
        }

        public Vector3 GetPosition() => transform.position;

        public object GetAnimationController()
        {
            throw new NotImplementedException();
        }

        public void SetDefaultAnimation(string name)
        {
            throw new NotImplementedException();
        }

        public void Play(string name)
        {
            throw new NotImplementedException();
        }

        public void ReceiveDamage(DamageEvent payload)
        {
            throw new NotImplementedException();
        }
    }
}


