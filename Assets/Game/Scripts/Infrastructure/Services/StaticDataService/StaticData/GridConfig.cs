using UnityEngine;

namespace SwipeElements.Infrastructure.Services.StaticDataService.StaticData
{
    [CreateAssetMenu(fileName = nameof(GridConfig), menuName = "Configs/" + nameof(GridConfig))]
    public sealed class GridConfig : ScriptableObject
    {
        [field: SerializeField] public float ScreenPadding { get; private set; }
    }
}