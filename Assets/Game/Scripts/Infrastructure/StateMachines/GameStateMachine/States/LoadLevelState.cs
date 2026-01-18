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

        public LoadLevelState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct
        (
            ISceneLoadService sceneLoadService,
            ILoadingScreenService loadingScreenService
        )
        {
            _sceneLoadService = sceneLoadService;
            _loadingScreenService = loadingScreenService;
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
            _gameStateMachine.Enter<GameState>();
        }
    }
}