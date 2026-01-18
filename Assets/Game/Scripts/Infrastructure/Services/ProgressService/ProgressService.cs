using System;
using JetBrains.Annotations;

namespace SwipeElements.Infrastructure.Services.ProgressService
{
    [UsedImplicitly]
    public sealed class ProgressService : IProgressService, IDisposable
    {
        public PrefsInt LevelIndex { get; } = new ();
        public PrefsString LevelJson { get; } = new ();
        
        public void Init()
        {
            LevelIndex.Load(DataKey.LEVEL_INDEX, 0);
            LevelJson.Load(DataKey.LEVEL_JSON, string.Empty);
        }

        public void Dispose()
        {
            LevelIndex.Dispose();
            LevelJson.Dispose();
        }
    }
}