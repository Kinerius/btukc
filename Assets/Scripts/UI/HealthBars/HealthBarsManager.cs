using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.HealthBars
{
    public class HealthBarsManager : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBarPrefab;
        private PoolManager poolManager;
        private Dictionary<Entity, HealthBar> healthBars;
        private GlobalClock globalClock;

        public void Setup(PoolManager poolManager, GlobalClock globalClock)
        {
            this.globalClock = globalClock;
            this.poolManager = poolManager;
            healthBars = new ();
        }

        public void RegisterEntity(Entity entity)
        {
            var healthBar = poolManager.GetInactiveObject(healthBarPrefab);

            healthBar.transform.SetParent(transform, false);
            healthBar.Setup(entity, globalClock);
            healthBar.gameObject.SetActive(true);
            healthBars[entity] = healthBar;
        }

        public void UnRegisterEntity(Entity entity)
        {
            var healthBar = healthBars[entity];
            healthBar.ReturnToPool();
            healthBars.Remove(entity);
        }
    }
}
