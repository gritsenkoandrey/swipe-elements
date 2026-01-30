using JetBrains.Annotations;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using SwipeElements.Game.ECS.Systems.Cleanup;
using SwipeElements.Game.ECS.Systems.Initialize;
using SwipeElements.Game.ECS.Systems.Update;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.PhysicsService;
using SwipeElements.Infrastructure.Services.ResultGameService;
using SwipeElements.Infrastructure.Services.StaticDataService;

namespace SwipeElements.Infrastructure.Factories.SystemFactory
{
    [UsedImplicitly]
    public sealed class SystemFactory : ISystemFactory
    {
        private World _world;
        private SystemsGroup _systemsGroup;
        
        private readonly IPhysicsService _physicsService;
        private readonly ICameraService _cameraService;
        private readonly IStaticDataService _staticDataService;
        private readonly IResultGameService _resultGameService;

        public SystemFactory
        (
            IPhysicsService physicsService, 
            ICameraService cameraService, 
            IStaticDataService staticDataService, 
            IResultGameService resultGameService
        )
        {
            _physicsService = physicsService;
            _cameraService = cameraService;
            _staticDataService = staticDataService;
            _resultGameService = resultGameService;
        }

        void ISystemFactory.Init()
        {
            _world = World.Default;
        }

        void ISystemFactory.CreateGameSystems()
        {
            _systemsGroup = _world.CreateSystemsGroup();
            _systemsGroup.AddSystem(new GridInitializeSystem());
            _systemsGroup.AddSystem(new ElementInitializeSystem(_staticDataService));
            _systemsGroup.AddSystem(new AirBallonInitializeSystem(_staticDataService));
            _systemsGroup.AddSystem(new DelaySystem());
            _systemsGroup.AddSystem(new InputSystem(_physicsService, _cameraService, _staticDataService));
            _systemsGroup.AddSystem(new SwipeElementSystem(_staticDataService));
            _systemsGroup.AddSystem(new MoveElementSystem());
            _systemsGroup.AddSystem(new NormalizeElementSystem(_staticDataService));
            _systemsGroup.AddSystem(new MergeElementSystem(_staticDataService));
            _systemsGroup.AddSystem(new AirBallonMoveSystem(_cameraService));
            _systemsGroup.AddSystem(new WinSystem(_resultGameService));            
            _systemsGroup.AddSystem(new DestroyElementSystem());
            _world.AddSystemsGroup(order: 0, _systemsGroup);
            _world.Commit();
        }
        
        void ISystemFactory.Cleanup()
        {
            if (_systemsGroup != null)
            {
                Filter filter = _world.Filter.Build();
                filter.Dispose();
                filter = _world.Filter.Build();
                filter.RemoveAllEntities();
                _world.Commit();
                _world.RemoveSystemsGroup(_systemsGroup);
                _systemsGroup = null;
            }
        }
    }
}