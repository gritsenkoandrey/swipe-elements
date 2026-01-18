namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine
{
    public interface IEnterState : IExitState
    {
        void Enter();
    }
    
    public interface IEnterState<in T1> : IExitState
    {
        void Enter(T1 param);
    }

    public interface IEnterState<in T1, in T2> : IExitState
    {
        void Enter(T1 param1, T2 param2);
    }
    
    public interface IEnterState<in T1, in T2, in T3> : IExitState
    {
        void Enter(T1 param1, T2 param2, T3 param3);
    }
}