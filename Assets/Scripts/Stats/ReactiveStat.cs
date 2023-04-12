using System;

namespace Stats
{
    public class ReactiveStat<T> : IReactiveStat<T>
    {
        private T value;
        public event Action<T> OnChanged = _ => {};

        public T Value
        {
            get => value;
            set
            {
                if (value.Equals(this.value))
                    return;
                
                this.value = value;
                OnChanged(this.value);
            }
        }

        public ReactiveStat(T value)
        {
            Value = value;
        }

    }
}