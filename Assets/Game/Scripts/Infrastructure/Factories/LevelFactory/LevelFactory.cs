using JetBrains.Annotations;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Factories.AirBallonFactory;
using SwipeElements.Infrastructure.Factories.ElementFactory;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.StaticDataService;
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
            LevelView level = _objectResolver.Instantiate(_staticDataService.GetLevelConfig().BaseLevel, null);
            LevelBuilder builder = new (level, _staticDataService, _cameraService, _progressService, _elementFactory, _airBallonFactory);
            return builder.Build();
        }
    }
}