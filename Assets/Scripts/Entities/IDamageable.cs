using Entities.Events;

namespace Entities
{
    public interface IDamageable
    {
        public void ReceiveDamage(DamageEvent payload);
    }
}