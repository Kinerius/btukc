using Game.Entities;
using Game.Level;
using UnityEngine;

namespace Game
{
    public abstract class EntityController : ScriptableObject
    {
        public abstract void Initialize(IEntity entity, LevelManager levelManager);
        public abstract void Update();

        public abstract void InterruptActions();
    }
}
