using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems.Initialize
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridInitializeSystem : IInitializer
    {
        private Filter _gridFilter;
        private Stash<NormalizeComponent> _normalizeStash;
        private Stash<MergeComponent> _mergeStash;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _gridFilter = World.Filter
                .With<GridTag>()
                .Build();
            
            _normalizeStash = World.GetStash<NormalizeComponent>();
            _mergeStash = World.GetStash<MergeComponent>();
            
            foreach (Entity entity in _gridFilter)
            {
                _normalizeStash.Add(entity);
                _mergeStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}