using System;
using System.Collections.Generic;
using SwipeElements.Infrastructure.StateMachines.GameStateMachine.States;

namespace SwipeElements.Infrastructure.StateMachines.GameStateMachine
{
    public sealed class GameStateMachine : IGameStateMachine
    {
        public IReadOnlyDictionary<Type, IExitState> States { get; }

        private IExitState _activeState;

        public GameStateMachine()
        {
            States = new Dictionary<Type, IExitState>
            {
                { typeof(BootstrapState), new BootstrapState(this) },
                { typeof(PrepareState), new PrepareState(this) },
                { typeof(LoadLevelState), new LoadLevelState(this) },
                { typeof(GameState), new GameState(this) },
                { typeof(ResultState), new ResultState(this) },
                { typeof(RestartState), new RestartState(this) },
            };
        }

        void IGameStateMachine.Enter<TState>() => 
            ChangeState<TState>().Enter();

        void IGameStateMachine.Enter<TState, T1>(T1 param1) => 
            ChangeState<TState>().Enter(param1);
        
        void IGameStateMachine.Enter<TState, T1, T2>(T1 param1, T2 param2) => 
            ChangeState<TState>().Enter(param1, param2);

        void IGameStateMachine.Enter<TState, T1, T2, T3>(T1 param1, T2 param2, T3 param3) => 
            ChangeState<TState>().Enter(param1, param2, param3);

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            _activeState?.Exit();
            _activeState = States[typeof(TState)];
            
            return _activeState as TState;
        }
    }
}