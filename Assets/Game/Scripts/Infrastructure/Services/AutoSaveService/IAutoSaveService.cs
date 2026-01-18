namespace SwipeElements.Infrastructure.Services.AutoSaveService
{
    public interface IAutoSaveService
    {
        void OnApplicationQuit();
        void OnApplicationPause(bool pauseStatus);
    }
}