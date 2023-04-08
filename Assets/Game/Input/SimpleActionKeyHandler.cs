using System;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class SimpleActionKeyHandler
    {
        private bool isStarted;
        private readonly Action action;

        public SimpleActionKeyHandler(InputActionReference input, Action action)
        {
            this.action = action;
            input.action.started += _ => isStarted = true;
            input.action.canceled += _ => isStarted = false;
        }

        public void TryStartAction()
        {
            if (isStarted) action();
        }
    }
}