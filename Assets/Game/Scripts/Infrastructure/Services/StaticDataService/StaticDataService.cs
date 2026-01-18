using JetBrains.Annotations;
using SwipeElements.Infrastructure.Services.AssetService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;

namespace SwipeElements.Infrastructure.Services.StaticDataService
{
    [UsedImplicitly]
    public sealed class StaticDataService : IStaticDataService
    {
        private readonly IAssetService _assetService;
        
        private ScreenConfig _screenConfig;
        private LevelConfig _levelConfig;
        private AirBalloonConfig _airBalloonConfig;
        private ElementConfig _elementConfig;
        private GridConfig _gridConfig;

        public StaticDataService(IAssetService assetService)
        {
            _assetService = assetService;
        }

        void IStaticDataService.Init()
        {
            _screenConfig = _assetService.LoadFromResources<ScreenConfig>(AssetAddress.SCREEN_DATA_PATH);
            _levelConfig = _assetService.LoadFromResources<LevelConfig>(AssetAddress.LEVEL_DATA_PATH);
            _airBalloonConfig = _assetService.LoadFromResources<AirBalloonConfig>(AssetAddress.AIR_BALLON_DATA_PATH);
            _elementConfig = _assetService.LoadFromResources<ElementConfig>(AssetAddress.ELEMENT_DATA_PATH);
            _gridConfig = _assetService.LoadFromResources<GridConfig>(AssetAddress.GRID_DATA_PATH);
        }
        
        ScreenConfig IStaticDataService.GetScreenConfig() => _screenConfig;
        
        LevelConfig IStaticDataService.GetLevelConfig() => _levelConfig;
        
        AirBalloonConfig IStaticDataService.GetAirBallonConfig() => _airBalloonConfig;
        
        ElementConfig IStaticDataService.GetElementConfig() => _elementConfig;
        
        GridConfig IStaticDataService.GetGridConfig() => _gridConfig;
    }
}