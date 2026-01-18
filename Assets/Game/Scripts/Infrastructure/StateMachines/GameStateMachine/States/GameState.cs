using SwipeElements.Infrastructure.Services.LoadingScreenService;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.ResultGameService;
using SwipeElements.UI;
using SwipeElements.UI.Screens;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class GameState : IEnterState
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ILoadingScreenService _loadingScreenService;
        private IScreenService _screenService;
        private IProgressService _progressService;
        private IResultGameService _resultGameService;
        
        private GameScreen _gameScreen;

        public GameState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct
        (
            ILoadingScreenService loadingScreenService,
            IScreenService screenService,
            IProgressService progressService,
            IResultGameService resultGameService
        )
        {
            _loadingScreenService = loadingScreenService;
            _screenService = screenService;
            _progressService = progressService;
            _resultGameService = resultGameService;
        }

        void IEnterState.Enter()
        {
            _loadingScreenService.Hide();
            
            ShowGameScreen();
        }

        void IExitState.Exit()
        {
            _screenService.TryHide<GameScreen>();
            
            _progressService.LevelJson.Value = string.Empty;
            _gameScreen.OnNextButtonClick -= OnNextButtonClick;
            _gameScreen.OnRestartButtonClick -= OnRestartButtonClick;
            _resultGameService.OnResultGame -= OnResultGame;
        }

        private void ShowGameScreen()
        {
            if (_screenService.TryShow(out GameScreen screen))
            {
                _gameScreen = screen;
                _gameScreen.OnNextButtonClick += OnNextButtonClick;
                _gameScreen.OnRestartButtonClick += OnRestartButtonClick;
            }
            
            _resultGameService.OnResultGame += OnResultGame;
        }

        private void OnNextButtonClick()
        {
            OnResultGame(true);
        }
        
        private void OnRestartButtonClick()
        {
            _gameStateMachine.Enter<RestartState, string>(SceneName.RESTART);
        }
        
        private void OnResultGame(bool isWin)
        {
            _gameStateMachine.Enter<ResultState, bool>(isWin);
        }
    }
}