using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridInitializeSystem : IInitializer
    {
        private Filter _gridFilter;
        private Filter _elementFilter;
        private Stash<ElementComponent> _elementStash;
        private Stash<GridComponent> _gridStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<CleanupComponent> _cleanupStash;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _elementFilter = World.Filter
                .With<ElementComponent>()
                .With<TransformComponent>()
                .Build();
            
            _gridFilter = World.Filter
                .With<GridComponent>()
                .Build();
            
            _elementStash = World.GetStash<ElementComponent>();
            _gridStash = World.GetStash<GridComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _cleanupStash = World.GetStash<CleanupComponent>();
            
            foreach (Entity entity in _gridFilter)
            {
                ref GridComponent grid = ref _gridStash.Get(entity);
                
                Vector3 origin = grid.view.GetOrigin();
                
                foreach (Entity elementEntity in _elementFilter)
                {
                    ref ElementComponent element = ref _elementStash.Get(elementEntity);
                    ref TransformComponent transform = ref _transformStash.Get(elementEntity);
                    
                    transform.transform.localScale = Vector3.one * grid.view.CellSize;
                    transform.transform.position = grid.view.GetCenter(origin, element.view.Position.x, element.view.Position.y);
                }

                _cleanupStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}