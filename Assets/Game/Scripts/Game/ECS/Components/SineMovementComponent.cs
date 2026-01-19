using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Components
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SineMovementComponent : IComponent
    {
        public float amplitude;
        public float frequency;
    }
}