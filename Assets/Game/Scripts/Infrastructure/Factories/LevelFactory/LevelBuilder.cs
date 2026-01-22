using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Extensions;
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

namespace SwipeElements.Infrastructure.Factories.LevelFactory
{
    public sealed class LevelBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICameraService _cameraService;
        private readonly IProgressService _progressService;
        private readonly IElementFactory _elementFactory;
        private readonly IAirBallonFactory _airBallonFactory;
        
        private readonly LevelView _level;
        private JsonData _data;
        
        public LevelBuilder
        (
            LevelView level,
            IStaticDataService staticDataService, 
            ICameraService cameraService, 
            IProgressService progressService, 
            IElementFactory elementFactory, 
            IAirBallonFactory airBallonFactory)
        {
            _level = level;
            _staticDataService = staticDataService;
            _cameraService = cameraService;
            _progressService = progressService;
            _elementFactory = elementFactory;
            _airBallonFactory = airBallonFactory;
        }
        
        public LevelView Build()
        {
            SerializeLevel();
            FitBackground(_level);
            AdaptiveGrid(_level);
            CreateElements(_level);
            CreateAirBallons(_level);
            
            return _level;
        }

        private void SerializeLevel()
        {
            int length = _staticDataService.GetLevelConfig().Levels.Length;
            
            TextAsset asset = _staticDataService.GetLevelConfig().Levels[_progressService.LevelIndex.Value % length];
            
            if (string.IsNullOrEmpty(_progressService.LevelJson.Value))
            {
                _data = LevelSerializer.Deserialize(asset.text);
            }
            else
            {
                _data = LevelSerializer.Deserialize(_progressService.LevelJson.Value);

                if (_data.elements.Count == 0)
                {
                    _data = LevelSerializer.Deserialize(asset.text);
                }
            }
        }
        
        private void FitBackground(LevelView level)
        {
            Bounds bounds = level.Background.bounds;
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
        
        private void AdaptiveGrid(LevelView level)
        {
            Camera camera = _cameraService.MainCamera;
            GridConfig gridConfig = _staticDataService.GetGridConfig();
            GridView grid = level.Grid;
            
            float screenHeight = camera.orthographicSize * 2f;
            float screenWidth = screenHeight * camera.aspect;

            float availableWidth = screenWidth * (1f - gridConfig.ScreenPadding * 2f);
            float availableHeight = screenHeight * (1f - gridConfig.ScreenPadding * 2f);

            float maxSizeByWidth = availableWidth / _data.grid.gridSize.x;
            float maxSizeByHeight = availableHeight / _data.grid.gridSize.y;
            
            float cellSize = Mathf.Min(maxSizeByWidth, maxSizeByHeight);
            
            grid.Init(_data.grid.gridSize, cellSize);
        }
        
        private void CreateElements(LevelView level)
        {
            GridView grid = level.Grid;

            foreach (ElementData elementData in _data.elements)
            {
                if (_elementFactory.TryCreateElement(elementData.id, level.ElementsRoot, out ElementProvider element))
                {
                    element.GetData().view.SetPosition(elementData.position);
                    
                    element.transform.localScale = Vector3.one * grid.CellSize;
                    element.transform.position = grid.GetCenter(elementData.position.x, elementData.position.y);
                    
                    level.Elements.Add(element);
                }
            }
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
    }
}