using _Project.Core.StateMachine;
using UnityEngine;

namespace _Project.Core.Singleton
{
    public class GameController : PersistentSingleton<GameController>
    {
        private GameStateMachine _gameStateMachine;

        protected override void Awake()
        {
            base.Awake();
            _gameStateMachine = new GameStateMachine();
        }

        private void Update()
        {
            _gameStateMachine.Update();
        }
    }
}