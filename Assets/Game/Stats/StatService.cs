using System.Collections.Generic;

namespace Stats
{
    public class StatService : IStatService
    {
        private readonly List<IStatModifier> modifiers;
        private readonly IStatRepository repository;

        public StatService(IStatRepository repository)
        {
            this.repository = repository;
            modifiers = new List<IStatModifier>();
        }

        public void AddStatModifier(IStatModifier statModifier, bool permanent = false)
        {
            if (!permanent) modifiers.Add(statModifier);
            statModifier.ApplyModifier(repository);
        }

        public void RemoveStatModifier(IStatModifier statModifier, bool permanent = false)
        {
            if (!permanent)
            {
                if (!modifiers.Contains(statModifier)) return;
                modifiers.Remove(statModifier);
                statModifier.UndoModifier(repository);
            }
            else
            {
                statModifier.UndoModifier(repository);
            }
        }

        public float GetStatTotalValue(string statName)
        {
            return repository.GetOrAddStat(statName).Value;
        }

        // TODO: this should be readonly
        public IReactiveStat<float> ObserveStat(string statName)
        {
            return repository.GetOrAddStat(statName).ReactiveTotalValue;
        }

        public Stat GetStatReference(string statName)
        {
            return repository.GetOrAddStat(statName);
        }

        public void AddStatsFromRepository(StatRepository statRepository)
        {
            repository.AddStats(statRepository);
        }

        public void RemoveStatsFromRepository(StatRepository statRepository)
        {
            repository.RemoveStats(statRepository);
        }
    }
}