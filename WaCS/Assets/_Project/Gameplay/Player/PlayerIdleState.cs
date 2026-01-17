using UnityEngine;

namespace _Project.Gameplay.Player
{
    public class PlayerIdleState : IState
    {
        private readonly PlayerController _controller;
        
        public PlayerIdleState(PlayerController controller)
        {
            _controller = controller;
        }
        
        public void EnterState()
        {
            Debug.Log("Entered PlayerIdleState");
        }

        public void TickState()
        {
            if (_controller.Context.Input.Move != Vector2.zero)
            {
                _controller.Context.StateMachine.ChangeState(new PlayerMoveState(_controller));
            }
        }

        public void ExitState()
        {
            Debug.Log("Exited PlayerIdleState");
        }
    }
}