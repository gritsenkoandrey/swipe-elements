using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Factories.SystemFactory;
using SwipeElements.Infrastructure.Serialize;
using SwipeElements.Infrastructure.Services.ExitApplicationService;
using SwipeElements.Infrastructure.Services.LoadingScreenService;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.ResultGameService;
using SwipeElements.UI;
using SwipeElements.UI.Screens;
using UnityEngine;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine.States
{
    public sealed class GameState : IEnterState<LevelView>
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ILoadingScreenService _loadingScreenService;
        private IScreenService _screenService;
        private IProgressService _progressService;
        private IResultGameService _resultGameService;
        private IExitApplicationService _exitApplicationService;
        private ISystemFactory _systemFactory;
        
        private GameScreen _gameScreen;
        private LevelView _levelView;

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
            IResultGameService resultGameService,
            IExitApplicationService exitApplicationService,
            ISystemFactory systemFactory
        )
        {
            _loadingScreenService = loadingScreenService;
            _screenService = screenService;
            _progressService = progressService;
            _resultGameService = resultGameService;
            _exitApplicationService = exitApplicationService;
            _systemFactory = systemFactory;
        }

        void IEnterState<LevelView>.Enter(LevelView param)
        {
            _levelView = param;
            _loadingScreenService.Hide();
            _exitApplicationService.OnExitGame += OnExitGame;
            _resultGameService.OnResultGame += OnResultGame;
            
            ShowGameScreen();
        }

        void IExitState.Exit()
        {
            _screenService.TryHide<GameScreen>();
            
            _progressService.LevelJson.Value = string.Empty;

            if (_gameScreen)
            {
                _gameScreen.OnNextButtonClick -= OnNextButtonClick;
                _gameScreen.OnRestartButtonClick -= OnRestartButtonClick;
            }
            
            _resultGameService.OnResultGame -= OnResultGame;
            _exitApplicationService.OnExitGame -= OnExitGame;
            _levelView = null;
            _gameScreen = null;
            _systemFactory.Cleanup();
        }

        private void ShowGameScreen()
        {
            if (_screenService.TryShow(out GameScreen screen))
            {
                _gameScreen = screen;
                _gameScreen.OnNextButtonClick += OnNextButtonClick;
                _gameScreen.OnRestartButtonClick += OnRestartButtonClick;
            }
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

        private void OnExitGame()
        {
            _progressService.LevelJson.Value = _levelView.Serialize();
            
            Debug.Log("Save Game");
        }
    }
}