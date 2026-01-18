using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;

namespace SwipeElements.Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService
    {
        void Init();
        ScreenConfig GetScreenConfig();
        LevelConfig GetLevelConfig();
        AirBalloonConfig GetAirBallonConfig();
        ElementConfig GetElementConfig();
        GridConfig GetGridConfig();
    }
}