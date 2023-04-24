using UnityEngine;

namespace Entities
{
    public interface IEntityView
    {
        EntityAnimationController GetAnimationController();

        Transform GetAnchor(string anchorTag);

        void StopMoving();

        void LookAt(Vector3 targetPosition);

        void Teleport(Vector3 targetPosition);

        void MoveTowards(Vector3 position);

        void ToggleMovement(bool enabled);
    }
}
