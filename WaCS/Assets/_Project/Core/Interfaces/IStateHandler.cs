using System;

public interface IStateHandler
{
    void ChangeState(IState newState);
}