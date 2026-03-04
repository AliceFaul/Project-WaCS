using UnityEngine;

namespace _Project.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerContext Context { get; private set; }
        
        [SerializeField] private PlayerInteraction interaction;
        
        private void Awake()
        {
            Context = new PlayerContext(this);
            Context.StateMachine.ChangeState(new  PlayerIdleState(this));
            interaction.Init(Context);
        }

        private void Update()
        {
            Context.Movement.Move(Context.Input.Move);
            Context.StateMachine.UpdateState();
        }
    }
}