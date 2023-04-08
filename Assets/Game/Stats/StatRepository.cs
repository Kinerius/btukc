using System.Collections.Generic;
using JetBrains.Annotations;

namespace Stats
{
    public class StatRepository : IStatRepository
    {
        private readonly List<string> statNames;
        private readonly List<Stat> stats;

        public StatRepository(StatPackageData statPackageData = null)
        {
            statNames = new List<string>();
            stats = new List<Stat>();

            var defaultStats = DefaultStats.GetAllStats();
            foreach (StatData defaultStat in defaultStats)
            {
                CreateStatFromData(defaultStat, defaultStat.defaultValue);
            }

            if (statPackageData != null)
                InitializeRepositoryWithData(statPackageData);
        }

        private void InitializeRepositoryWithData(StatPackageData statPackageData)
        {
            var stats = statPackageData.stats;

            foreach (var statDataInitialization in stats)
            {
                var statData = statDataInitialization.stat;
                CreateStatFromData(statData, statDataInitialization.initialValue);
            }
        }

        private void CreateStatFromData(StatData statData, float initialValue = 0)
        {
            var stat = GetOrAddStat(statData.name.ToLower());

            if (statData.min != null)
            {
                CreateStatFromData(statData.min);
                stat.SetMinBinding(GetOrAddStat(statData.min.name));
            }

            if (statData.max != null)
            {
                CreateStatFromData(statData.max);
                stat.SetMaxBinding(GetOrAddStat(statData.max.name));
            }

            if (statData.multiplier != null)
            {
                CreateStatFromData(statData.multiplier);
                stat.SetMultiplierBinding(GetOrAddStat(statData.multiplier.name));
            }

            if (statData.additive != null)
            {
                CreateStatFromData(statData.additive);
                stat.SetModifierBinding(GetOrAddStat(statData.additive.name));
            }

            stat.AddValue(initialValue);
        }

        public void AddStat(Stat stat)
        {
            if (stats.Contains(stat)) return;
            statNames.Add(stat.GetName());
            stats.Add(stat);
        }

        [CanBeNull]
        public Stat GetStat(string statName)
        {
            var name = statName.ToLower();
            return !statNames.Contains(name) ? null : stats[statNames.IndexOf(name)];
        }
        
        public Stat GetOrAddStat(string statName)
        {
            var name = statName.ToLower();

            if (!statNames.Contains(name))
            {
                statNames.Add(name);
                stats.Add(new Stat(name, 0));
            }
            
            return stats[statNames.IndexOf(name)];
        }

        public void AddStats(StatRepository statRepository)
        {
            foreach (Stat targetStat in statRepository.stats)
            {
                var stat = GetOrAddStat(targetStat.GetName());
                stat.AddValue(targetStat.Value);
            }
        }
        
        public void RemoveStats(StatRepository statRepository)
        {
            foreach (Stat targetStat in statRepository.stats)
            {
                var stat = GetOrAddStat(targetStat.GetName());
                stat.AddValue(-targetStat.Value);
            }
        }
    }
}
