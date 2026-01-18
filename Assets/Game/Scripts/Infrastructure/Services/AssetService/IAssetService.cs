using UnityEngine;

namespace SwipeElements.Infrastructure.Services.AssetService
{
    public interface IAssetService
    {
        T LoadFromResources<T>(string path) where T : Object;
        T[] LoadAllFromResources<T>(string path) where T : Object;
        void CleanUp();
    }
}