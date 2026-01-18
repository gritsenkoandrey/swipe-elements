using JetBrains.Annotations;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.AssetService
{
    [UsedImplicitly]
    public sealed class AssetService : IAssetService
    {
        T IAssetService.LoadFromResources<T>(string path) => Resources.Load<T>(path);
        T[] IAssetService.LoadAllFromResources<T>(string path) => Resources.LoadAll<T>(path);
        void IAssetService.CleanUp() => Resources.UnloadUnusedAssets();
    }
}