using NUnit.Framework;
using UnityEngine;

namespace Stats.Editor.Tests
{
    public class StatRepositoryShould
    {
        private int baseHealth;
        private int maxHealth;
        private float healthMultiplier;

        [Test]
        public void Be_Initialized_Properly_With_Data()
        {
            StatPackageData packageData = ScriptableObject.CreateInstance<StatPackageData>();
            StatData hp = ScriptableObject.CreateInstance<StatData>();
            StatData hpMax = ScriptableObject.CreateInstance<StatData>();
            StatData hpMult = ScriptableObject.CreateInstance<StatData>();
            
            hp.max = hpMax;
            hpMax.multiplier = hpMult;
            hp.name = "hp";
            hpMax.name = "hpMax";
            hpMult.name = "hpMult";

            baseHealth = 500;
            maxHealth = 100;
            healthMultiplier = -50f;

            packageData.stats = new[]
            {
                new StatDataInitialization { initialValue = baseHealth, stat = hp },
                new StatDataInitialization { initialValue = maxHealth, stat = hpMax },
                new StatDataInitialization { initialValue = healthMultiplier, stat = hpMult },
            };

            StatRepository statRepository = new StatRepository(packageData);
            
            // it should be 50
            var expectedHealth = Mathf.Min(baseHealth, maxHealth * (100 + healthMultiplier) / 100);
            
            Assert.AreEqual(expectedHealth, statRepository.GetOrAddStat(hp.name).Value, "Current health");
        }
    }
}