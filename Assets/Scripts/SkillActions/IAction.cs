using Cysharp.Threading.Tasks;
using Game.Level;
using Stats;

namespace SkillActions
{
    public interface IAction
    {
        UniTask StartAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository skillStats);
    }
}