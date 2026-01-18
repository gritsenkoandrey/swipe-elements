using JetBrains.Annotations;
using SwipeElements.Infrastructure.Factories.LevelFactory;
using SwipeElements.Infrastructure.Factories.SystemFactory;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Scopes.EntryPoints
{
    [UsedImplicitly]
    public sealed class GameEntryPoint : IInitializable
    {
        private readonly ISystemFactory _systemFactory;
        private readonly ILevelFactory _levelFactory;
        
        public GameEntryPoint(ISystemFactory systemFactory, ILevelFactory levelFactory)
        {
            _systemFactory = systemFactory;
            _levelFactory = levelFactory;
        }
        
        void IInitializable.Initialize()
        {
            _systemFactory.CreateGameSystems();
            _levelFactory.CreateLevel();
        }
    }
}