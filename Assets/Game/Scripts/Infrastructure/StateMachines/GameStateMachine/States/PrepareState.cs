using SwipeElements.Infrastructure.Factories.ElementFactory;
using SwipeElements.Infrastructure.Factories.SystemFactory;
using SwipeElements.Infrastructure.Services.ApplicationService;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.StaticDataService;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class PrepareState : IEnterState
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private IStaticDataService _staticDataService;
        private IElementFactory _elementFactory;
        private IProgressService _progressService;
        private IApplicationService _applicationService;
        private ISystemFactory _systemFactory;

        public PrepareState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct
        (
            IStaticDataService staticDataService,
            IElementFactory elementFactory,
            IProgressService progressService,
            IApplicationService applicationService,
            ISystemFactory systemFactory
        )
        {
            _staticDataService = staticDataService;
            _elementFactory = elementFactory;
            _progressService = progressService;
            _applicationService = applicationService;
            _systemFactory = systemFactory;
        }

        void IEnterState.Enter()
        {
            _staticDataService.Init();
            _progressService.Init();
            _elementFactory.Init();
            _systemFactory.Init();
            _applicationService.Init();
            
            _gameStateMachine.Enter<LoadLevelState, string>(SceneName.GAME);
        }

        void IExitState.Exit()
        {
        }
    }
}