using System;
using System.Collections.Generic;

public class StateMachine
{
    public Dictionary<Type, IState> StateMap;
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = newState;
        CurrentState.Enter();
    }

    public IState GetState<T>() where T : IState
    {
        var type = typeof(T);
        return StateMap[type];
    }
}
