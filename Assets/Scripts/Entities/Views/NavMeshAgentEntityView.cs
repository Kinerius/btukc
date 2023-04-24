﻿using Entities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentEntityView : EntityView
    {
        [HideInInspector] [SerializeField] private NavMeshAgent agent;
        private bool isMovementEnabled;

        private void OnValidate()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public override void OnAwake()
        {

        }

        public override void StopMoving()
        {
            agent.isStopped = true;
        }

        public override void LookAt(Vector3 position)
        {
            var transformPosition = transform.position;
            position.y = transformPosition.y;
            Quaternion rotation = Quaternion.LookRotation((position - transformPosition).normalized);
            transform.rotation = rotation;
        }

        public override void Teleport(Vector3 finalPosition)
        {
            agent.Warp(finalPosition);
        }

        public override void MoveTowards(Vector3 position)
        {
            agent.destination = position;
        }

        public override void ToggleMovement(bool enabled)
        {
            isMovementEnabled = enabled;
            agent.isStopped = !enabled;
            agent.angularSpeed = enabled ? 360 : 0;
            agent.acceleration = enabled ? 8 : 1000;
        }
    }
}
