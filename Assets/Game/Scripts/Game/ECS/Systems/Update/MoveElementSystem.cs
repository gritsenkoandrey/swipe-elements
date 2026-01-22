using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Components;
using SwipeElements.Game.ECS.Tags;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.ECS.Systems.Update
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveElementSystem : ISystem
    {
        private Filter _filter;
        private Stash<MoveComponent> _moveStash;
        private Stash<SpeedComponent> _speedStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<MergeComponent> _mergeStash;
        private Stash<NormalizeComponent> _normalizeStash;
        
        public World World { get; set; }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<ElementTag>()
                .With<MoveComponent>()
                .With<SpeedComponent>()
                .With<TransformComponent>()
                .Build();
            
            _moveStash = World.GetStash<MoveComponent>();
            _speedStash = World.GetStash<SpeedComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _mergeStash = World.GetStash<MergeComponent>();
            _normalizeStash = World.GetStash<NormalizeComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _filter)
            {
                ref MoveComponent move = ref _moveStash.Get(entity);
                ref SpeedComponent speed = ref _speedStash.Get(entity);
                ref TransformComponent transform = ref _transformStash.Get(entity);
                
                Vector3 current = transform.transform.position;
                Vector3 next = Vector3.MoveTowards(current, move.to, speed.speed * deltaTime);
                float distance = Vector3.Distance(current, move.to);
                
                if (distance < Mathf.Epsilon)
                {
                    transform.transform.position = move.to;
                    
                    _moveStash.Remove(entity);
                    _speedStash.Remove(entity);
                    _mergeStash.AddOrGet(entity);
                    _normalizeStash.AddOrGet(entity);
                }
                else
                {
                    transform.transform.position = next;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}