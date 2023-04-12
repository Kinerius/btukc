using Entities;
using Entities.Events;
using Game.Entities;
using SkillActions;
using Stats;
using System;
using UnityEngine;
using EventHandler = Entities.EventHandler;

namespace Game
{
    public class Entity : MonoBehaviour, IEntity
    {
        [SerializeField] private StatPackageData stats;
        [SerializeField] private SkillActionData[] skills;
        [SerializeField] private EntityController entityController;
        [SerializeField] private EntityView viewTransform;

        private IStatService statService;
        private CharacterController characterController;
        private IReactiveStat<float> movementSpeed;
        private Quaternion lastLookRotation;
        private EventHandler eventHandler;
        private SkillService skillService;

        public IStatService Stats => statService;
        public EventHandler EventHandler => eventHandler;
        public EntityAnimationController GetAnimationController() =>
            viewTransform.GetAnimationController();
        public int GetLayer() =>
            gameObject.layer;

        public void StartAction(int indexAction)
        {
            skillService.StartSkillAction(indexAction, new RaycastHit() { point = transform.position + (viewTransform.transform.forward * 2)}, this, viewTransform);
        }

        private void Start()
        {
            statService = new StatService(stats.AsStatRepository());
            movementSpeed = statService.ObserveStat("MovementSpeed");
            eventHandler = new EventHandler();

            // TODO: Remove this ugly dependency
            var levelManager = FindObjectOfType<LevelView>().LevelManager;
            skillService = new SkillService(levelManager);

            foreach (SkillActionData skillActionData in skills)
                skillService.AddSkill(skillActionData.AsSkillAction());

            entityController.Initialize(this);
            characterController = GetComponent<CharacterController>();
            lastLookRotation = Quaternion.LookRotation(transform.forward);
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

        public Vector3 GetPosition() =>
            transform.position;

        public void ReceiveDamage(DamageEvent payload)
        {
            throw new NotImplementedException();
        }
    }
}
