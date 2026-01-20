using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Components;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DelaySystem : ISystem
    {
        private Filter _filter;
        private Stash<DelayComponent> _delayStash;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter.With<DelayComponent>().Build();
            _delayStash = World.GetStash<DelayComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _filter)
            {
                ref DelayComponent delay = ref _delayStash.Get(entity);
                
                delay.time -= deltaTime;
                
                if (delay.time < 0f)
                {
                    _delayStash.Remove(entity);
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}