using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Infrastructure.Services.ResultGameService;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WinSystem : ISystem
    {
        private readonly IResultGameService _resultGameService;
        
        private Filter _winFilter;
        private Filter _elementFilter;
        private Stash<WinComponent> _winStash;
        
        public WinSystem(IResultGameService resultGameService)
        {
            _resultGameService = resultGameService;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _winFilter = World.Filter.With<WinComponent>().Build();
            _elementFilter = World.Filter.With<ElementComponent>().Build();
            _winStash = World.GetStash<WinComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_winFilter.IsEmpty())
            {
                return;
            }
            
            foreach (Entity entity in _winFilter)
            {
                _winStash.Remove(entity);
            }

            if (_elementFilter.IsEmpty())
            {
                _resultGameService.SendResultGame(true);
            }
        }
        
        public void Dispose()
        {
        }
    }
}