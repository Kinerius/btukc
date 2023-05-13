using System.Collections.Generic;
using Entities;
using Game;
using Stats;
using System;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Character
{
    public class CharacterControllerEntityView : EntityView
    {
        private Dictionary<string, Transform> anchors;

        [SerializeField] private CharacterController characterController;
        private Quaternion lastLookRotation;
        private bool isActionEnabled = true;
        private Entity entity;
        private IReactiveStat<float> movementSpeed;

        private void Start()
        {
            entity = GetComponent<Entity>();
            movementSpeed = entity.Stats.ObserveStat("MovementSpeed");
        }

        public override void MoveTowards(Vector3 position)
        {
            if (!isActionEnabled)
                return;

            var movementDirection = position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lastLookRotation, 360 * 2 * Time.deltaTime);

            var finalDirection = movementDirection * movementSpeed.Value * Time.deltaTime;

            if (!(finalDirection.sqrMagnitude > 0))
                return;

            lastLookRotation = Quaternion.LookRotation(finalDirection);

            finalDirection.y = Physics.gravity.y * Time.deltaTime;
            characterController.Move(finalDirection);
        }

        public override void ForceMoveTowards(Vector3 position)
        {
            characterController.Move(position - transform.position);
        }

        public override void ToggleMovement(bool enabled)
        {
            isActionEnabled = enabled;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lastLookRotation, 360);
        }

        public override void OnAwake() { }

        public override void StopMoving() { }

        public override void LookAtInstant(Vector3 position)
        {
            var transformPosition = transform.position;
            position.y = transformPosition.y;
            Quaternion rotation = Quaternion.LookRotation((position - transformPosition).normalized);
            transform.rotation = rotation;
        }

        public override void Teleport(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }
    }
}
