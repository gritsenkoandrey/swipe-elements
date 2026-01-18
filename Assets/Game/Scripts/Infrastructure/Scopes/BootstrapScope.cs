using SwipeElements.Infrastructure.Scopes.EntryPoints;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Scopes
{
    public sealed class BootstrapScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterEntryPoint<BootstrapEntryPoint>(Lifetime.Scoped).AsSelf().Build();
        }
    }
}