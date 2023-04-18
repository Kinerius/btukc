using Entities;
using Stats;
using UnityEngine;

namespace Game.Entities
{
    public interface IEntity : IHasStats, IDamageable, IHasEventHandler
    {
        Vector3 GetPosition();

        int GetLayer();

        void StartAction(int indexAction);

        void ToggleActions(bool enabled);
    }
}
