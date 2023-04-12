using System;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public static class InputActionReferenceExtensions
    {
        public static void BindAction<T>(this InputActionReference input, Action<T> action) where T : struct
        {
            input.action.started += v => action(v.ReadValue<T>());
            input.action.canceled += v => action(v.ReadValue<T>());
        }
    }
}