using SwipeElements.Game.ECS.Providers;
using UnityEngine;

namespace SwipeElements.Infrastructure.Factories.AirBallonFactory
{
    public interface IAirBallonFactory
    {
        AirBallonProvider CreateAirBallon(Vector3 position, Transform parent);
    }
}