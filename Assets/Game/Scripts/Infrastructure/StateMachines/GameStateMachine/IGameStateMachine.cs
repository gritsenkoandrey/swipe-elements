namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IEnterState;
        void Enter<TState, T1>(T1 param) where TState : class, IEnterState<T1>;
        void Enter<TState, T1, T2>(T1 param1, T2 param2) where TState : class, IEnterState<T1, T2>;
        void Enter<TState, T1, T2, T3>(T1 param1, T2 param2, T3 param3) where TState : class, IEnterState<T1, T2, T3>;
    }
}