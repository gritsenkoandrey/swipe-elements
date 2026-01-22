using System;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Game.Extensions;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.StaticDataService;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems.Update
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NormalizeElementSystem : ISystem
    {
        private Filter _elementFilter;
        private Filter _gridFilter;
        private Filter _normalizeFilter;
        private Filter _moveFilter;
        private Filter _destroyFilter;
        private Stash<ElementTag> _elementStash;
        private Stash<MoveComponent> _moveStash;
        private Stash<SpeedComponent> _speedStash;
        private Stash<GridTag> _gridStash;
        private Stash<NormalizeComponent> _normalizeStash;
        
        private readonly float _gravitySpeed;
        private readonly FastList<SortableEntity> _sortBuffer = new ();

        public NormalizeElementSystem(IStaticDataService staticDataService)
        {
            _gravitySpeed = staticDataService.GetElementConfig().GravitySpeed;
        }
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _elementFilter = World.Filter.With<ElementTag>().Build();
            _gridFilter = World.Filter.With<GridTag>().Build();
            _normalizeFilter = World.Filter.With<NormalizeComponent>().Build();
            _destroyFilter = World.Filter.With<DestroyComponent>().Build();
            _moveFilter = World.Filter.With<MoveComponent>().Build();
            _normalizeStash = World.GetStash<NormalizeComponent>();
            _elementStash = World.GetStash<ElementTag>();
            _moveStash = World.GetStash<MoveComponent>();
            _gridStash = World.GetStash<GridTag>();
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
                ref ElementTag element = ref _elementStash.Get(entity);
                
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
                    ref ElementTag element = ref _elementStash.Get(entity);
                    element.view.SetPosition(new (currentX, targetY));
                    
                    ref MoveComponent move = ref _moveStash.AddOrGet(entity);
                    move.to = gridView.GetCenter(currentX, targetY);
                    
                    ref SpeedComponent speed = ref _speedStash.AddOrGet(entity);
                    speed.speed = _gravitySpeed;
                }

                targetY++;
            }
            
            _sortBuffer.Clear();
        }
        
        public void Dispose()
        {
            _sortBuffer.Clear();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GridView GetGridView()
        {
            Entity entity = _gridFilter.First();
            
            ref GridTag grid = ref _gridStash.Get(entity);
            
            return grid.view;
        }
        
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
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
    }
}