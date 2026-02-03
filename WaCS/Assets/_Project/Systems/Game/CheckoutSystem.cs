using System;
using System.Threading.Tasks;
using _Project.Gameplay.Customer;

namespace _Project.Systems.Game
{
    // tạo một struct dùng để lưu lại kết quả khi hoàn thành
    public readonly struct CheckoutResult
    {
        public readonly Customer Customer;
        public readonly float Change;
        public readonly float Total;
        public readonly float Paid;

        public CheckoutResult(CheckoutSession session)
        {
            Customer = session.Customer;
            Change = session.Change;
            Total = session.TotalPrice;
            Paid = session.Paid;
        }
    }

    // Core logic của hệ thống thanh toán cho customer
    public class CheckoutSystem : IManager
    {
        private CheckoutSession _currentSession;
        private bool IsBusy => _currentSession != null;
        public bool IsReady { get; private set; }

        // checkout event
        public event Action<CheckoutSession> OnCheckoutStarted;
        public event Action<string, float> OnItemScanned;
        public event Action<CheckoutResult> OnCheckoutCompleted;
        public event Action<CheckoutSession> OnCheckoutFailed;

        public CheckoutSession CurrentSession { get { return _currentSession; } }

        public async Task<bool> InitAsync()
        {
            await Task.CompletedTask;
            IsReady = true;
            return true;
        }

        // bắt đầu một session thanh toán
        public bool StartCheckout(Customer customer)
        {
            // nếu không có customer hoặc đang trong một session rồi
            // thì không thể bắt đầu một session mới
            if (customer == null || IsBusy || _currentSession != null) return false;
            _currentSession = new CheckoutSession(customer);
            OnCheckoutStarted?.Invoke(_currentSession);
            return true;
        }

        public bool TryScanItem(ItemData item)
        {
            if(_currentSession == null) return false;
            _currentSession.Scan(item.Price);
            OnItemScanned?.Invoke(item.ItemID, item.Price);
            return true;
        }

        // hàm hoàn thành việc scanning item
        public void FinishScanning()
        {
            if(_currentSession == null) return;
            _currentSession.ScanCompleted();
        }

        // hàm nhận tiền từ customer
        public void ReceiveMoney(float money)
        {
            if(_currentSession == null) return;
            _currentSession.Pay(money);
        }

        // hàm xác nhận có thối tiền không và hoàn thành 1 session thanh toán
        public bool GiveChange(float amount)
        {
            if(_currentSession == null) return false;
            if(!_currentSession.GiveChange(amount))
            {
                FailCheckout();
                return false;
            }
            CompleteCheckout();
            return true;
        }

        private void CompleteCheckout()
        {
            _currentSession.CompleteSession();
            OnCheckoutCompleted?.Invoke(new CheckoutResult(_currentSession));
            _currentSession = null;
        }
        
        public void FailCheckout()
        {
            if(_currentSession == null) return;
            _currentSession.FailSession();
            OnCheckoutFailed?.Invoke(_currentSession);
            _currentSession = null;
        }
    }
}