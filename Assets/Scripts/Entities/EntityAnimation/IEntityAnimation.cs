namespace Entities
{
    public interface IEntityAnimation
    {
        void SetDefaultAnimation(string name);
        void Play(string name);
    }
}