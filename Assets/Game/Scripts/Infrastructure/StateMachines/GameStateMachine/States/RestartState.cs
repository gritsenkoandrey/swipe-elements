using SwipeElements.Infrastructure.Services.SceneLoadService;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class RestartState : IEnterState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ISceneLoadService _sceneLoadService;

        public RestartState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct(ISceneLoadService sceneLoadService)
        {
            _sceneLoadService = sceneLoadService;
        }
        
        void IEnterState<string>.Enter(string param)
        {
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