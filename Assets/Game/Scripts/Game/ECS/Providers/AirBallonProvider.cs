using Scellecs.Morpeh.Providers;
using SwipeElements.Game.ECS.Tags;
using Unity.IL2CPP.CompilerServices;

namespace SwipeElements.Game.ECS.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AirBallonProvider : MonoProvider<AirBallonTag>
    {
    }
}