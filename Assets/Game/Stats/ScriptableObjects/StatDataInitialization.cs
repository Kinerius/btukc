using System;

namespace Stats
{

    [Serializable]
    public class StatDataInitialization
    {
        public StatData stat;
        public float initialValue;

        public override string ToString()
        {
            return stat.name;
        }
    }
}