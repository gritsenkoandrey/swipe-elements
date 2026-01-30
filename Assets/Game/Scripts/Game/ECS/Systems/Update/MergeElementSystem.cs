using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.StaticDataService;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

namespace SwipeElements.Game.ECS.Systems.Update
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MergeElementSystem : ISystem
    {
        private Filter _mergeFilter;
        private Filter _elementFilter;
        private Filter _gridFilter;
        private Filter _aliveFilter;
        private Filter _moveFilter;
        
        private Stash<MergeComponent> _mergeStash;
        private Stash<IdComponent> _idStash;
        private Stash<ElementTag> _elementStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<DestroyComponent> _destroyStash;
        private Stash<DelayComponent> _delayStash;
        private Stash<GridTag> _gridStash;
        private Stash<NormalizeComponent> _normalizeStash;
        
        private const int MIN_MERGE_COUNT = 3;
        
        private readonly float _destroyAnimationTime;

        public MergeElementSystem(IStaticDataService staticDataService)
        {
            _destroyAnimationTime = staticDataService.GetElementConfig().DestroyAnimationTime;
        }
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _mergeFilter = World.Filter
                .With<MergeComponent>()
                .Build();
            
            _elementFilter = World.Filter
                .With<ElementTag>()
                .With<TransformComponent>()
                .With<IdComponent>()
                .Without<MoveComponent>()
                .Without<SpeedComponent>()
                .Without<DestroyComponent>()
                .Build();
            
            _gridFilter = World.Filter
                .With<GridTag>()
                .Build();
            
            _moveFilter = World.Filter
                .With<MoveComponent>()
                .With<SpeedComponent>()
                .Build();
            
            _mergeStash = World.GetStash<MergeComponent>();
            _idStash = World.GetStash<IdComponent>();
            _elementStash = World.GetStash<ElementTag>();
            _transformStash = World.GetStash<TransformComponent>();
            _destroyStash = World.GetStash<DestroyComponent>();
            _delayStash = World.GetStash<DelayComponent>();
            _gridStash = World.GetStash<GridTag>();
            _normalizeStash = World.GetStash<NormalizeComponent>();
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
            
            Dictionary<Vector2Int, Entity> grid = DictionaryPool<Vector2Int, Entity>.Get();
            HashSet<Entity> candidates = HashSetPool<Entity>.Get();
            HashSet<int> columns = HashSetPool<int>.Get();
            
            Vector2Int size = GetGridView().Size;
            
            GenerateMap(grid);
            HorizontalScan(size, candidates, grid);
            VerticalScan(size, candidates, grid);
            DestroyMark(candidates, columns);
            NormalizeMark(columns, size, grid);
            
            DictionaryPool<Vector2Int, Entity>.Release(grid);
            HashSetPool<Entity>.Release(candidates);
            HashSetPool<int>.Release(columns);
        }
        
        public void Dispose()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GenerateMap(Dictionary<Vector2Int, Entity> grid)
        {
            foreach (Entity entity in _elementFilter)
            {
                ref ElementTag element = ref _elementStash.Get(entity);
                
                Vector2Int pos = element.view.Position;
                
                grid[pos] = entity;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HorizontalScan(Vector2Int size, HashSet<Entity> candidates, Dictionary<Vector2Int, Entity> grid)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Entity current = GetEntityAt(x, y, grid);
                    
                    if (current == default)
                    {
                        continue;
                    }
        
                    if (IsSameId(current, GetEntityAt(x - 1, y, grid)))
                    {
                        continue;
                    }
            
                    int count = 1;
                    
                    while (IsSameId(current, GetEntityAt(x + count, y, grid)))
                    {
                        count++;
                    }
            
                    if (count >= MIN_MERGE_COUNT)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            candidates.Add(GetEntityAt(x + i, y, grid));
                        }
                    }
                    
                    x += count - 1;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void VerticalScan(Vector2Int size, HashSet<Entity> candidates, Dictionary<Vector2Int, Entity> grid)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Entity current = GetEntityAt(x, y, grid);
                    
                    if (current == default)
                    {
                        continue;
                    }
        
                    if (IsSameId(current, GetEntityAt(x, y - 1, grid)))
                    {
                        continue;
                    }
            
                    int count = 1;
                    
                    while (IsSameId(current, GetEntityAt(x, y + count, grid)))
                    {
                        count++;
                    }
            
                    if (count >= MIN_MERGE_COUNT)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            candidates.Add(GetEntityAt(x, y + i, grid));
                        }
                    }
                    
                    y += count - 1;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Entity GetEntityAt(int x, int y, Dictionary<Vector2Int, Entity> grid)
        {
            return grid.GetValueOrDefault(new (x, y));
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GridView GetGridView()
        {
            Entity entity = _gridFilter.First();
            
            ref GridTag grid = ref _gridStash.Get(entity);
            
            return grid.view;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DestroyMark(HashSet<Entity> candidates, HashSet<int> columns)
        {
            foreach (Entity entity in candidates)
            {
                ref TransformComponent transform = ref _transformStash.Get(entity);
                ref ElementTag element = ref _elementStash.Get(entity);
                
                _destroyStash.AddOrGet(entity).gameObject = transform.transform.gameObject;
                _delayStash.AddOrGet(entity).time = _destroyAnimationTime;
                
                element.view.Animator.StartDestroyAnimation(_destroyAnimationTime);
                
                columns.Add(element.view.Position.x);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void NormalizeMark(HashSet<int> columns, Vector2Int size, Dictionary<Vector2Int, Entity> grid)
        {
            foreach (int x in columns)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Entity entity = GetEntityAt(x, y, grid);
        
                    if (entity == default || _destroyStash.Has(entity))
                    {
                        continue;
                    }
        
                    _normalizeStash.AddOrGet(entity);
                }
            }
        }
    }
}