using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Infrastructure.Services.CameraService;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems.Update
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AirBallonMoveSystem : ISystem
    {
        private Filter _airBallonFilter;
        private Stash<SpeedComponent> _speedStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<SineComponent> _sineStash;
        
        private readonly float _rightX;
        private readonly float _leftX;
        
        public AirBallonMoveSystem(ICameraService cameraService)
        {
            (float right, float left) = cameraService.CalculateHorizontalScreenBounds();
            
            _leftX = left;
            _rightX = right;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _airBallonFilter = World.Filter
                .With<AirBallonTag>()
                .With<SpeedComponent>()
                .With<SineComponent>()
                .With<TransformComponent>()
                .Build();
            
            _speedStash = World.GetStash<SpeedComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _sineStash = World.GetStash<SineComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _airBallonFilter)
            {
                ref SpeedComponent speedComponent = ref _speedStash.Get(entity);
                ref TransformComponent transformComponent = ref _transformStash.Get(entity);
                ref SineComponent sineComponent = ref _sineStash.Get(entity);
                
                Vector3 position = transformComponent.transform.position;
                
                position.x += sineComponent.direction * speedComponent.speed * deltaTime;
                position.y += Mathf.Cos(Time.time * sineComponent.frequency + entity.Id) * sineComponent.amplitude * deltaTime;
                
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
    }
}