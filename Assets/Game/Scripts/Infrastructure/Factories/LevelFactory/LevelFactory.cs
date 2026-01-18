using JetBrains.Annotations;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Factories.AirBallonFactory;
using SwipeElements.Infrastructure.Factories.ElementFactory;
using SwipeElements.Infrastructure.Serialize;
using SwipeElements.Infrastructure.Serialize.Settings;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Factories.LevelFactory
{
    [UsedImplicitly]
    public sealed class LevelFactory : ILevelFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICameraService _cameraService;
        private readonly IObjectResolver _objectResolver;
        private readonly IAirBallonFactory _airBallonFactory;
        private readonly IElementFactory _elementFactory;
        private readonly IProgressService _progressService;
        
        private LevelView _level;
        
        LevelView ILevelFactory.Level => _level;
        
        public LevelFactory
        (
            IStaticDataService staticDataService, 
            IObjectResolver objectResolver, 
            ICameraService cameraService, 
            IAirBallonFactory airBallonFactory, 
            IElementFactory elementFactory,
            IProgressService progressService
        )
        {
            _staticDataService = staticDataService;
            _objectResolver = objectResolver;
            _cameraService = cameraService;
            _airBallonFactory = airBallonFactory;
            _elementFactory = elementFactory;
            _progressService = progressService;
        }

        LevelView ILevelFactory.CreateLevel()
        {
            _level = null;
            _level = _objectResolver.Instantiate(_staticDataService.GetLevelConfig().BaseLevel, null);

            CreateElements(_level);
            CreateAirBallons(_level);
            FitBackground(_level.Background);
            AdaptiveGrid(_level.Grid);

            return _level;
        }
        
        private void FitBackground(SpriteRenderer background)
        {
            Bounds bounds = background.bounds;
            Camera camera = _cameraService.MainCamera;
        
            float screenRatio = (float)Screen.width / Screen.height;
            float targetRatio = bounds.size.x / bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                float differenceInSize = targetRatio / screenRatio;
                
                camera.orthographicSize = bounds.size.y / 2f * differenceInSize;
            }
            else
            {
                camera.orthographicSize = bounds.size.y / 2f;
            }

            float bottomY = bounds.min.y;
            float topY = bounds.max.y;
            float cameraBottomPosition = bottomY + camera.orthographicSize;
            float cameraTopPosition = topY - camera.orthographicSize;
            float targetY = Mathf.Lerp(cameraBottomPosition, cameraTopPosition, 0f);
            Vector3 newPosition = camera.transform.position;
            newPosition.y = targetY;
            newPosition.x = bounds.center.x;
            
            camera.transform.position = newPosition;
        }
        
        private void AdaptiveGrid(GridView grid)
        {
            Camera camera = _cameraService.MainCamera;
            GridConfig gridConfig = _staticDataService.GetGridConfig();
            
            float screenHeight = camera.orthographicSize * 2f;
            float screenWidth = screenHeight * camera.aspect;

            float availableWidth = screenWidth * (1f - gridConfig.ScreenPadding * 2f);
            float availableHeight = screenHeight * (1f - gridConfig.ScreenPadding * 2f);

            float maxSizeByWidth = availableWidth / grid.Size.x;
            float maxSizeByHeight = availableHeight / grid.Size.y;

            grid.CellSize = Mathf.Min(maxSizeByWidth, maxSizeByHeight);
        }

        private void CreateAirBallons(LevelView level)
        {
            AirBalloonConfig config = _staticDataService.GetAirBallonConfig();
            
            int count = Random.Range(config.MinMaxCount.Min, config.MinMaxCount.Max + 1);
            
            Camera camera = _cameraService.MainCamera;
            float height = 2f * camera.orthographicSize;
            float width = height * camera.aspect;
            float topY = camera.transform.position.y + height / 2f;
            float bottomSpawnY = camera.transform.position.y + height / 6f;
            float leftX = camera.transform.position.x - width / 2f;
            float rightX = camera.transform.position.x + width / 2f;
            float padding = 1f; 

            for (int i = 0; i < count; i++)
            {
                float randomX = Random.Range(leftX + padding, rightX - padding);
                float randomY = Random.Range(bottomSpawnY, topY - padding);
                
                Vector3 spawnPosition = new (randomX, randomY, 0f);
                
                _airBallonFactory.CreateAirBallon(spawnPosition, level.transform);
            }
        }
        
        private void CreateElements(LevelView level)
        {
            int length = _staticDataService.GetLevelConfig().Levels.Length;
            
            TextAsset asset = _staticDataService.GetLevelConfig().Levels[_progressService.LevelIndex.Value % length];

            JsonData data;
            
            if (string.IsNullOrEmpty(_progressService.LevelJson.Value))
            {
                data = LevelSerializer.Deserialize(asset.text);
            }
            else
            {
                data = LevelSerializer.Deserialize(_progressService.LevelJson.Value);

                if (data.elements.Count == 0)
                {
                    data = LevelSerializer.Deserialize(asset.text);
                }
            }
            
            level.Grid.SetGridSize(data.grid.gridSize);

            foreach (ElementData elementData in data.elements)
            {
                if (_elementFactory.TryCreateElement(elementData.id, level.ElementsRoot, out ElementProvider element))
                {
                    element.GetData().view.SetPosition(elementData.position);
                    
                    level.Elements.Add(element);
                }
            }
        }
    }
}