using SwipeElements.Infrastructure.Factories.LevelFactory;
using SwipeElements.Infrastructure.Serialize;
using SwipeElements.Infrastructure.Services.ProgressService;
using UnityEngine;
using VContainer;

namespace SwipeElements.Infrastructure.Services.AutoSaveService
{
    public sealed class AutoSaveService : MonoBehaviour, IAutoSaveService
    {
        private ILevelFactory _levelFactory;
        private IProgressService _progressService;

        [Inject]
        private void Construct(ILevelFactory levelFactory, IProgressService progressService)
        {
            _levelFactory = levelFactory;
            _progressService = progressService;
        }
        
        public void OnApplicationQuit()
        {
            TrySaveLevel();
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                TrySaveLevel();
            }
        }
        
        private void TrySaveLevel()
        {
            if (_levelFactory.Level != null)
            {
                _progressService.LevelJson.Value = _levelFactory.Level.Serialize();
                
                Debug.Log("Level Saved");
            }
        }
    }
}