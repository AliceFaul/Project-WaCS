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
        private Cart Cart { get; }
        public CheckoutState State { get; private set; }

        public float TotalPrice
        {
            get
            {
                return Cart.TotalPrice;
            }
            private set { }
        }
        public float Paid
        {
            get
            {
                return _paidAmount;
            }
            private set { }
        }

        private float _paidAmount;

        public CheckoutSession(Customer customer)
        {
            Customer = customer;
            Cart = customer.Cart;
            State = CheckoutState.Scanning;
        }
        
        public void ScanCompleted() => State = CheckoutState.WaitingForPayment;
        
        public void CompleteSession() => State = CheckoutState.Completed;
        
        public void FailSession() => State = CheckoutState.Failed;
        
        public void Pay(float amount)
        {
            _paidAmount += amount;
        }
        
        public bool IsPaymentEnough() => _paidAmount >= TotalPrice;

        public float Change => Paid - TotalPrice;
    }
}