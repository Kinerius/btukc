using System;
using Agent;
using Entities;
using Game;
using Game.Entities;
using Game.Level;
using UnityEngine;

namespace AI
{
    public abstract class AIBehaviour : ScriptableObject
    {
        public event Action OnRaiseBehaviourCheck = () => {};
        protected IEntityView agent;
        protected IEntity thisEntity;
        protected IEntityAnimation thisEntityAnimation;
        protected LevelManager levelManager;

        public void Setup(IEntity entity, IEntityAnimation animation, IEntityView view, LevelManager levelManager)
        {
            this.levelManager = levelManager;
            this.thisEntity = entity;
            this.agent = view;
            thisEntityAnimation = animation;
            OnSetup();
        }

        protected void RaiseBehaviourCheck()
        {
            OnRaiseBehaviourCheck.Invoke();
        }

        protected abstract void OnSetup();
        public abstract void Update();

        // This is called every time we want to check if the behaviour should be activated
        public abstract bool IsActive();

        // This is called when this Behaviour is activated
        public abstract void Activate();
        public abstract void Disable();
    }
}
