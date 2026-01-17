using UnityEngine;

namespace _Project.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerContext Context { get; private set; }
        
        private void Awake()
        {
            Context = new PlayerContext(this);
            Context.StateMachine.ChangeState(new  PlayerIdleState(this));
        }

        private void Update()
        {
            Context.Movement.Move(Context.Input.Move);
            Context.StateMachine.UpdateState();
        }
    }
}