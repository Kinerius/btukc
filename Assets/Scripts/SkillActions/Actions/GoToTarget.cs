using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System.Threading;
using UnityEngine;

namespace SkillActions.Actions
{

    public class GoToTarget : ScriptableAction
    {
        [SerializeField] private float minDistance = 2;
        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken)
        {
            if (IsAtDistance(data))
                return;

            await UniTask.WaitUntil(() =>
            {
                data.view.MoveTowards(data.targetPosition);

                return IsAtDistance(data);
            }, cancellationToken: cancellationToken);
        }

        private bool IsAtDistance(SkillActionTriggerData data) =>
            Vector3.Distance(data.view.transform.position, GetTargetPosition(data)) <= minDistance;

        private Vector3 GetTargetPosition(SkillActionTriggerData data) =>
            data.targetEntity?.GetPosition() ?? data.targetPosition;
    }
}
