using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityCamera = UnityEngine.Camera;

namespace Character
{
    public class CharacterControllerEntityView : EntityView
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float maxPathLength = 20;
        [SerializeField] private float speed = 7;
        [SerializeField] private float rotationSpeed = 180;

        [Range(1f, 100f)]
        [SerializeField] private float directionalMovementPercent = 50;

        private NavMeshPath _pathBuffer;
        private int _moveIndex = 0;
        private NavMeshQueryFilter _navMeshQueryFilter;
        private Dictionary<string, Transform> anchors;

        public override void OnAwake()
        {
            var navMeshBuildSettings = NavMesh.GetSettingsByIndex(1);
            _navMeshQueryFilter = new NavMeshQueryFilter
            {
                agentTypeID = navMeshBuildSettings.agentTypeID,
                areaMask = 1 << NavMesh.GetAreaFromName("Walkable")
            };
            _pathBuffer = new NavMeshPath();
        }

        private void Update()
        {
            MoveCharacter();
        }

        public override void StopMoving()
        {
            _pathBuffer.ClearCorners();
        }

        public override void LookAt(Vector3 position)
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

        private void MoveCharacter()
        {
            if (_pathBuffer.status == NavMeshPathStatus.PathComplete && IsPathValid())
            {
                var nextPoint = _pathBuffer.corners[_moveIndex];
                var distance = Vector3.Distance(transform.position, nextPoint);
                var direction = (nextPoint - transform.position).normalized;
                var lookDirection = Quaternion.LookRotation(direction);
                lookDirection.x = 0;
                lookDirection.z = 0; // look rotation should always be flat

                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
                
                if (distance > 0.5f)
                {
                    var forwardMovement = transform.forward * speed * (100 - directionalMovementPercent) / 100;
                    var directionalMovement = direction * speed * directionalMovementPercent / 100;
                    _characterController.Move((forwardMovement + directionalMovement) * Time.deltaTime);
                    _characterController.Move(Physics.gravity * Time.deltaTime);
                }
                else
                {
                    _moveIndex++;
                    MoveCharacter();
                }
            }
        }

        public override void SetDestination(Vector3 targetPosition)
        {
            _moveIndex = 1;
            NavMesh.SamplePosition(transform.position, out var sampleHit, 3, _navMeshQueryFilter);
            var currentPosition = sampleHit.position;
            targetPosition.y = currentPosition.y;

            _pathBuffer.ClearCorners();
            NavMesh.CalculatePath(currentPosition, targetPosition, _navMeshQueryFilter, _pathBuffer);
            if (GetPathLength() < maxPathLength && IsPathValid()) return;
            NavMesh.Raycast(currentPosition, targetPosition, out var navHit, _navMeshQueryFilter);
            NavMesh.CalculatePath(currentPosition, navHit.position, _navMeshQueryFilter, _pathBuffer);
        }

        private bool IsPathValid()
        {
            return _pathBuffer.corners.Length > 0 && _moveIndex < _pathBuffer.corners.Length;
        }

        private float GetPathLength()
        {
            if (_pathBuffer.corners.Length == 0) return float.MaxValue;
            var previousCorner = _pathBuffer.corners[0];
            var dist = 0f;
            for (int i = 1; i < _pathBuffer.corners.Length; i++)
            {
                dist += Vector3.Distance(_pathBuffer.corners[i] , previousCorner);
                previousCorner = _pathBuffer.corners[i];
            }
            return dist;
        }
    }
}
