using JetBrains.Annotations;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using SwipeElements.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Factories.AirBallonFactory
{
    [UsedImplicitly]
    public sealed class AirBallonFactory : IAirBallonFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IObjectResolver _objectResolver;
        
        public AirBallonFactory(IStaticDataService staticDataService, IObjectResolver objectResolver)
        {
            _staticDataService = staticDataService;
            _objectResolver = objectResolver;
        }

        AirBallonProvider IAirBallonFactory.CreateAirBallon(Vector3 position, Transform parent)
        {
            AirBalloonConfig config = _staticDataService.GetAirBallonConfig();

            return _objectResolver.Instantiate(config.AirBallons.GetRandomElement(), position, Quaternion.identity, parent);
        }
    }
}