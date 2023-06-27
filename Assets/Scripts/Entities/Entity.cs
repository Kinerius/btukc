using Cysharp.Threading.Tasks;
using Entities;
using Entities.Events;
using Game.Entities;
using SkillActions;
using Stats;
using System;
using System.Collections.Generic;
using Tags;
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

        private readonly HashSet<string> listOfBlockers = new ();
        private IStatService statService;
        private EventHandler eventHandler;
        private SkillService skillService;
        private bool isActionEnabled;
        private IReactiveStat<float> health;
        private LevelView levelView;
        private TagService tags;

        public IStatService Stats => statService;
        public EventHandler EventHandler => eventHandler;
        public TagService Tags => tags;

        public IEntityView GetView() =>
            view;

        public int GetLayer() =>
            gameObject.layer;

        public bool IsActionsEnabled() =>
            isActionEnabled;

        public void StartAction(int indexAction)
        {
            if (!isActionEnabled) return;

            skillService.StartSkillAction(indexAction, new RaycastHit
                { point = transform.position + (view.transform.forward * 2) }, this, view).Forget();
        }

        public void StartAction(SkillAction action, IEntity target)
        {
            if (!isActionEnabled) return;

            skillService.StartSkillAction(action, this, view, target).Forget();
        }

        public void ToggleActions(bool enabled, string source)
        {
            // we make sure multiple sources of action togglers exist
            if (enabled)
                listOfBlockers.Remove(source);
            else
                listOfBlockers.Add(source);

            isActionEnabled = listOfBlockers.Count == 0;
            view.ToggleMovement(isActionEnabled);
        }

        public void Setup(LevelView levelView)
        {
            isActionEnabled = true;
            statService = new StatService(stats.AsStatRepository());
            SetupStats();
            eventHandler = new EventHandler();
            tags = new TagService();
            // TODO: Remove this ugly dependency
            this.levelView = levelView;
            var levelManager = this.levelView.LevelManager;
            skillService = new SkillService(levelManager);

            foreach (SkillActionData skillActionData in skills)
                skillService.AddSkill(skillActionData.AsSkillAction());

            // multiple entities might have the same controller so we split the reference
            entityController = Instantiate(entityController);
            entityController.Initialize(this, this.levelView.LevelManager);
            health.OnChanged += OnHealthChanged;

            this.levelView.RegisterEntity(this);
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
            // TODO: Change this into a faction flag
            // anything that is an entity can receive damage, we use layers to decide if they are friend or foe
            if (GetLayer() == payload.Entity.GetLayer()) return;

            health.Value -= payload.PreComputedDamage;
            //Debug.Log($"{name} hp: {health.Value} (-{payload.PreComputedDamage})");
        }

        private void OnHealthChanged(float statValue)
        {
            if (statValue <= 0)
                OnDeath();
        }

        private void OnDeath()
        {
            InterruptActions();
            levelView.UnRegisterEntity(this);

            Destroy(gameObject);
        }

        public void InterruptActions()
        {
            entityController.InterruptActions();
            skillService.InterruptActions();
        }

    }
}
