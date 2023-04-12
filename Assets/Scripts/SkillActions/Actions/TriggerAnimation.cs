using System;
using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/TriggerAnimation")]
    public class TriggerAnimation : ScriptableAction
    {
        [SerializeField] private string animationName;
        [SerializeField] private AnimationEventData[] events;
        [SerializeField] private bool waitForAnimationToEnd;

        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager environment, StatRepository skillStats)
        {
            var animation = data.view.GetAnimationController();

            void EventHandler(string eventName)
            {
                OnAnimationEvent(eventName, data, environment, skillStats);
            }

            Debug.Log($"Starting animation {animationName}");

            animation.OnAnimationEvent += EventHandler;
            animation.Play(animationName);

            // TODO: Evaluate replacing this whole system with a timeline

            var state = animation.GetAnimationState(animationName);
            if (waitForAnimationToEnd)
            {
                await UniTask.WaitUntil(() => state.time != 0);

                await UniTask.WaitUntil(() => state.time == 0);
                Debug.Log("is zero");
            }
            Debug.Log("Animation Ended!");
            animation.OnAnimationEvent -= EventHandler;
        }

        private void OnAnimationEvent(string eventName, SkillActionTriggerData data, LevelManager environment, StatRepository skillStats)
        {
            Debug.Log($"animation event {eventName}");
            for (int i = 0; i < events.Length; i++)
            {
                var evt = events[i];

                if (evt.eventName == eventName)
                {
                    evt.action.StartAction(data, environment, skillStats);
                }
            }
        }
    }

    [Serializable]
    public struct AnimationEventData
    {
        public string eventName;
        public ScriptableAction action;
    }
}
