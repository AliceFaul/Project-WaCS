using _Project.Gameplay.Customer;

namespace _Project.Systems.Game
{
    public enum CheckoutState
    {
        Idle,
        Waiting,
        Scanning,
        WaitingForPayment,
        WaitingForChange,
        Completed,
        Failed
    }

    public class CheckoutSession
    {
        public Customer Customer { get; }
        public CheckoutState State { get; private set; }

        public float TotalPrice { get; private set; }
        public float Paid { get; private set; }
        public float Change => Paid - TotalPrice;


        // Constructor của một Session
        public CheckoutSession(Customer customer)
        {
            Customer = customer;
            State = CheckoutState.Scanning;
            Paid = 0f;
            TotalPrice = 0f;
        }
        
        // hoàn tất scan item và bắt đầu tính tiền
        public void ScanCompleted()
        {
            if (State != CheckoutState.Scanning) return;
            State = CheckoutState.WaitingForPayment;
        }
        
        public void CompleteSession() => State = CheckoutState.Completed;
        
        public void FailSession() => State = CheckoutState.Failed;
        
        // nhận tiền từ customer
        public void Pay(float amount)
        {
            if(State != CheckoutState.WaitingForPayment) return;
            Paid += amount;
        }

        // scan item và cập nhật tổng tiền thanh toán
        public void Scan(float price)
        {
            if (State != CheckoutState.Scanning) return;
            TotalPrice += price;
        }

        public bool IsPaymentEnough() => Paid >= TotalPrice;

        public bool GiveChange(float amount)
        {
            if(State != CheckoutState.WaitingForChange) return false;
            if(amount < Change) return false;
            State = CheckoutState.Completed;
            return true;
        }
    }
}