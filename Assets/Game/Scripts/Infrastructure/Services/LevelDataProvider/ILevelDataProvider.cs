using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;

namespace SwipeElements.Infrastructure.Services.LevelDataProvider
{
    public interface ILevelDataProvider
    {
        LevelData GetLevelData();
    }
}