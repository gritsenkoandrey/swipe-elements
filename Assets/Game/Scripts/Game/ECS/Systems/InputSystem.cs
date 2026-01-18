using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.PhysicsService;
using SwipeElements.Utils;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputSystem : ISystem
    {
        private readonly IPhysicsService _physicsService;
        private readonly ICameraService _cameraService;
        
        private Filter _selectFilter;
        private Stash<SelectComponent> _selectStash;
        private Stash<SwipeComponent> _swipeStash;
        private Stash<DestroyComponent> _destroyStash;
        private Stash<MoveComponent> _moveStash;
        private Stash<MergeComponent> _mergeStash;
        private Stash<NormalizeComponent> _normalizeStash;
        
        public InputSystem(IPhysicsService physicsService, ICameraService cameraService)
        {
            _physicsService = physicsService;
            _cameraService = cameraService;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _selectFilter = World.Filter
                .With<SelectComponent>()
                .Build();
            
            _selectStash = World.GetStash<SelectComponent>();
            _swipeStash = World.GetStash<SwipeComponent>();
            _destroyStash = World.GetStash<DestroyComponent>();
            _moveStash = World.GetStash<MoveComponent>();
            _mergeStash = World.GetStash<MergeComponent>();
            _normalizeStash = World.GetStash<NormalizeComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnStartClick(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                OnEndClick(Input.mousePosition);
            }
        }

        public void Dispose()
        {
        }

        private void OnStartClick(Vector3 position)
        {
            Ray ray = _cameraService.MainCamera.ScreenPointToRay(position);

            if (_physicsService.TryRayCastHit(ray, 1 << Layers.Element, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out ElementProvider element))
                {
                    if (EntityIsActualize(element))
                    {
                        return;
                    }
                    
                    ref SelectComponent select = ref _selectStash.Add(element.Entity);

                    select.start = position;
                }
            }
        }

        private void OnEndClick(Vector3 position)
        {
            foreach (Entity entity in _selectFilter)
            {
                if (_selectStash.Has(entity))
                {
                    ref SelectComponent select = ref _selectStash.Get(entity);
                
                    select.end = position;
                
                    Vector3 direction = select.end - select.start;
                
                    ref SwipeComponent swipe = ref _swipeStash.Add(entity);
                
                    swipe.direction = new (direction.x, direction.y);
                }
                
                _selectStash.Remove(entity);
            }
        }
        
        private bool EntityIsActualize(ElementProvider element)
        {
            return _destroyStash.Has(element.Entity) || 
                   _moveStash.Has(element.Entity) || 
                   _mergeStash.Has(element.Entity) || 
                   _normalizeStash.Has(element.Entity);
        }
    }
}