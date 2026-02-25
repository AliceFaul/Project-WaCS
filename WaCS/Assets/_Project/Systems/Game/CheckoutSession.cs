using System.Collections.Generic;

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
        private readonly List<ItemData> _items = new();

        public string CustomerId { get; }
        public CheckoutState State { get; private set; }

        public decimal TotalPrice { get; private set; }
        public decimal Paid { get; private set; }
        public decimal GetChange()
        {
            return Paid >= TotalPrice ? Paid - TotalPrice : 0;
        }

        public IReadOnlyList<ItemData> Items { get { return _items; } }

        // Constructor của một Session
        public CheckoutSession(string customerId)
        {
            CustomerId = customerId;
            State = CheckoutState.Scanning;
        }
        
        public bool TryAddItem(ItemData item)
        {
            if(State != CheckoutState.Scanning) return false;
            _items.Add(item);
            TotalPrice += (decimal)item.SellPrice;
            return true;
        }

        public bool TryFinishScan()
        {
            if(State != CheckoutState.Scanning) return false;
            State = CheckoutState.WaitingForPayment;
            return true;
        }

        public bool TryPay(decimal money)
        {
            if(State != CheckoutState.WaitingForPayment) return false;
            Paid += money;
            return true;
        }

        public bool TryComplete(out CheckoutResult result)
        {
            result = default;
            if(State != CheckoutState.WaitingForPayment)
                return false;
            if(Paid < TotalPrice)
                return false;

            State = CheckoutState.Completed;
            result = new CheckoutResult(this);
            return true;
        }
    }
}