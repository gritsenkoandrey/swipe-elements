using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems.Initialize
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridInitializeSystem : ISystem
    {
        private Filter _gridFilter;
        private Stash<NormalizeComponent> _normalizeStash;
        private Stash<MergeComponent> _mergeStash;
        private Stash<InitializeComponent> _initializeStash;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _gridFilter = World.Filter
                .With<GridTag>()
                .Without<InitializeComponent>()
                .Build();
            
            _normalizeStash = World.GetStash<NormalizeComponent>();
            _mergeStash = World.GetStash<MergeComponent>();
            _initializeStash = World.GetStash<InitializeComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _gridFilter)
            {
                _normalizeStash.Add(entity);
                _mergeStash.Add(entity);
                _initializeStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}