using SkillEffects;

namespace Game.Level
{
    public class LevelManager
    {
        public readonly PoolManager poolManager;
        public readonly GlobalClock globalClock;
        public readonly SkillEffectManager effectsManager;

        public LevelManager(GlobalClock globalClock, SkillEffectManager effectsManager)
        {
            poolManager = new PoolManager();
            this.globalClock = globalClock;
            this.effectsManager = effectsManager;
        }
    }
}
