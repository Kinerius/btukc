namespace Game.Level
{
    public class LevelState
    {
        public readonly PoolManager poolManager;
        public readonly GlobalClock globalClock;

        public LevelState(GlobalClock globalClock)
        {
            poolManager = new PoolManager();
            this.globalClock = globalClock;
        }
    }
}