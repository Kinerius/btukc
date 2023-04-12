using Game.Entities;

namespace Handlers
{
    public class DamagePayload
    {
        public IEntity Source { get; }
        public long PrecomputedDamage { get; }

        public DamagePayload(IEntity source, long precomputedDamage)
        {
            Source = source;
            PrecomputedDamage = precomputedDamage;
        }
    }
}
