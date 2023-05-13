using Cysharp.Threading.Tasks;
using Entities;
using System.Collections.Generic;
using Game.Entities;
using Game.Level;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SkillActions
{
    public class SkillService : ISkillService
    {
        private readonly LevelManager levelManager;
        private readonly IEntity owner;
        private readonly List<SkillAction> skills = new ();
        private Queue<SkillActionTriggerData> dataPool = new ();
        private SkillAction currentSkillAction;

        public SkillService(LevelManager levelManager)
        {
            this.levelManager = levelManager;

            for (int i = 0; i < 20; i++) { dataPool.Enqueue(new SkillActionTriggerData()); }
        }

        private SkillActionTriggerData GetTriggerData()
        {
            if (dataPool.Count == 0) { dataPool.Enqueue(new SkillActionTriggerData()); }

            var skillActionTriggerData = dataPool.Dequeue();
            skillActionTriggerData.Reset();
            return skillActionTriggerData;
        }

        public void AddSkill(SkillAction skillAction)
        {
            skills.Add(skillAction);
            skillAction.Setup(levelManager.globalClock);
        }

        public void RemoveSkill(SkillAction skillAction)
        {
            skills.Remove(skillAction);
        }

        public async UniTask StartSkillAction(int i, RaycastHit hit, IEntity owner, EntityView view)
        {
            if (skills.Count <= 0 || i >= skills.Count) return;
            if (currentSkillAction != null) return;
            var skillAction = skills[i];
            var dataTargetPosition = hit.point;
            await StartSkillAction(skillAction, owner, view, dataTargetPosition);
        }

        public async UniTask StartSkillAction(SkillAction skillAction, IEntity owner, EntityView view, Vector3 dataTargetPosition)
        {
            if (currentSkillAction != null) return;
            var data = GetTriggerData();
            data.owner = owner;
            data.view = view;
            data.targetPosition = dataTargetPosition;
            await InternalStartSkillaction(skillAction, data);
        }

        public async UniTask StartSkillAction(SkillAction skillAction, IEntity owner, EntityView view, IEntity target)
        {
            if (currentSkillAction != null) return;
            var data = GetTriggerData();
            data.owner = owner;
            data.view = view;
            data.targetPosition = target.GetPosition();
            data.targetEntity = target;
            await InternalStartSkillaction(skillAction, data);
        }

        private async Task InternalStartSkillaction(SkillAction skillAction, SkillActionTriggerData data)
        {
            currentSkillAction = skillAction;
            await skillAction.StartSkillAction(data, levelManager);
            currentSkillAction = null;
        }

        public void InterruptActions()
        {
            currentSkillAction?.Interrupt();
            currentSkillAction = null;
        }
    }
}
