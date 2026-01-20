using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using SwipeElements.Infrastructure.Services.StaticDataService;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ElementInitializeSystem : IInitializer
    {
        private Filter _elementFilter;
        private Stash<IdComponent> _idStash;
        private Stash<ElementTag> _elementStash;
        private Stash<CleanupComponent> _cleanupStash;
        
        private readonly float _idleAnimationTime;

        public ElementInitializeSystem(IStaticDataService staticDataService)
        {
            _idleAnimationTime = staticDataService.GetElementConfig().IdleAnimationTime;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _elementFilter = World.Filter
                .With<ElementTag>()
                .Build();
            
            _idStash = World.GetStash<IdComponent>();
            _elementStash = World.GetStash<ElementTag>();
            _cleanupStash = World.GetStash<CleanupComponent>();
            
            foreach (Entity entity in _elementFilter)
            {
                ref ElementTag element = ref _elementStash.Get(entity);
                ref IdComponent id = ref _idStash.Add(entity);

                id.id = element.view.Id.GetHashCode();
                element.view.StartIdleAnimation(_idleAnimationTime);

                _cleanupStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}