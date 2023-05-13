using Game;
using System;
using UI.HealthBars;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HealthBarsManager healthBarsManager;
        private PoolManager poolManager;

        public void Setup(PoolManager poolManager, GlobalClock globalClock)
        {
            this.poolManager = poolManager;

            healthBarsManager.Setup(poolManager, globalClock);
        }

        public void RegisterEntity(Entity entity)
        {
            healthBarsManager.RegisterEntity(entity);
        }

        public void UnRegisterEntity(Entity entity)
        {
            healthBarsManager.UnRegisterEntity(entity);
        }
    }
}
