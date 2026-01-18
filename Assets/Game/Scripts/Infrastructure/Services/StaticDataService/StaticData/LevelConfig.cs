using SwipeElements.Game.Views;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.StaticDataService.StaticData
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "Configs/" + nameof(LevelConfig))]
    public sealed class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public LevelView BaseLevel { get; private set; }
        [field: SerializeField] public TextAsset[] Levels { get; private set; }
    }
}