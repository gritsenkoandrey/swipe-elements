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
        
        private Filter _filter;
        private Stash<ElementComponent> _elementStash;
        private Stash<WinComponent> _winStash;
        
        public WinSystem(IResultGameService resultGameService)
        {
            _resultGameService = resultGameService;
        }

        public World World { get; set; }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<WinComponent>()
                .Build();
            
            _elementStash = World.GetStash<ElementComponent>();
            _winStash = World.GetStash<WinComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _filter)
            {
                _winStash.Remove(entity);
                
                if (_elementStash.IsEmpty())
                {
                    _resultGameService.SendResultGame(true);
                    
                    return;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}