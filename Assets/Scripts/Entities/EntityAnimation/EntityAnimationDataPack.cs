using UnityEngine;

namespace Entities.Animation
{
    [CreateAssetMenu]
    public class EntityAnimationDataPack : ScriptableObject
    {
        public AnimationClip[] clips;
    }
}