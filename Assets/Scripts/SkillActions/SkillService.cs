using Entities;
using System.Collections.Generic;
using Game.Entities;
using Game.Level;
using UnityEngine;

namespace SkillActions
{
    public class SkillService : ISkillService
    {
        private readonly LevelManager levelManager;
        private readonly IEntity owner;
        private readonly List<SkillAction> skills = new();
        private Queue<SkillActionTriggerData> dataPool = new();

        public SkillService(LevelManager levelManager)
        {
            this.levelManager = levelManager;

            for (int i = 0; i < 20; i++)
            {
                dataPool.Enqueue(new SkillActionTriggerData());
            }
        }

        private SkillActionTriggerData GetTriggerData()
        {
            if (dataPool.Count == 0)
            {
                dataPool.Enqueue(new SkillActionTriggerData());
            }

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

        public void StartSkillAction(int i, RaycastHit hit, IEntity owner, EntityView view)
        {
            if (skills.Count > 0 && i < skills.Count)
            {
                var skillAction = skills[i];
                var dataTargetPosition = hit.point;

                StartSkillAction(skillAction, owner, view, dataTargetPosition);
            }
        }

        public void StartSkillAction(SkillAction skillAction, IEntity owner, EntityView view, Vector3 dataTargetPosition)
        {
            var data = GetTriggerData();
            data.owner = owner;
            data.view = view;
            data.targetPosition = dataTargetPosition;
            skillAction.StartSkillAction(data, levelManager);
        }

        public void StartSkillAction(SkillAction skillAction, IEntity owner, EntityView view, IEntity target)
        {
            var data = GetTriggerData();
            data.owner = owner;
            data.view = view;
            data.targetPosition = target.GetPosition();
            data.targetEntity = target;
            skillAction.StartSkillAction(data, levelManager);
        }
    }
}
