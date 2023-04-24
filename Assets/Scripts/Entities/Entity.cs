using Agent;
using Entities;
using Entities.Events;
using Game.Entities;
using Handlers;
using SkillActions;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;
using EventHandler = Entities.EventHandler;

namespace Game
{
    public class Entity : MonoBehaviour, IEntity
    {
        [SerializeField] private StatPackageData stats;
        [SerializeField] private SkillActionData[] skills;
        [SerializeField] private EntityController entityController;
        [FormerlySerializedAs("viewTransform")] [SerializeField] private EntityView view;

        private IStatService statService;

        private EventHandler eventHandler;
        private SkillService skillService;
        private bool isActionEnabled;
        private IReactiveStat<float> health;

        public IStatService Stats => statService;
        public EventHandler EventHandler => eventHandler;

        public IEntityView GetView() => view;

        public int GetLayer() => gameObject.layer;

        public void StartAction(int indexAction)
        {
            if (!isActionEnabled) return;

            skillService.StartSkillAction(indexAction, new RaycastHit
                { point = transform.position + (view.transform.forward * 2) }, this, view);
        }

        public void StartAction(SkillAction action, IEntity target)
        {
            if (!isActionEnabled) return;

            skillService.StartSkillAction(action, this, view, target);
        }

        public void ToggleActions(bool enabled)
        {
            isActionEnabled = enabled;

            view.ToggleMovement(enabled);
        }

        public void Setup(LevelView levelView)
        {
            isActionEnabled = true;
            statService = new StatService(stats.AsStatRepository());
            SetupStats();
            eventHandler = new EventHandler();

            // TODO: Remove this ugly dependency
            var levelManager = levelView.LevelManager;
            skillService = new SkillService(levelManager);

            foreach (SkillActionData skillActionData in skills)
                skillService.AddSkill(skillActionData.AsSkillAction());

            // multiple entities might have the same controller so we split the reference
            entityController = Instantiate(entityController);
            entityController.Initialize(this, levelView.LevelManager);
            health.OnChanged += OnHealthChanged;

            EntityHandler.Register(this, GetComponent<Collider>());
        }

        private void SetupStats()
        {
            health = statService.ObserveStat("HP");
            health.Value = statService.GetStatTotalValue("HPMax");
        }

        private void Update()
        {
            if (!isActionEnabled) return;

            entityController.Update();
        }

        public Vector3 GetPosition() =>
            transform.position;

        public void ReceiveDamage(DamageEvent payload)
        {
            // anything that is an entity can receive damage, we use layers to decide if they are friend or foe
            if (GetLayer() == payload.Entity.GetLayer()) return;

            health.Value -= payload.PreComputedDamage;
            Debug.Log($"{name} hp: {health.Value} (-{payload.PreComputedDamage})");
        }

        private void OnHealthChanged(float statValue)
        {
            if (statValue <= 0)
                OnDeath();
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
