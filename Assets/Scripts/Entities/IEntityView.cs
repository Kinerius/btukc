using UnityEngine;

namespace Entities
{
    public interface IEntityView
    {
        EntityAnimationController GetAnimationController();

        Transform GetAnchor(string anchorTag);

        void StopMoving();

        void LookAtInstant(Vector3 targetPosition);

        void ForceMoveTowards(Vector3 targetPosition);

        void MoveTowards(Vector3 targetPosition);
        void Teleport(Vector3 targetPosition);

        void ToggleMovement(bool enabled);
    }
}
