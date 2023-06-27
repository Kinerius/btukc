using SkillActions;
using UnityEngine;

namespace SkillEffects
{
    public abstract class SkillEffectData : ScriptableObject
    {
        public abstract bool Apply(SkillActionTriggerData data);
        public abstract bool OnUpdate(float deltaTime);
        public abstract void Dispose();

        public abstract void Remove();
    }
}
