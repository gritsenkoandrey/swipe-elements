using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
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
    public sealed class NormalizeElementSystem : ISystem
    {
        private readonly ElementConfig _config;
        
        private Filter _elementFilter;
        private Filter _gridFilter;
        private Filter _normalizeFilter;
        private Filter _moveFilter;
        private Filter _destroyFilter;
        private Stash<ElementComponent> _elementStash;
        private Stash<MoveComponent> _moveStash;
        private Stash<SpeedComponent> _speedStash;
        private Stash<GridComponent> _gridStash;
        private Stash<NormalizeComponent> _normalizeStash;
        
        private readonly FastList<SortableEntity> _sortBuffer = new ();

        public NormalizeElementSystem(IStaticDataService staticDataService)
        {
            _config = staticDataService.GetElementConfig();
        }
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _elementFilter = World.Filter.With<ElementComponent>().Build();
            _gridFilter = World.Filter.With<GridComponent>().Build();
            _normalizeFilter = World.Filter.With<NormalizeComponent>().Build();
            _destroyFilter = World.Filter.With<DestroyComponent>().Build();
            _moveFilter = World.Filter.With<MoveComponent>().Build();
            _normalizeStash = World.GetStash<NormalizeComponent>();
            _elementStash = World.GetStash<ElementComponent>();
            _moveStash = World.GetStash<MoveComponent>();
            _gridStash = World.GetStash<GridComponent>();
            _speedStash = World.GetStash<SpeedComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_normalizeFilter.IsEmpty() || _moveFilter.IsNotEmpty() || _destroyFilter.IsNotEmpty())
            {
                return;
            }

            foreach (Entity entity in _normalizeFilter)
            {
                _normalizeStash.Remove(entity);
            }

            foreach (Entity entity in _elementFilter)
            {
                ref ElementComponent element = ref _elementStash.Get(entity);
                
                _sortBuffer.Add(new () 
                { 
                    entity = entity, 
                    x = element.view.Position.x, 
                    y = element.view.Position.y 
                });
            }

            if (_sortBuffer.capacity == 0)
            {
                return;
            }

            _sortBuffer.Sort();

            GridView gridView = GetGridView();
            Vector3 origin = gridView.GetOrigin();
            
            int currentX = int.MaxValue;
            int targetY = 0;

            foreach (SortableEntity item in _sortBuffer)
            {
                Entity entity = item.entity;
                
                if (item.x != currentX)
                {
                    currentX = item.x;
                    targetY = 0;
                }
                
                if (item.y != targetY)
                {
                    ref ElementComponent element = ref _elementStash.Get(entity);
                    element.view.SetPosition(new (currentX, targetY));
                    
                    ref MoveComponent move = ref _moveStash.AddOrGet(entity);
                    move.to = gridView.GetCenter(origin, currentX, targetY);
                    
                    ref SpeedComponent speed = ref _speedStash.AddOrGet(entity);
                    speed.speed = _config.GravitySpeed;
                }

                targetY++;
            }
            
            _sortBuffer.Clear();
        }
        
        private GridView GetGridView()
        {
            Entity entity = _gridFilter.First();
            
            ref GridComponent grid = ref _gridStash.Get(entity);
            
            return grid.view;
        }
        
        private struct SortableEntity : IComparable<SortableEntity>
        {
            public Entity entity;
            public int x;
            public int y;

            public int CompareTo(SortableEntity other)
            {
                int deltaX = x - other.x;
                
                if (deltaX != 0)
                {
                    return deltaX;
                }
                
                return y - other.y;
            }
        }
        
        public void Dispose()
        {
            _sortBuffer.Clear();
        }
    }
}