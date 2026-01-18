using SwipeElements.Infrastructure.StateMachines.GameStateMachine;

namespace SwipeElements.Infrastructure.StateMachines.StateMachineFactory
{
    public interface IStateMachineFactory
    {
        IGameStateMachine CreateGameStateMachine();
    }
}