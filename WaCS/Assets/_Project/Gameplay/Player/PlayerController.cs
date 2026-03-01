using UnityEngine;
using System;

namespace _Project.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerContext Context { get; private set; }
        
        [SerializeField] private PlayerInteraction interaction;

        public static event Action<PlayerContext> OnContextInitialized;

        private void Awake()
        {
            Context = new PlayerContext(this);
            OnContextInitialized?.Invoke(Context);
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