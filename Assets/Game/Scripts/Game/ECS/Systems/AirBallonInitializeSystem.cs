using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AirBallonInitializeSystem : ISystem
    {
        private readonly AirBalloonConfig _config;
        
        private Filter _airBallonFilter;
        private Stash<SpeedComponent> _speedStash;
        private Stash<SineMovementComponent> _sineMovementStash;
        private Stash<CleanupComponent> _cleanupStash;

        public AirBallonInitializeSystem(IStaticDataService staticDataService)
        {
            _config = staticDataService.GetAirBallonConfig();
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
                speedComponent.speed = Random.Range(_config.MinMaxSpeed.Min, _config.MinMaxSpeed.Max);

                ref SineMovementComponent sinusoidComponent = ref _sineMovementStash.Add(entity);
                sinusoidComponent.amplitude = Random.Range(_config.MinMaxAmplitude.Min, _config.MinMaxAmplitude.Max);
                sinusoidComponent.frequency = Random.Range(_config.MinMaxFrequency.Min, _config.MinMaxFrequency.Max);

                _cleanupStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}