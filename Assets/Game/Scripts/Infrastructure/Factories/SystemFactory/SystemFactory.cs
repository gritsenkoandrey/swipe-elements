using JetBrains.Annotations;
using Scellecs.Morpeh;
using SwipeElements.Game.ECS.Systems;
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

            _systemsGroup.AddInitializer(new GridInitializeSystem());
            _systemsGroup.AddInitializer(new ElementInitializeSystem(_staticDataService));
            
            _systemsGroup.AddSystem(new DelaySystem());
            _systemsGroup.AddSystem(new InputSystem(_physicsService, _cameraService));
            _systemsGroup.AddSystem(new SwipeElementSystem(_staticDataService));
            _systemsGroup.AddSystem(new MoveElementSystem());
            _systemsGroup.AddSystem(new NormalizeElementSystem(_staticDataService));
            _systemsGroup.AddSystem(new MergeElementSystem(_staticDataService));
            _systemsGroup.AddSystem(new AirBallonInitializeSystem(_staticDataService));
            _systemsGroup.AddSystem(new AirBallonMoveSystem(_cameraService));
            _systemsGroup.AddSystem(new WinSystem(_resultGameService));
            
            _systemsGroup.AddSystem(new DestroyElementSystem());
            _systemsGroup.AddSystem(new CleanupSystem());

            _world.AddSystemsGroup(order: 0, _systemsGroup);
        }
        
        void ISystemFactory.Cleanup()
        {
            if (_systemsGroup != null)
            {
                _world.RemoveSystemsGroup(_systemsGroup);
                _systemsGroup = null;
            }
        }
    }
}