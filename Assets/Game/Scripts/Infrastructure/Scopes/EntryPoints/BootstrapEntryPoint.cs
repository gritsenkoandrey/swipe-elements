using JetBrains.Annotations;
using SwipeElements.Infrastructure.StateMachines.GameStateMachine.States;
using SwipeElements.Infrastructure.StateMachines.StateMachineFactory;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Scopes.EntryPoints
{
    [UsedImplicitly]
    public sealed class BootstrapEntryPoint : IInitializable
    {
        private readonly IStateMachineFactory _stateMachineFactory;

        public BootstrapEntryPoint(IStateMachineFactory stateMachineFactory)
        {
            _stateMachineFactory = stateMachineFactory;
        }

        void IInitializable.Initialize() => _stateMachineFactory.CreateGameStateMachine().Enter<BootstrapState>();
    }
}