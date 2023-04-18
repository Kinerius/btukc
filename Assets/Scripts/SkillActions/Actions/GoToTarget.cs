using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using UnityEngine;

namespace SkillActions.Actions
{
    [CreateAssetMenu(menuName = "Skills/Actions/GoToTarget")]

    public class GoToTarget : ScriptableAction
    {
        [SerializeField] private float minDistance = 2;
        public override async UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats)
        {
            if (IsAtDistance(data))
            {
                return;
            }

            await UniTask.WaitUntil(() =>
            {
                data.view.SetDestination(data.targetPosition);

                return IsAtDistance(data);
            });
        }

        private bool IsAtDistance(SkillActionTriggerData data) =>
            Vector3.Distance(data.view.transform.position, GetTargetPosition(data)) <= minDistance;

        private Vector3 GetTargetPosition(SkillActionTriggerData data) =>
            data.targetEntity?.GetPosition() ?? data.targetPosition;
    }
}
