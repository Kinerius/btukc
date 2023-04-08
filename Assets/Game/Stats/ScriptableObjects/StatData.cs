using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "NewStat", menuName = "Data/New Stat")]
    public class StatData : ScriptableObject
    {
        public float defaultValue;
        public StatData min;
        public StatData max;
        public StatData additive;
        public StatData multiplier;
        public string description;
    }
}