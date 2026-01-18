using System;
using SwipeElements.Infrastructure.Factories.SystemFactory;
using SwipeElements.Infrastructure.Scopes.EntryPoints;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Scopes
{
    public sealed class GameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Scoped).As<IDisposable>();

            builder.RegisterEntryPoint<GameEntryPoint>(Lifetime.Scoped).AsSelf().Build();
        }
    }
}