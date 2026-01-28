using _Project.Gameplay.Customer;

namespace _Project.Systems.Game
{
    public enum CheckoutState
    {
        Idle,
        Waiting,
        Scanning,
        WaitingForPayment,
        Completed,
        Failed
    }
    
    public class CheckoutSession
    {
        public Customer Customer { get; }
    }
}