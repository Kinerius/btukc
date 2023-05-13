using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;
using System.Threading;

namespace SkillActions
{
    public interface IAction
    {
        UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats, CancellationToken cancellationToken);
    }
}
