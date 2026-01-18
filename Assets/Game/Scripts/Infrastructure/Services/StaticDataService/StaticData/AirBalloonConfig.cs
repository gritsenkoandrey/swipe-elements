using SwipeElements.Game.ECS.Providers;
using SwipeElements.Utils;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.StaticDataService.StaticData
{
    [CreateAssetMenu(fileName = nameof(AirBalloonConfig), menuName = "Configs/" + nameof(AirBalloonConfig))]
    public sealed class AirBalloonConfig : ScriptableObject
    {
        [field: SerializeField] public AirBallonProvider[] AirBallons { get; private set; }
        [field: SerializeField] public MinMaxInt MinMaxCount { get; private set; }
        [field: SerializeField] public MinMaxFloat MinMaxSpeed { get; private set; }
        [field: SerializeField] public MinMaxFloat MinMaxAmplitude { get; private set; }
        [field: SerializeField] public MinMaxFloat MinMaxFrequency { get; private set; }
    }
}