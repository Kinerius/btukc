using Entities;
using Stats;
using Tags;
using UnityEngine;

namespace Game.Entities
{
    public interface IEntity : IHasStats, IDamageable, IHasEventHandler, IHasTags
    {
        Vector3 GetPosition();
        int GetLayer();
        bool IsActionsEnabled();
        void StartAction(int indexAction);
        void StartAction(SkillAction action, IEntity target);
        void ToggleActions(bool enabled, string source);
        IEntityView GetView();
        void InterruptActions();
    }
}
