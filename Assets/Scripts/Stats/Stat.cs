using UnityEngine;

namespace Stats
{
    public class Stat
    {
        public float Value => totalValue.Value;
        public IReactiveStat<float> ReactiveTotalValue => totalValue;

        private readonly string name;
        private readonly float baseValue;
        private const float BaseMultiplier = 100;

        private readonly IReactiveStat<float> totalValue = new ReactiveStat<float>(0);
        
        private IReactiveStat<float> max = new ReactiveStat<float>(float.MaxValue);
        private IReactiveStat<float> min = new ReactiveStat<float>(-float.MaxValue);
        private IReactiveStat<float> modifier = new ReactiveStat<float>(0);
        private IReactiveStat<float> multiplier = new ReactiveStat<float>(0);
        public IReactiveStat<float> Max => max;
        public IReactiveStat<float> Min => min;
        public IReactiveStat<float> Modifier => modifier;
        public IReactiveStat<float> Multiplier => multiplier;

        public Stat(string name, float initialValue)
        {
            this.name = name;
            baseValue = initialValue;
            
            SetMaxBinding(max);
            SetMinBinding(min);
            SetModifierBinding(modifier);
            SetMultiplierBinding(multiplier);
        }

        private void OnAnyModifierChanged(float value)
        {
           CalculateTotalValue();
        }

        public void AddValue(float value)
        {
            modifier.Value += value;
        }

        public void AddMultiplier(float value)
        {
            multiplier.Value += value;
        }

        public void SetModifierBinding(Stat stat)
        {
            SetModifierBinding(stat.ReactiveTotalValue);
        }
        
        public void SetMultiplierBinding(Stat stat)
        {
            SetMultiplierBinding(stat.ReactiveTotalValue);
        }

        public void SetMinBinding(Stat stat)
        {
            SetMinBinding(stat.ReactiveTotalValue);
        }        
        
        public void SetMaxBinding(Stat stat)
        {
            SetMaxBinding(stat.ReactiveTotalValue);
        }

        private void SetModifierBinding(IReactiveStat<float> reactiveStat)
        {
            if (modifier != null)
            {
                modifier.OnChanged -= OnAnyModifierChanged;
            }
            
            modifier = reactiveStat;
            modifier.OnChanged += OnAnyModifierChanged;
            CalculateTotalValue();
        }

        private void SetMultiplierBinding(IReactiveStat<float> reactiveStat)
        {
            if (multiplier != null)
            {
                multiplier.OnChanged -= OnAnyModifierChanged;
            }
            
            multiplier = reactiveStat;
            multiplier.OnChanged += OnAnyModifierChanged;
            CalculateTotalValue();
        }

        private void SetMinBinding(IReactiveStat<float> reactiveStat)
        {
            if (min != null)
            {
                min.OnChanged -= OnAnyModifierChanged;
            }
            
            min = reactiveStat;
            min.OnChanged += OnAnyModifierChanged;
            CalculateTotalValue();
        }
    
        private void SetMaxBinding(IReactiveStat<float> reactiveStat)
        {
            if (max != null)
            {
                max.OnChanged -= OnAnyModifierChanged;
            }
            
            max = reactiveStat;
            max.OnChanged += OnAnyModifierChanged;
            CalculateTotalValue();
        }
    
        private void CalculateTotalValue()
        {
            totalValue.Value = Mathf.Clamp((baseValue + modifier.Value) * ((BaseMultiplier + multiplier.Value) / 100), min.Value, max.Value);
        }

        public string GetName()
        {
            return name;
        }
        public override string ToString()
        {
            return name + ": " + totalValue.Value;
        }
    }
}
