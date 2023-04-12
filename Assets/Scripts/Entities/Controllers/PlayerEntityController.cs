using Game.Input;
using Stats;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [CreateAssetMenu(menuName = "Controllers/PlayerEntityController", fileName = "PlayerEntityController", order = 0)]
    public class PlayerEntityController : EntityController
    {
        public InputActionReference movement;
        public InputActionReference action1;
        public InputActionReference action2;
        private Vector3 movementDirection;
        private Entity entity;

        public override void Initialize(Entity entity)
        {
            this.entity = entity;
            movement.action.Enable();
            action1.action.Enable();
            action2.action.Enable();

            action1.action.performed += OnAction1;
            action2.action.performed += OnAction2;
        }

        private void OnAction1(InputAction.CallbackContext obj)
        {
            entity.StartAction(0);
        }

        private void OnAction2(InputAction.CallbackContext obj)
        {
            entity.StartAction(1);
        }

        public override void Update()
        {
            var inputMovementDirection = movement.action.ReadValue<Vector2>();
            movementDirection.x = inputMovementDirection.x;
            movementDirection.z = inputMovementDirection.y;

            entity.Move(movementDirection);
        }
    }
}
