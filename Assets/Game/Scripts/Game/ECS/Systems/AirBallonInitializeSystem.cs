using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using SwipeElements.Utils;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AirBallonInitializeSystem : ISystem
    {
        private Filter _airBallonFilter;
        private Stash<SpeedComponent> _speedStash;
        private Stash<SineMovementComponent> _sineMovementStash;
        private Stash<CleanupComponent> _cleanupStash;
        
        private readonly MinMaxFloat _minMaxSpeed;
        private readonly MinMaxFloat _minMaxAmplitude;
        private readonly MinMaxFloat _minMaxFrequency;

        public AirBallonInitializeSystem(IStaticDataService staticDataService)
        {
            AirBalloonConfig config = staticDataService.GetAirBallonConfig();
            
            _minMaxSpeed = config.MinMaxSpeed;
            _minMaxAmplitude = config.MinMaxAmplitude;
            _minMaxFrequency = config.MinMaxFrequency;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _airBallonFilter = World.Filter
                .With<AirBallonTag>()
                .Without<SpeedComponent>()
                .Without<SineMovementComponent>()
                .Build();
            
            _speedStash = World.GetStash<SpeedComponent>();
            _sineMovementStash = World.GetStash<SineMovementComponent>();
            _cleanupStash = World.GetStash<CleanupComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _airBallonFilter)
            {
                ref SpeedComponent speedComponent = ref _speedStash.Add(entity);
                speedComponent.speed = Random.Range(_minMaxSpeed.Min, _minMaxSpeed.Max);

                ref SineMovementComponent sinusoidComponent = ref _sineMovementStash.Add(entity);
                sinusoidComponent.amplitude = Random.Range(_minMaxAmplitude.Min, _minMaxAmplitude.Max);
                sinusoidComponent.frequency = Random.Range(_minMaxFrequency.Min, _minMaxFrequency.Max);

                _cleanupStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}