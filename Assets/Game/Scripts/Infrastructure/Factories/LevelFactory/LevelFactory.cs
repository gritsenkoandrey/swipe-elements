using JetBrains.Annotations;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Builders;
using SwipeElements.Infrastructure.Services.LevelDataProvider;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;
using SwipeElements.Infrastructure.Services.StaticDataService;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Factories.LevelFactory
{
    [UsedImplicitly]
    public sealed class LevelFactory : ILevelFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IObjectResolver _objectResolver;
        private readonly ILevelDataProvider _levelDataProvider;
        private readonly ILevelBuilder _levelBuilder;
        
        public LevelFactory
        (
            IStaticDataService staticDataService, 
            IObjectResolver objectResolver, 
            ILevelDataProvider levelDataProvider,
            ILevelBuilder levelBuilder)
        {
            _staticDataService = staticDataService;
            _objectResolver = objectResolver;
            _levelDataProvider = levelDataProvider;
            _levelBuilder = levelBuilder;
        }

        LevelView ILevelFactory.CreateLevel()
        {
            LevelView level = _objectResolver.Instantiate(_staticDataService.GetLevelConfig().BaseLevel, null);
            LevelData data = _levelDataProvider.GetLevelData();
            _levelBuilder.Build(level, data);
            return level;
        }
    }
}