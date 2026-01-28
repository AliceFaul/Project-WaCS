using UnityEngine;

public interface IGameState
{
    void Enter() => Debug.Log("Game State Entered");
    void Tick();
    void Exit() => Debug.Log("Game State Exited");
}