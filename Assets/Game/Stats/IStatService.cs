namespace Stats
{
    public interface IStatService
    {
        void AddStatModifier(IStatModifier statModifier, bool permanent = true);
        void RemoveStatModifier(IStatModifier statModifier, bool permanent = true);
        float GetStatTotalValue(string statName);
        IReactiveStat<float> ObserveStat(string statName);
        Stat GetStatReference(string name);
        void AddStatsFromRepository(StatRepository getStats);
        void RemoveStatsFromRepository(StatRepository getStats);
    }
}