using System;
using System.Collections.Generic;
using Entities.Animation;
using UnityEngine;

namespace Entities
{
    public class EntityAnimationController : IEntityAnimation
    {
        public event Action<string> OnAnimationEvent = _ => { };
        private HashSet<string> animationClips = new();
        private string defaultAnimation;
        private UnityEngine.Animation animation;
        private EntityAnimationDataPack dataPack;

        private Dictionary<string, AnimationState> animationStates = new Dictionary<string, AnimationState>();

        public EntityAnimationController(UnityEngine.Animation animation, EntityAnimationDataPack dataPack)
        {
            this.dataPack = dataPack;
            this.animation = animation;
            Setup();
        }

        private void Setup()
        {
            if (animation == null) return;
            if (dataPack.clips.Length == 0) return;

            for (var i = 0; i < dataPack.clips.Length; i++)
            {
                var clip = dataPack.clips[i];
                var clipName = clip.name.ToLower();
                animationClips.Add(clipName);
                animation.AddClip(clip, clipName);

                var animationState = animation[clipName];

                animationStates.Add(clipName, animationState);
            }

            SetDefaultAnimation(dataPack.clips[0].name.ToLower());
            
            CheckState();
        }

        public void SetDefaultAnimation(string name)
        {
            if (animation == null) return;

            name = name.ToLower();
            
            if (animationClips.Contains(name))
            {
                defaultAnimation = name;
                CheckState();
            }
            else
            {
                Debug.LogError($"This entity doesnt contain animation: {name}", animation);
            }
        }

        public void Play(string name)
        {
            if (animation == null) return;

            name = name.ToLower();

            if (animationClips.Contains(name))
            {
                animation.CrossFadeQueued(name, 0.1f, QueueMode.PlayNow);
                animation.CrossFadeQueued(defaultAnimation, 0.25f, QueueMode.CompleteOthers);
            }
            else
            {
                Debug.LogError($"This entity doesnt contain animation: {name}", animation);
            }
        }

        private void CheckState()
        {
            if (animation == null) return;

            if (!animation.isPlaying)
            {
                animation.Play(defaultAnimation);
            }
        }

        public void RaiseAnimationEvent(string eventName)
        {
            OnAnimationEvent(eventName);
        }

        public bool IsAnimationPlaying(string animationName)
        {
            animationName = animationName.ToLower();
            return animationStates[animationName].weight > 0;
        }

        public AnimationState GetAnimationState(string animationName)
        {
            animationName = animationName.ToLower();
            return animationStates[animationName];
        }
    }
}