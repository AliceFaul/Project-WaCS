using UnityEngine;

public interface IState
{
    void EnterState() => Debug.Log("State Entered");
    void TickState();
    void ExitState() => Debug.Log("State Exited");
}

public enum PlayerState
{
    Idle,
    Move,
    Interact,
    BuildMode,
    Disable
}