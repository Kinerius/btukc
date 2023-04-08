using System;

namespace Stats
{
    public interface IReactiveStat<T>
    {
        event Action<T> OnChanged;
        T Value { get; set; }
    }
}