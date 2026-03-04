namespace _Project.Gameplay.Player
{
    public class PlayerContext
    {
        public PlayerController Controller { get; }
        
        public IPlayerInput Input { get; }
        public IPlayerMovement Movement { get; }
        
        public PlayerStateMachine StateMachine { get; }

        public Inventory Inventory { get; }
        public Hotbar Hotbar { get; }

        public PlayerContext(PlayerController controller)
        {
            Controller = controller;
            
            Input = controller.GetComponent<IPlayerInput>();
            Movement = controller.GetComponent<IPlayerMovement>();
            
            StateMachine = new PlayerStateMachine();
            Inventory = new Inventory();
            Hotbar = new Hotbar(9);
        }
    }
}