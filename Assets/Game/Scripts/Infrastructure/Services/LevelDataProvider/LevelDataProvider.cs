using JetBrains.Annotations;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Extensions;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.StaticDataService;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.LevelDataProvider
{
    [UsedImplicitly]
    public sealed class LevelDataProvider : ILevelDataProvider
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        
        public LevelDataProvider(IStaticDataService staticDataService, IProgressService progressService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
        }
        
        LevelData ILevelDataProvider.GetLevelData()
        {
            int length = _staticDataService.GetLevelConfig().Levels.Length;
            
            TextAsset asset = _staticDataService.GetLevelConfig().Levels[_progressService.LevelIndex.Value % length];
            
            LevelData data;
            
            if (string.IsNullOrEmpty(_progressService.LevelJson.Value))
            {
                data = asset.text.Deserialize();
            }
            else
            {
                data = _progressService.LevelJson.Value.Deserialize();

                if (data.elements.Count == 0)
                {
                    data = asset.text.Deserialize();
                }
            }
            
            return data;
        }
    }
}