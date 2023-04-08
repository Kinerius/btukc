using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
#pragma warning disable CS4014

namespace Fx
{
    public class FxDestroyAfterFinish : PooledObject
    {
        [SerializeField] private ParticleSystem particleSystem;
        private Cooldown cooldown;

        public void Setup(GlobalClock globalClock)
        {
            cooldown = new Cooldown(globalClock, TimeSpan.FromSeconds(particleSystem.main.duration), true);
            DestroyTask();
        }

        private async UniTask DestroyTask()
        {
            await UniTask.WaitUntil(() => cooldown.IsUsable());
            ReturnToPool();
        }
    }
}