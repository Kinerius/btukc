namespace SkillActions
{
    public interface ISkillService
    {
        void AddSkill(SkillAction skillAction);
        void RemoveSkill(SkillAction skillAction);
    }
}