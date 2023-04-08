using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "NewStatPackage", menuName = "Data/New Stat Package")]
    public class StatPackageData : ScriptableObject
    {
        public StatDataInitialization[] stats;

        public StatRepository AsStatRepository()
        {
            return new StatRepository(this);
        }
    }
}