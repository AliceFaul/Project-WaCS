using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Core.Event;

namespace _Project.Systems.Game
{
    // tạo một struct dùng để lưu lại kết quả khi hoàn thành
    public readonly struct CheckoutResult
    {
        public readonly string Customer;
        public readonly decimal Change;
        public readonly decimal Total;
        public readonly decimal Paid;
        public readonly IReadOnlyList<ItemData> Items;

        public CheckoutResult(CheckoutSession session)
        {
            Customer = session.CustomerId;
            Change = session.GetChange();
            Total = session.TotalPrice;
            Paid = session.Paid;
            Items = session.Items.ToList();
        }
    }

    public readonly struct CheckoutStarted
    {
        public readonly string CustomerId; // id của customer

        public CheckoutStarted(string customerId)
        {
            CustomerId = customerId;
        }
    }

    public readonly struct CheckoutItemScanned
    {
        public readonly string ItemId; // id của item
        public readonly decimal ItemPrice; // giá của item
        public readonly decimal NewTotal; // tổng tiền mới khi scan item
        public readonly int ItemCount; // số lượng item trong lần scan

        public CheckoutItemScanned(string itemId, decimal itemPrice, decimal newTotal, int itemCount)
        {
            ItemId = itemId;
            ItemPrice = itemPrice;
            NewTotal = newTotal;
            ItemCount = itemCount;
        }
    }

    public readonly struct CheckoutScanFinished
    {
        public readonly decimal FinalTotal; // tổng tiền cuối cùng
        public readonly int ItemCount; // số lượng item sau khi scan xong hết

        public CheckoutScanFinished(decimal finalTotal, int itemCount)
        {
            FinalTotal = finalTotal;
            ItemCount = itemCount;
        }
    }

    public readonly struct CheckoutPaymentReceived
    {
        public readonly decimal PaidAmount; // số tiền customer trả
        public readonly decimal TotalPaid; // tổng tiền đã trả đến thời điểm hiện tại
        public readonly decimal Remaining; // tiền còn thiếu

        public CheckoutPaymentReceived(decimal paidAmount, decimal totalPaid, decimal remaining)
        {
            PaidAmount = paidAmount;
            TotalPaid = totalPaid;
            Remaining = remaining;
        }
    }

    public readonly struct CheckoutCompleted
    {
        public readonly CheckoutResult Result;
        public CheckoutCompleted(CheckoutResult result)
        {
            Result = result;
        }
    }

    public readonly struct CheckoutFailed
    {
        public readonly string CustomerId;
        public readonly string Reason;
        public CheckoutFailed(string customerId, string reason)
        {
            CustomerId = customerId;
            Reason = reason;
        }
    }

    // Core logic của hệ thống thanh toán cho customer
    public class CheckoutSystem : IManager
    {
        private CheckoutSession _currentSession;
        private readonly EventManager _eventManager;

        public CheckoutSystem(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public async Task<bool> InitAsync()
        {
            await Task.CompletedTask;
            return true;
        }

        public bool StartCheckout(string customerId)
        {
            if(_currentSession != null) return false;
            _currentSession = new CheckoutSession(customerId);
            _eventManager.Publish(new CheckoutStarted(customerId));
            return true;
        }

        public bool TryScanItem(ItemData item)
        {
            if(_currentSession == null || item == null)
                return false;
            if(!_currentSession.TryAddItem(item))
                return false;

            var total = _currentSession.TotalPrice;
            var count = _currentSession.Items.Count;
            _eventManager.Publish(new CheckoutItemScanned(item.ItemID, (decimal)item.Price, total, count));
            return true;
        }

        public bool TryFinishScan()
        {
            if(_currentSession == null) 
                return false;
            if(!_currentSession.TryFinishScan())
                return false;

            var finalTotal = _currentSession.TotalPrice;
            var itemCount = _currentSession.Items.Count;
            _eventManager.Publish(new CheckoutScanFinished(finalTotal, itemCount));
            return true;
        }

        public bool TryReceiveMoney(decimal money)
        {
            if(_currentSession == null || money <= 0)
                return false;
            if(!_currentSession.TryPay(money))
                return false;

            var paid = _currentSession.Paid;
            var total = _currentSession.TotalPrice;
            _eventManager.Publish(new CheckoutPaymentReceived(
                money,
                paid,
                Math.Max(total - paid, 0)
            ));
            return true;
        }

        public bool TryCompleteCheckout(out CheckoutResult result)
        {
            result = default;
            if (_currentSession == null)
                return false;
            if (!_currentSession.TryComplete(out result))
                return false;

            _eventManager.Publish(new CheckoutCompleted(result));
            _currentSession = null;
            return true;
        }

        public void CancelCheckout()
        {
            if (_currentSession == null)
                return;
            var customerId = _currentSession.CustomerId;
            _currentSession = null;
            _eventManager.Publish(new CheckoutFailed(customerId, "Checkout cancelled by user"));
        }
    }
}