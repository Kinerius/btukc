using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public static class DefaultStats
    {
        private static StatData[] allStats;

        public static IEnumerable<StatData> GetAllStats()
        {
            return allStats ??= Resources.LoadAll<StatData>("Stats/Entities");
        }
    }
}