using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using SwipeElements.Utils;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems.Initialize
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AirBallonInitializeSystem : ISystem
    {
        private Filter _airBallonFilter;
        private Stash<SpeedComponent> _speedStash;
        private Stash<SineComponent> _sineStash;
        private Stash<InitializeComponent> _initializeStash;
        
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
                .Without<SineComponent>()
                .Without<InitializeComponent>()
                .Build();
            
            _speedStash = World.GetStash<SpeedComponent>();
            _sineStash = World.GetStash<SineComponent>();
            _initializeStash = World.GetStash<InitializeComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _airBallonFilter)
            {
                ref SpeedComponent speedComponent = ref _speedStash.Add(entity);
                speedComponent.speed = Random.Range(_minMaxSpeed.Min, _minMaxSpeed.Max);

                ref SineComponent sineComponent = ref _sineStash.Add(entity);
                sineComponent.amplitude = Random.Range(_minMaxAmplitude.Min, _minMaxAmplitude.Max);
                sineComponent.frequency = Random.Range(_minMaxFrequency.Min, _minMaxFrequency.Max);
                sineComponent.direction = entity.Id % 2 == 0 ? 1f : -1f;
                
                _initializeStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}