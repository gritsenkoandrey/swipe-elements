using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;

namespace SwipeElements.Infrastructure.Builders
{
    public interface ILevelBuilder
    {
        void Build(LevelView level, LevelData data);
    }
}