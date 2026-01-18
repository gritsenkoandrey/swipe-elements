using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MergeElementSystem : ISystem
    {
        private readonly ElementConfig _config;
        
        private Filter _mergeFilter;
        private Filter _elementFilter;
        private Filter _gridFilter;
        private Filter _aliveFilter;
        private Filter _moveFilter;
        
        private Stash<MergeComponent> _mergeStash;
        private Stash<IdComponent> _idStash;
        private Stash<ElementComponent> _elementStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<DestroyComponent> _destroyStash;
        private Stash<DelayComponent> _delayStash;
        private Stash<GridComponent> _gridStash;
        
        private readonly Dictionary<Vector2Int, Entity> _grid = new ();
        private readonly HashSet<Entity> _candidates = new ();
        
        private const int MIN_MERGE_COUNT = 3;

        public MergeElementSystem(IStaticDataService staticDataService)
        {
            _config = staticDataService.GetElementConfig();
        }
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _mergeFilter = World.Filter
                .With<MergeComponent>()
                .Build();
            
            _elementFilter = World.Filter
                .With<ElementComponent>()
                .With<TransformComponent>()
                .With<IdComponent>()
                .Without<MoveComponent>()
                .Without<SpeedComponent>()
                .Without<DestroyComponent>()
                .Build();
            
            _gridFilter = World.Filter
                .With<GridComponent>()
                .Build();
            
            _moveFilter = World.Filter
                .With<MoveComponent>()
                .With<SpeedComponent>()
                .Build();
            
            _mergeStash = World.GetStash<MergeComponent>();
            _idStash = World.GetStash<IdComponent>();
            _elementStash = World.GetStash<ElementComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _destroyStash = World.GetStash<DestroyComponent>();
            _delayStash = World.GetStash<DelayComponent>();
            _gridStash = World.GetStash<GridComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_mergeFilter.IsEmpty() || _moveFilter.IsNotEmpty())
            {
                return;
            }
            
            foreach (Entity entity in _mergeFilter)
            {
                _mergeStash.Remove(entity);
            }
            
            _grid.Clear();
            _candidates.Clear();
            
            Vector2Int size = GetGridView().Size;
            
            GenerateMap();
            HorizontalScan(size);
            VerticalScan(size);
            DestroyElements();
        }
        
        public void Dispose()
        {
            _candidates.Clear();
            _grid.Clear();
        }

        private void GenerateMap()
        {
            foreach (Entity entity in _elementFilter)
            {
                ref ElementComponent element = ref _elementStash.Get(entity);
                
                Vector2Int pos = element.view.Position;
                
                _grid[pos] = entity;
            }
        }

        private void HorizontalScan(Vector2Int size)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Entity current = GetEntityAt(x, y);
                    
                    if (current == default)
                    {
                        continue;
                    }
        
                    if (IsSameId(current, GetEntityAt(x - 1, y)))
                    {
                        continue;
                    }
            
                    int count = 1;
                    
                    while (IsSameId(current, GetEntityAt(x + count, y)))
                    {
                        count++;
                    }
            
                    if (count >= MIN_MERGE_COUNT)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            _candidates.Add(GetEntityAt(x + i, y));
                        }
                    }
                    
                    x += count - 1;
                }
            }
        }

        private void VerticalScan(Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Entity current = GetEntityAt(x, y);
                    
                    if (current == default)
                    {
                        continue;
                    }
        
                    if (IsSameId(current, GetEntityAt(x, y - 1)))
                    {
                        continue;
                    }
            
                    int count = 1;
                    
                    while (IsSameId(current, GetEntityAt(x, y + count)))
                    {
                        count++;
                    }
            
                    if (count >= MIN_MERGE_COUNT)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            _candidates.Add(GetEntityAt(x, y + i));
                        }
                    }
                    
                    y += count - 1;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Entity GetEntityAt(int x, int y)
        {
            return _grid.GetValueOrDefault(new (x, y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsSameId(Entity a, Entity b)
        {
            if (a == default || b == default)
            {
                return false;
            }
            
            return _idStash.Get(a).id == _idStash.Get(b).id;
        }
        
        private GridView GetGridView()
        {
            Entity entity = _gridFilter.First();
            
            ref GridComponent grid = ref _gridStash.Get(entity);
            
            return grid.view;
        }
        
        private void DestroyElements()
        {
            foreach (Entity entity in _candidates)
            {
                ref TransformComponent transform = ref _transformStash.Get(entity);
                ref ElementComponent element = ref _elementStash.Get(entity);
                
                _destroyStash.AddOrGet(entity).gameObject = transform.transform.gameObject;
                _delayStash.AddOrGet(entity).value = _config.DestroyAnimationTime;
                
                element.view.StartDestroyAnimation(_config.DestroyAnimationTime);
            }
        }
    }
}