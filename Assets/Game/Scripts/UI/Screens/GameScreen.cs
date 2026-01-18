using System;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeElements.UI.Screens
{
    public sealed class GameScreen : BaseScreen
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextButton;
        
        public event Action OnRestartButtonClick = delegate { };
        public event Action OnNextButtonClick = delegate { };

        protected override void Subscribe()
        {
            _restartButton.onClick.AddListener(RestartButtonClick);
            _nextButton.onClick.AddListener(NextButtonClick);
        }
        
        protected override void Unsubscribe()
        {
            _restartButton.onClick.RemoveListener(RestartButtonClick);
            _nextButton.onClick.RemoveListener(NextButtonClick);
        }

        private void RestartButtonClick() => OnRestartButtonClick.Invoke();
        private void NextButtonClick() => OnNextButtonClick.Invoke();
    }
}