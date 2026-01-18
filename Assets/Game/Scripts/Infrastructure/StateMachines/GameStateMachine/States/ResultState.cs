using SwipeElements.Infrastructure.Services.ProgressService;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class ResultState : IEnterState<bool>
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private IProgressService _progressService;

        public ResultState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct
        (
            IProgressService progressService
        )
        {
            _progressService = progressService;
        }

        void IEnterState<bool>.Enter(bool param)
        {
            if (param)
            {
                _progressService.LevelIndex.Value++;
            }
            
            _gameStateMachine.Enter<RestartState, string>(SceneName.RESTART);
        }

        void IExitState.Exit()
        {
        }
    }
}