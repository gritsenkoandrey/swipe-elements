using JetBrains.Annotations;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Extensions;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Factories.AirBallonFactory;
using SwipeElements.Infrastructure.Factories.ElementFactory;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.GridService;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using UnityEngine;

namespace SwipeElements.Infrastructure.Builders
{
    [UsedImplicitly]
    public sealed class LevelBuilder : ILevelBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICameraService _cameraService;
        private readonly IElementFactory _elementFactory;
        private readonly IAirBallonFactory _airBallonFactory;
        private readonly IGridService _gridService;

        public LevelBuilder
        (
            IStaticDataService staticDataService, 
            ICameraService cameraService, 
            IElementFactory elementFactory, 
            IAirBallonFactory airBallonFactory, 
            IGridService gridService
        )
        {
            _staticDataService = staticDataService;
            _cameraService = cameraService;
            _elementFactory = elementFactory;
            _airBallonFactory = airBallonFactory;
            _gridService = gridService;
        }
        
        void ILevelBuilder.Build(LevelView level, LevelData data)
        {
            _cameraService.AdaptiveCamera(level.Background.bounds);
            _gridService.AdaptiveGrid(level.Grid, data);
            
            CreateElements(level, data);
            CreateBallons(level);
        }

        private void CreateElements(LevelView level, LevelData data)
        {
            GridView grid = level.Grid;

            foreach (ElementData elementData in data.elements)
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

        private void CreateBallons(LevelView level)
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