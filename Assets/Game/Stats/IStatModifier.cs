namespace Stats
{
    public interface IStatModifier
    {
        void ApplyModifier(IStatRepository statRepository);
        void UndoModifier(IStatRepository statRepository);
    }
}