using System;
using Object = UnityEngine.Object;

namespace AI
{
    [Serializable]
    public struct AIBehaviourData
    {
        public AIBehaviour behaviour;
        public int priority;

        public AIBehaviourData(AIBehaviour behaviour, int priority)
        {
            this.behaviour = behaviour;
            this.priority = priority;
        }

        public AIBehaviourData CopyNewInstance() =>
            new (Object.Instantiate(behaviour), priority);
    }
}
