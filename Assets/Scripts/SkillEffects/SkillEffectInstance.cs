using SkillActions;
using System;
using Object = UnityEngine.Object;

namespace SkillEffects
{
    public class SkillEffectInstance
    {
        private SkillEffectData skillEffect;
        private SkillActionTriggerData data;

        public SkillEffectInstance(SkillActionTriggerData data, SkillEffectData skillEffect)
        {
            this.data = data;
            this.skillEffect = Object.Instantiate(skillEffect);
        }

        public bool Apply() =>
            skillEffect.Apply(data);

        public void Remove() =>
            skillEffect.Remove();

        public bool Update(float deltaTime) =>
            skillEffect.OnUpdate(deltaTime);

        public void Dispose()
        {
            skillEffect.Dispose();
        }
    }
}
