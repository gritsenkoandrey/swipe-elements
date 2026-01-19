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
            
            builder.RegisterEntryPoint<GameEntryPoint>(Lifetime.Scoped).AsSelf().Build();
        }
    }
}