using Cysharp.Threading.Tasks;
using Game;
using Game.Entities;
using SkillActions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SkillEffects
{
    public class SkillEffectManager : IDisposable
    {
        private readonly List<SkillEffectInstance> skillEffectInstances = new ();
        private readonly GlobalClock globalClock;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly List<SkillEffectInstance> effectsToDispose = new ();
        private Dictionary<IEntity, List<SkillEffectInstance>> skillEffectsByEntity = new ();

        public SkillEffectManager(GlobalClock globalClock)
        {
            cancellationTokenSource = new CancellationTokenSource();
            this.globalClock = globalClock;
            UpdateEffectsAsync(cancellationTokenSource.Token).Forget();
        }

        public void Dispose()
        {
            foreach (SkillEffectInstance effectInstance in skillEffectInstances)
                effectInstance.Dispose();

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        private async UniTask UpdateEffectsAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    await UniTask.Yield();

                    cancellationToken.ThrowIfCancellationRequested();

                    foreach (SkillEffectInstance instance in effectsToDispose)
                    {
                        skillEffectInstances.Remove(instance);
                        instance.Dispose();
                    }
                    effectsToDispose.Clear();

                    foreach (var effect in skillEffectInstances)
                    {
                        bool keepAlive = effect.Update(globalClock.deltaTime);

                        if (!keepAlive)
                        {
                            effect.Remove();
                            effectsToDispose.Add(effect);
                        }

                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return;
                Debug.LogException(e);
            }
        }

        public void DisposeAll(IEntity entity)
        {
            if (!skillEffectsByEntity.TryGetValue(entity, out List<SkillEffectInstance> skillEffectList)) return;

            if (skillEffectList == null) return;

            foreach (var instance in skillEffectList)
                effectsToDispose.Add(instance);
        }

        public void AddEffect(SkillActionTriggerData skillActionData, SkillEffectData skillEffect)
        {
            var targetEntity = skillActionData.targetEntity;
            if (targetEntity == null) return;

            var skillEffectInstance = new SkillEffectInstance(skillActionData, skillEffect);

            bool keepAlive = skillEffectInstance.Apply();

            if (!keepAlive)
            {
                skillEffectInstance.Remove();
                return;
            }

            skillEffectInstances.Add(skillEffectInstance);
            RegisterEffectFor(skillActionData.targetEntity, skillEffectInstance);
        }

        private void RegisterEffectFor(IEntity entity, SkillEffectInstance instance)
        {
            if (!skillEffectsByEntity.ContainsKey(entity))
                skillEffectsByEntity.Add(entity, new List<SkillEffectInstance>());

            skillEffectsByEntity[entity].Add(instance);
        }
    }

}
