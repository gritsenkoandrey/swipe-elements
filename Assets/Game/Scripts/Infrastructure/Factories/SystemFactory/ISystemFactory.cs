namespace SwipeElements.Infrastructure.Factories.SystemFactory
{
    public interface ISystemFactory
    {
        void Init();
        void CreateGameSystems();
        void Cleanup();
    }
}