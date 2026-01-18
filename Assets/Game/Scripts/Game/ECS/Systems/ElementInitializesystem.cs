using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ElementInitializeSystem : ISystem
    {
        private readonly ElementConfig _config;
        
        private Filter _elementFilter;
        private Stash<IdComponent> _idStash;
        private Stash<ElementComponent> _elementStash;
        private Stash<CleanupComponent> _cleanupStash;

        public ElementInitializeSystem(IStaticDataService staticDataService)
        {
            _config = staticDataService.GetElementConfig();
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _elementFilter = World.Filter
                .With<ElementComponent>()
                .With<TransformComponent>()
                .Without<IdComponent>()
                .Build();
            
            _idStash = World.GetStash<IdComponent>();
            _elementStash = World.GetStash<ElementComponent>();
            _cleanupStash = World.GetStash<CleanupComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _elementFilter)
            {
                ref ElementComponent element = ref _elementStash.Get(entity);
                ref IdComponent id = ref _idStash.Add(entity);

                id.id = element.view.Id.GetHashCode();
                element.view.StartIdleAnimation(_config.IdleAnimationTime);

                _cleanupStash.Add(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}