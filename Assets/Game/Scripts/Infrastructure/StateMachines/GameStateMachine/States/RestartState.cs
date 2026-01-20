using SwipeElements.Infrastructure.Services.AssetService;
using SwipeElements.Infrastructure.Services.SceneLoadService;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class RestartState : IEnterState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ISceneLoadService _sceneLoadService;
        private IAssetService _assetService;

        public RestartState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct(ISceneLoadService sceneLoadService, IAssetService assetService)
        {
            _sceneLoadService = sceneLoadService;
            _assetService = assetService;
        }
        
        void IEnterState<string>.Enter(string param)
        {
            _assetService.CleanUp();
            _sceneLoadService.Load(param, EnterLoadLevelState);
        }
        
        void IExitState.Exit()
        {
        }

        private void EnterLoadLevelState()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(SceneName.GAME);
        }
    }
}