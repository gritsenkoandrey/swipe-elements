using UnityEngine;

namespace SwipeElements.Infrastructure.Services.StaticDataService.StaticData
{
    [CreateAssetMenu(fileName = nameof(InputConfig), menuName = "Configs/" + nameof(InputConfig))]
    public sealed class InputConfig : ScriptableObject
    {
        [field: SerializeField] public float SwipeThreshold { get; private set; } = 100f;
    }
}