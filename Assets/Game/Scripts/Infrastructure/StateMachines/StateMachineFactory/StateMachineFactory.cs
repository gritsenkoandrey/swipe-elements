using JetBrains.Annotations;
using SwipeElements.Infrastructure.StateMachines.GameStateMachine;
using VContainer;

namespace SwipeElements.Infrastructure.StateMachines.StateMachineFactory
{
    [UsedImplicitly]
    public sealed class StateMachineFactory : IStateMachineFactory
    {
        private readonly IObjectResolver _objectResolver;

        public StateMachineFactory(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        IGameStateMachine IStateMachineFactory.CreateGameStateMachine()
        {
            GameStateMachine.GameStateMachine gameStateMachine = new ();

            foreach (IExitState value in gameStateMachine.States.Values)
            {
                _objectResolver.Inject(value);
            }
            
            return gameStateMachine;
        }
    }
}