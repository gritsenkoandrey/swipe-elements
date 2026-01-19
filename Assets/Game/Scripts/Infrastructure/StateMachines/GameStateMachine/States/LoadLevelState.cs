using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Factories.LevelFactory;
using SwipeElements.Infrastructure.Factories.SystemFactory;
using SwipeElements.Infrastructure.Services.LoadingScreenService;
using SwipeElements.Infrastructure.Services.SceneLoadService;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class LoadLevelState : IEnterState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ISceneLoadService _sceneLoadService;
        private ILoadingScreenService _loadingScreenService;
        private ISystemFactory _systemFactory;
        private ILevelFactory _levelFactory;

        public LoadLevelState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct
        (
            ISceneLoadService sceneLoadService,
            ILoadingScreenService loadingScreenService,
            ISystemFactory systemFactory,
            ILevelFactory levelFactory
        )
        {
            _sceneLoadService = sceneLoadService;
            _loadingScreenService = loadingScreenService;
            _systemFactory = systemFactory;
            _levelFactory = levelFactory;
        }

        void IEnterState<string>.Enter(string param)
        {
            _loadingScreenService.Show();
            _sceneLoadService.Load(param, EnterGameState);
        }
        
        void IExitState.Exit()
        {
        }
        
        private void EnterGameState()
        {
            _systemFactory.CreateGameSystems();
            
            LevelView level = _levelFactory.CreateLevel();
            
            _gameStateMachine.Enter<GameState, LevelView>(level);
        }
    }
}