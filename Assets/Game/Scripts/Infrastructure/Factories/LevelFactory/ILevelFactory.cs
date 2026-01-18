using SwipeElements.Game.Views;

namespace SwipeElements.Infrastructure.Factories.LevelFactory
{
    public interface ILevelFactory
    {
        LevelView Level { get; }
        LevelView CreateLevel();
    }
}