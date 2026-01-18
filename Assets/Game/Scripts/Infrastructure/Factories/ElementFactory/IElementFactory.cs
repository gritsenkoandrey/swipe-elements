using SwipeElements.Game.ECS.Providers;
using UnityEngine;

namespace SwipeElements.Infrastructure.Factories.ElementFactory
{
    public interface IElementFactory
    {
        void Init();
        bool TryCreateElement(string id, Transform root, out ElementProvider elementProvider);
    }
}