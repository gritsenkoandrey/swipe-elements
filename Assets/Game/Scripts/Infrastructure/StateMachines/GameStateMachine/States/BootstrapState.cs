using SwipeElements.Infrastructure.Services.LoadingScreenService;
using SwipeElements.Infrastructure.Services.SceneLoadService;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class BootstrapState : IEnterState
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ISceneLoadService _sceneLoadService;
        private ILoadingScreenService _loadingScreenService;

        public BootstrapState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct(ISceneLoadService sceneLoadService, ILoadingScreenService loadingScreenService)
        {
            _sceneLoadService = sceneLoadService;
            _loadingScreenService = loadingScreenService;
        }

        void IEnterState.Enter()
        {
            _loadingScreenService.Show();
            _sceneLoadService.Load(SceneName.BOOTSTRAP, EnterPrepareState);
        }

        void IExitState.Exit()
        {
        }

        private void EnterPrepareState() => _gameStateMachine.Enter<PrepareState>();
    }
}