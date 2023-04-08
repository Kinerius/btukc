using UnityEngine;

namespace Game
{
    public abstract class EntityController : ScriptableObject
    {
        public abstract void Initialize(Entity entity);
        public abstract void Update();
    }
}