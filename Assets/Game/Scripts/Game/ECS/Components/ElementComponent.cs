using System;
using Scellecs.Morpeh;
using SwipeElements.Game.Views;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Components
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ElementComponent : IComponent
    {
        public ElementView view;
    }
}