using Game.Entities;
using SkillActions;
using UnityEngine;

namespace SkillEffects.Effects
{
    [CreateAssetMenu(menuName = "Skills/Effects/KnockBack")]
    public class KnockBack : SkillEffectData
    {
        private const string EFFECT_NAME = "KNOCK_BACK";
        private static readonly int EFFECT_TAG = EFFECT_NAME.GetHashCode();

        [SerializeField] private float knockBackTimeInSeconds;
        [SerializeField] private float knockBackDistance;

        private SkillActionTriggerData data;
        private Vector3 targetPosition;
        private float currentKnockBackTime;
        private IEntity targetEntity;
        private Vector3 targetInitialPosition;

        public override bool Apply(SkillActionTriggerData data)
        {
            this.data = data;

            targetEntity = data.targetEntity;

            if (targetEntity == null)
            {
                Debug.LogError("Cant apply knockback to null");

                return false;
            }

            if (targetEntity.Tags.HasTag(EFFECT_TAG))
                return false;

            targetEntity.Tags.ApplyTag(EFFECT_TAG);
            targetEntity.ToggleActions(false, EFFECT_NAME);
            targetEntity.InterruptActions();
            targetInitialPosition = targetEntity.GetPosition();
            targetPosition = targetInitialPosition + (targetInitialPosition - data.owner.GetPosition()).normalized * knockBackDistance;
            return true;
        }

        public override void Remove()
        {
            targetEntity.Tags.RemoveTag(EFFECT_TAG);
            targetEntity.ToggleActions(true, EFFECT_NAME);
        }

        public override void Dispose()
        {
            // nothing to dispose
        }

        public override bool OnUpdate(float deltaTime)
        {
            currentKnockBackTime += deltaTime;

            if (currentKnockBackTime >= knockBackTimeInSeconds)
                return false;

            var currentPosition = GetPosition();
            // Todo: investigate if its best to use inercia forces
            targetEntity.GetView().ForceMoveTowards(currentPosition);

            return true;
        }

        private Vector3 GetPosition()
        {
            float easeOutExpo = EaseOutQuart(currentKnockBackTime);
            return Vector3.Lerp(targetInitialPosition, targetPosition, easeOutExpo);
        }

        private float EaseOutQuart(float time) =>
            1f - Mathf.Pow(1f - time, 4f);

        private float EaseOutExpo(float time) =>
            time >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * time);
    }
}
