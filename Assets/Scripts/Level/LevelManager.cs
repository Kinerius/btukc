namespace Game.Level
{
    public class LevelManager
    {
        public readonly PoolManager poolManager;
        public readonly GlobalClock globalClock;

        public LevelManager(GlobalClock globalClock)
        {
            poolManager = new PoolManager();
            this.globalClock = globalClock;
        }
    }
}