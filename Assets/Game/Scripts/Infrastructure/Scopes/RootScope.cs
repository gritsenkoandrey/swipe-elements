using SwipeElements.Infrastructure.Factories.AirBallonFactory;
using SwipeElements.Infrastructure.Factories.ElementFactory;
using SwipeElements.Infrastructure.Factories.LevelFactory;
using SwipeElements.Infrastructure.Factories.SystemFactory;
using SwipeElements.Infrastructure.Services.ApplicationService;
using SwipeElements.Infrastructure.Services.AssetService;
using SwipeElements.Infrastructure.Services.CameraService;
using SwipeElements.Infrastructure.Services.CoroutineService;
using SwipeElements.Infrastructure.Services.ExitApplicationService;
using SwipeElements.Infrastructure.Services.LoadingScreenService;
using SwipeElements.Infrastructure.Services.PhysicsService;
using SwipeElements.Infrastructure.Services.ProgressService;
using SwipeElements.Infrastructure.Services.ResultGameService;
using SwipeElements.Infrastructure.Services.SceneLoadService;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.StateMachines.StateMachineFactory;
using SwipeElements.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Scopes
{
    public sealed class RootScope : LifetimeScope
    {
        [SerializeField] private LoadingScreenService _loadingScreenService;
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private ScreenService _screenService;
        [SerializeField] private ExitApplicationService _exitApplicationService;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder
                .RegisterComponentInNewPrefab(_loadingScreenService, Lifetime.Singleton)
                .UnderTransform(transform)
                .As<ILoadingScreenService>();
            
            builder
                .RegisterComponentInNewPrefab(_coroutineService, Lifetime.Singleton)
                .UnderTransform(transform)
                .As<ICoroutineService>();
            
            builder
                .RegisterComponentInNewPrefab(_cameraService, Lifetime.Singleton)
                .UnderTransform(transform)
                .As<ICameraService>();
            
            builder
                .RegisterComponentInNewPrefab(_screenService, Lifetime.Singleton)
                .UnderTransform(transform)
                .As<IScreenService>();
            
            builder
                .RegisterComponentInNewPrefab(_exitApplicationService, Lifetime.Singleton)
                .UnderTransform(transform)
                .As<IExitApplicationService>();

            builder.Register<ILevelFactory, LevelFactory>(Lifetime.Singleton);
            builder.Register<IAirBallonFactory, AirBallonFactory>(Lifetime.Singleton);
            builder.Register<IElementFactory, ElementFactory>(Lifetime.Singleton);
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
            builder.Register<ISceneLoadService, SceneLoadService>(Lifetime.Singleton);
            builder.Register<IAssetService, AssetService>(Lifetime.Singleton);
            builder.Register<IProgressService, ProgressService>(Lifetime.Singleton);
            builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
            builder.Register<IPhysicsService, PhysicsService>(Lifetime.Singleton);
            builder.Register<IResultGameService, ResultGameService>(Lifetime.Singleton);
            builder.Register<IApplicationService, ApplicationService>(Lifetime.Singleton);
            builder.Register<IStateMachineFactory, StateMachineFactory>(Lifetime.Singleton);
        }
    }
}