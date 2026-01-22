using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Game.Extensions;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.StaticDataService;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems.Update
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SwipeElementSystem : ISystem
    {
        private Filter _swipeFilter;
        private Filter _elementFilter;
        private Filter _gridFilter;
        
        private Stash<SwipeComponent> _swipeStash;
        private Stash<ElementTag> _elementStash;
        private Stash<GridTag> _gridStash;
        private Stash<MoveComponent> _moveStash;
        private Stash<SpeedComponent> _speedStash;

        private readonly float _swipeSpeed;

        public SwipeElementSystem(IStaticDataService staticDataService)
        {
            _swipeSpeed = staticDataService.GetElementConfig().SwipeSpeed;
        }
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _swipeFilter = World.Filter
                .With<SwipeComponent>()
                .With<ElementTag>()
                .Build();
            
            _elementFilter = World.Filter
                .With<ElementTag>()
                .Without<DestroyComponent>()
                .Build();
            
            _gridFilter = World.Filter
                .With<GridTag>()
                .Build();
            
            _swipeStash = World.GetStash<SwipeComponent>();
            _elementStash = World.GetStash<ElementTag>();
            _gridStash = World.GetStash<GridTag>();
            _moveStash = World.GetStash<MoveComponent>();
            _speedStash = World.GetStash<SpeedComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _swipeFilter)
            {
                ref SwipeComponent swipe = ref _swipeStash.Get(entity);
                ref ElementTag element = ref _elementStash.Get(entity);
                
                Vector2Int axis = GetAxis(swipe.direction);
                GridView grid = GetGridView();
                
                Vector2Int currentPosition = element.view.Position;
                Vector2Int targetPosition = element.view.Position + axis;
                
                if (TryGetElementAt(targetPosition, out Entity neighborEntity))
                {
                    ref ElementTag neighborElement = ref _elementStash.Get(neighborEntity);

                    element.view.SetPosition(targetPosition);
                    neighborElement.view.SetPosition(currentPosition);
                    
                    _moveStash.AddOrGet(entity).to = grid.GetCenter(targetPosition.x, targetPosition.y);
                    _moveStash.AddOrGet(neighborEntity).to = grid.GetCenter(currentPosition.x, currentPosition.y);
                    _speedStash.AddOrGet(entity).speed = _swipeSpeed;
                    _speedStash.AddOrGet(neighborEntity).speed = _swipeSpeed;
                }
                else if (axis == Vector2Int.left && targetPosition.x >= 0 || 
                         axis == Vector2Int.right && targetPosition.x < grid.Size.x)
                {
                    element.view.SetPosition(targetPosition);
                    
                    _moveStash.AddOrGet(entity).to = grid.GetCenter(targetPosition.x, targetPosition.y);
                    _speedStash.AddOrGet(entity).speed = _swipeSpeed;
                }
                
                _swipeStash.Remove(entity);
            }
        }
        
        public void Dispose()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2Int GetAxis(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                return direction.x > 0 ? Vector2Int.right : Vector2Int.left;
            }
            
            return direction.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GridView GetGridView()
        {
            Entity entity = _gridFilter.First();
            
            ref GridTag grid = ref _gridStash.Get(entity);
            
            return grid.view;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetElementAt(Vector2Int position, out Entity result)
        {
            foreach (Entity entity in _elementFilter)
            {
                ref ElementTag element = ref _elementStash.Get(entity);
                
                if (element.view.Position == position)
                {
                    result = entity;
                    return true;
                }
            }
            
            result = default;
            return false;
        }
    }
}