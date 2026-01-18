namespace SwipeElements.Infrastructure.Services.ProgressService
{
    public interface IProgressService
    {
        void Init();
        PrefsInt LevelIndex { get; }
        PrefsString LevelJson { get; }
    }
}