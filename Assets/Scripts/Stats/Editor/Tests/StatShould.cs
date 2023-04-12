using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Stats.Editor.Tests
{
    public class StatShould
    {
        [Test]
        public void CalculateTotalValueOnAdd()
        {
            Stat stat = new Stat("",100);
            
            stat.AddValue(150);
            
            Assert.AreEqual(250, stat.Value);
        }
        
        [Test]
        public void CalculateTotalValueOnAddMultiplier()
        {
            Stat stat = new Stat("",100);

            stat.AddMultiplier(100);
                
            Assert.AreEqual(200, stat.Value);
        }

        [Test]
        public void ClampTotalValueToMin()
        {
            Stat stat = new Stat("",500);
            stat.Min.Value = 100;
            stat.AddValue(-2000);
            
            Assert.AreEqual(100, stat.Value);
        }
        
        [Test]
        public void ClampTotalValueToMax()
        {
            Stat stat = new Stat("",100);
            stat.Max.Value = 500;

            stat.AddValue(1000);

            Assert.AreEqual(500, stat.Value);
        }

        [Test]
        public void UpdateTotalValueOnMaxBindingChange()
        {
            Stat stat = new Stat("",100);
            Stat maxStat = new Stat("",0);

            stat.AddValue(500);
            stat.AddMultiplier(1000);
            stat.SetMaxBinding(maxStat);
            maxStat.AddValue(100);

            Assert.AreEqual(100, stat.Value);
            
        }
        
        [Test]
        public void UpdateTotalValueOnMinBindingChange()
        {
            Stat stat = new Stat("",0);
            Stat minStat = new Stat("",100);

            stat.SetMinBinding(minStat);
            minStat.AddValue(50);

            Assert.AreEqual(150, stat.Value);
            
        }

        [Test]
        public void UpdateValueOnMultiplierBindingChange()
        {    
            Stat damage = new Stat("",0);
            Stat damageMultiplier = new Stat("",0);

            damage.AddValue(1000);
            damage.SetMultiplierBinding(damageMultiplier);
            damageMultiplier.AddValue(50);
            damageMultiplier.AddMultiplier(100);
            
            Assert.AreEqual(1000 * 2, damage.Value );
        }

        [Test]
        public void CallDeathOnStatZero()
        {
            Stat currentHealth = new Stat("hp",0);
            Stat maxHealth = new Stat("maxhp",0);
            bool isDead = false;

            currentHealth.SetMaxBinding(maxHealth);
            maxHealth.AddValue(1000);
            currentHealth.AddValue(maxHealth.Value);
            currentHealth.ReactiveTotalValue.OnChanged += value => isDead = value <= 0;

            maxHealth.AddValue(-10000); 

            Assert.IsTrue(isDead);
        }
    }
}
