using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CleanupSystem : ICleanupSystem
    {
        private Filter _filter;

        public World World { get; set; }
        
        public void OnAwake()
        {
            _filter = World.Filter.With<CleanupComponent>().Build();
        }
        
        public void OnUpdate(float deltaTime)
        {
        }
        
        public void Dispose()
        {
            _filter.RemoveAllEntities();
        }
    }
}