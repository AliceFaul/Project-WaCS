using UnityEngine;

namespace _Project.Gameplay.Player
{
    public class PlayerMoveState : IState
    {
        private readonly PlayerController _controller;
        
        public PlayerMoveState(PlayerController controller)
        {
            _controller = controller;
        }
        
        public void EnterState()
        {
            Debug.Log("Entered PlayerMoveState");
        }

        public void TickState()
        {
            if (_controller.Context.Input.Move == Vector2.zero)
            {
                _controller.Context.StateMachine.ChangeState(new  PlayerIdleState(_controller));
            }
        }

        public void ExitState()
        {
            Debug.Log("Exited PlayerMoveState");
        }
    }
}