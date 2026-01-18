using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Infrastructure.Services.CameraService;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AirBallonMoveSystem : ISystem
    {
        private readonly ICameraService _cameraService;
        
        private Filter _airBallonFilter;
        private Stash<SpeedComponent> _speedStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<SinusoidalMovementComponent> _sinusoidalMovementStash;
        
        private float _rightX;
        private float _leftX;
        
        public AirBallonMoveSystem(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _airBallonFilter = World.Filter
                .With<AirBallonTag>()
                .With<SpeedComponent>()
                .With<SinusoidalMovementComponent>()
                .With<TransformComponent>()
                .Build();
            
            _speedStash = World.GetStash<SpeedComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _sinusoidalMovementStash = World.GetStash<SinusoidalMovementComponent>();
            
            CalculateScreenBounds();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _airBallonFilter)
            {
                ref SpeedComponent speedComponent = ref _speedStash.Get(entity);
                ref TransformComponent transformComponent = ref _transformStash.Get(entity);
                ref SinusoidalMovementComponent sinusoidalComponent = ref _sinusoidalMovementStash.Get(entity);
                
                Vector3 position = transformComponent.transform.position;
                
                float direction = (entity.Id % 2 == 0) ? 1f : -1f;
                position.x += direction * speedComponent.speed * deltaTime;
                position.y += Mathf.Cos(Time.time * sinusoidalComponent.frequency + entity.Id) * sinusoidalComponent.amplitude * deltaTime;
                
                if (position.x > _rightX)
                {
                    position.x = _leftX;
                }
                else if (position.x < _leftX)
                {
                    position.x = _rightX;
                }
                
                transformComponent.transform.position = position;
            }
        }
        
        public void Dispose()
        {
        }
        
        private void CalculateScreenBounds()
        {
            Camera camera = _cameraService.MainCamera;
            float screenHeight = 2f * camera.orthographicSize;
            float screenWidth = screenHeight * camera.aspect;
            
            _rightX = camera.transform.position.x + screenWidth / 2f + 1f;
            _leftX = camera.transform.position.x - screenWidth / 2f - 1f;
        }
    }
}