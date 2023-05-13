using Agent;
using AI;
using Entities;
using Game;
using Game.Entities;
using Game.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Controllers/AIEntityController", fileName = "AIEntityController", order = 0)]
    public class AIEntityController : EntityController
    {
        private const double AI_UPDATE_TIME_IN_SECONDS = 0.5;

        // todo: move this away to another component, we dont want to create multiple AI entity controllers just to define different behaviours
        // we need to inject the behaviours from its initialization
        [SerializeField] private AIBehaviourData[] behaviours;
        [SerializeField] private bool log;

        private List<AIBehaviourData> orderedBehaviours = new ();
        private Cooldown behaviourRecheck;
        private AIBehaviour currentBehaviour;
        private EntityAnimationController animation;
        private IEntity entity;

        public override void Initialize(IEntity entity, LevelManager levelManager)
        {
            this.entity = entity;
            animation = this.entity.GetView().GetAnimationController();
            behaviourRecheck = new Cooldown(levelManager.globalClock, TimeSpan.FromSeconds(AI_UPDATE_TIME_IN_SECONDS));
            orderedBehaviours.Clear();

            foreach (var data in behaviours)
            {
                var newInstance = data.CopyNewInstance();
                newInstance.behaviour.Setup(entity, animation, entity.GetView(), levelManager);
                newInstance.behaviour.OnRaiseBehaviourCheck += UpdatePriority;
                orderedBehaviours.Add(newInstance);
            }

            orderedBehaviours = orderedBehaviours.OrderByDescending(d => d.priority).ToList();
            UpdatePriority();
        }

        public override void Update()
        {
            if (!entity.IsActionsEnabled()) return;

            if (behaviourRecheck.IsUsable())
            {
                behaviourRecheck.Use();
                UpdatePriority();
            }

            if (currentBehaviour != null) { currentBehaviour.Update(); }
        }

        public override void InterruptActions()
        {
            if (log)
                Debug.Log($"[AIView] Interrupted", this);

            currentBehaviour?.Disable();
            currentBehaviour = null;
        }

        private void UpdatePriority()
        {
            var currentBehaviourIsActive = currentBehaviour != null && currentBehaviour.IsActive();

            foreach (AIBehaviourData aiBehaviourData in orderedBehaviours)
            {
                var aiBehaviour = aiBehaviourData.behaviour;
                var targetBehaviourIsActive = aiBehaviour.IsActive();

                if (aiBehaviour == currentBehaviour && currentBehaviourIsActive) { return; }

                if (!targetBehaviourIsActive) continue;

                if (currentBehaviour != null) { currentBehaviour.Disable(); }

                currentBehaviour = aiBehaviour;
                currentBehaviour.Activate();

                if (log)
                    Debug.Log($"[AIView] Activated {currentBehaviour.name}", this);

                return;
            }
        }
    }
}
