using System;

namespace SwipeElements.Infrastructure.Services.SceneLoadService
{
    public interface ISceneLoadService
    {
        void Load(string name, Action onLoaded);
    }
}