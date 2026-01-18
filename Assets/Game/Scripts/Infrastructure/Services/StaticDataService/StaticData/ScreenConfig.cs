using SwipeElements.UI.Screens;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.StaticDataService.StaticData
{
    [CreateAssetMenu(fileName = nameof(ScreenConfig), menuName = "Configs/" + nameof(ScreenConfig))]
    public sealed class ScreenConfig : ScriptableObject
    {
        [field: SerializeField] public BaseScreen[] Screens { get; private set; }
    }
}