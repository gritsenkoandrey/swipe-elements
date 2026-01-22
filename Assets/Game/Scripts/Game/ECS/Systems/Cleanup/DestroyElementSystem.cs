using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems.Cleanup
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyElementSystem : ICleanupSystem
    {
        private Filter _destroyFilter;
        private Filter _gridFilter;
        private Stash<WinComponent> _winStash;
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _destroyFilter = World.Filter
                .With<ElementTag>()
                .With<DestroyComponent>()
                .Without<DelayComponent>()
                .Build();
            
            _gridFilter = World.Filter
                .With<GridTag>()
                .Build();
            
            World.GetStash<DestroyComponent>().AsDisposable();
            
            _winStash = World.GetStash<WinComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_destroyFilter.IsEmpty())
            {
                return;
            }
            
            foreach (Entity entity in _destroyFilter)
            {
                World.RemoveEntity(entity);
            }

            foreach (Entity entity in _gridFilter)
            {
                _winStash.AddOrGet(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}