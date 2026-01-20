using System;
using Scellecs.Morpeh;
using SwipeElements.Game.Views;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Tags
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct GridTag : IComponent
    {
        public GridView view;
    }
}