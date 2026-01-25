using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;

namespace SwipeElements.Infrastructure.Services.GridService
{
    public interface IGridService
    {
        void AdaptiveGrid(GridView grid, LevelData data);
    }
}