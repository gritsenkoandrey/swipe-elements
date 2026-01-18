using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyElementSystem : ICleanupSystem
    {
        private Filter _destroyFilter;
        private Filter _gridFilter;
        private Stash<DestroyComponent> _destroyStash;
        private Stash<NormalizeComponent> _normalizeStash;
        private Stash<WinComponent> _winStash;
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _destroyFilter = World.Filter
                .With<ElementComponent>()
                .With<DestroyComponent>()
                .Without<DelayComponent>()
                .Build();
            
            _gridFilter = World.Filter
                .With<GridComponent>()
                .Build();
            
            _destroyStash = World.GetStash<DestroyComponent>();
            _normalizeStash = World.GetStash<NormalizeComponent>();
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
                ref DestroyComponent destroy = ref _destroyStash.Get(entity);
                destroy.Dispose();
                
                World.RemoveEntity(entity);
            }
            
            Entity gridEntity = _gridFilter.First();

            _normalizeStash.AddOrGet(gridEntity);
            _winStash.AddOrGet(gridEntity);
        }
        
        public void Dispose()
        {
        }
    }
}