using JetBrains.Annotations;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.GridService
{
    [UsedImplicitly]
    public sealed class GridService : IGridService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICameraService _cameraService;
        
        public GridService(IStaticDataService staticDataService, ICameraService cameraService)
        {
            _staticDataService = staticDataService;
            _cameraService = cameraService;
        }

        void IGridService.AdaptiveGrid(GridView grid, LevelData data)
        {
            Camera camera = _cameraService.MainCamera;
            GridConfig gridConfig = _staticDataService.GetGridConfig();
            
            float screenHeight = camera.orthographicSize * 2f;
            float screenWidth = screenHeight * camera.aspect;

            float availableWidth = screenWidth * (1f - gridConfig.ScreenPadding * 2f);
            float availableHeight = screenHeight * (1f - gridConfig.ScreenPadding * 2f);

            float maxSizeByWidth = availableWidth / data.grid.gridSize.x;
            float maxSizeByHeight = availableHeight / data.grid.gridSize.y;
            
            float cellSize = Mathf.Min(maxSizeByWidth, maxSizeByHeight);
            
            grid.Init(data.grid.gridSize, cellSize);
        }
    }
}