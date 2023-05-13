using System;
using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/TriggerAnimation")]
    public class TriggerAnimation : ScriptableAction
    {
        [SerializeField] private string animationName;
        [SerializeField] private AnimationEventData[] events;
        [SerializeField] private bool waitForAnimationToEnd;

        // TODO: Evaluate replacing this whole system with a timeline
        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager environment, StatRepository skillStats, CancellationToken cancellationToken)
        {
            var animation = data.view.GetAnimationController();

            void EventHandler(string eventName)
            {
                OnAnimationEvent(eventName, data, environment, skillStats, cancellationToken);
            }

            animation.OnAnimationEvent += EventHandler;
            animation.Play(animationName);

            try
            {
                var state = animation.GetAnimationState(animationName);
                if (waitForAnimationToEnd)
                {
                    await UniTask.WaitUntil(() => state.time != 0, cancellationToken: cancellationToken);
                    await UniTask.WaitUntil(() => state.time == 0, cancellationToken: cancellationToken);
                }
            }
            catch (OperationCanceledException _) { }
            catch (Exception e) { Debug.LogException(e); }
            finally
            {
                animation.OnAnimationEvent -= EventHandler;
            }
        }

        private async void OnAnimationEvent(string eventName, SkillActionTriggerData data, LevelManager environment, StatRepository skillStats, CancellationToken cancellationToken)
        {
            foreach (var evt in events)
                if (evt.eventName == eventName)
                    await evt.action.StartAction(data, environment, skillStats, cancellationToken);
        }
    }

    [Serializable]
    public struct AnimationEventData
    {
        public string eventName;
        public ScriptableAction action;
    }
}
