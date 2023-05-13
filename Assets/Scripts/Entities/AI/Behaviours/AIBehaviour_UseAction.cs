using System;
using Entities;
using Entities.Events;
using Game.Entities;
using SkillActions;
using UnityEngine;

namespace AI.Behaviours
{
    [CreateAssetMenu(menuName = "AI/Use Action")]
    public class AIBehaviour_UseAction : AIBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private float cooldownInSeconds;
        [SerializeField] private SkillActionData action;
        private IEntity nearbyPlayer;
        private Cooldown attackCooldown;
        private SkillAction skillAction;

        protected override void OnSetup()
        {
            attackCooldown = new Cooldown(levelManager.globalClock, TimeSpan.FromSeconds(cooldownInSeconds));
            thisEntity.EventHandler.On<PlayerNearbyEvent>(OnPlayerNearby);
            skillAction = action.AsSkillAction();
            skillAction.Setup(levelManager.globalClock);
        }

        public override void Activate()
        {

        }

        public override void Disable()
        {

        }

        private void OnPlayerNearby(PlayerNearbyEvent obj)
        {
            nearbyPlayer = obj.PlayerEntity;
        }

        public override void Update()
        {
            if (nearbyPlayer != null)
            {
                agent.MoveTowards(nearbyPlayer.GetPosition());
            }

            if (Vector3.Distance(thisEntity.GetPosition(), nearbyPlayer.GetPosition()) < distance)
            {
                agent.LookAtInstant(nearbyPlayer.GetPosition());

                thisEntity.StartAction(skillAction, nearbyPlayer);

                attackCooldown.Use();
                RaiseBehaviourCheck();
            }
        }

        public override bool IsActive()
        {
            if (!attackCooldown.IsUsable()) return false;
            if (nearbyPlayer == null) return false;
            return true;
        }

    }
}
