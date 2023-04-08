namespace Stats
{
    public interface IStatRepository
    {
        void AddStat(Stat stat);
        Stat GetOrAddStat(string statName);
        void AddStats(StatRepository statRepository);
        void RemoveStats(StatRepository statRepository);
    }
}